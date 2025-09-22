using Application.Features.Identity.Roles;
using Application.Features.Identity.Users;
using Azure.Core;
using Domain.Entities;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Common;
using Infrastructure.Identity;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading;

namespace Infrastructure.Persistence.DbInitializers
{
    public class ApplicationDbInitializer(
        IMultiTenantContextAccessor<CIMTenantInfo> tenantInfoContextAccessor,
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService,
        ApplicationDbContext applicationDbContext, IConfiguration configuration)
    {
        RoleService roleService = new(roleManager, userManager, applicationDbContext, currentUserService, tenantInfoContextAccessor, configuration);
        public async Task InitializeDatabaseAsync(CancellationToken cancellationToken)
        {
            if (applicationDbContext.Database.GetMigrations().Any())
            {
                if ((await applicationDbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    await applicationDbContext.Database.MigrateAsync(cancellationToken);
                }

                if (await applicationDbContext.Database.CanConnectAsync(cancellationToken))
                {
                    await InitializeDefaultRolesAsync(cancellationToken);
                    await InitializeAdminUserAsync();
                    await InitialiseMastersAsync(cancellationToken);
                }
            }
        }

        private async Task InitializeDefaultRolesAsync(CancellationToken cancellationToken)
        {
            foreach (string roleName in RoleConstants.DefaultRoles)
            {
                if (roleName == RoleConstants.Admin || roleName == RoleConstants.Basic)
                {
                    if (await roleManager.Roles.SingleOrDefaultAsync(role => role.Name == roleName, cancellationToken)
                        is not ApplicationRole incomingRole)
                    {
                        incomingRole = new ApplicationRole() { Name = roleName, Description = $"{roleName} Role" };
                        await roleManager.CreateAsync(incomingRole);
                    }

                    // Assign permissions to newly added role
                    if (roleName == RoleConstants.Basic)
                    {
                        await AssignPermissionsToRole(CimPermissions.Basic, incomingRole, cancellationToken);
                    }
                    else if (roleName == RoleConstants.Admin)
                    {
                        await AssignPermissionsToRole(CimPermissions.Admin, incomingRole, cancellationToken);
                        if (tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Id == TenancyConstants.Root.Id)
                        {
                            await AssignPermissionsToRole(CimPermissions.Root, incomingRole, cancellationToken);
                        }
                    }
                }
            }
        }
                
        private async Task InitializeAdminUserAsync()
        {
            if (string.IsNullOrEmpty(tenantInfoContextAccessor.MultiTenantContext.TenantInfo.AdminEmail))
            {
                return;
            }

            if (await userManager.Users.FirstOrDefaultAsync(u => u.Email == tenantInfoContextAccessor.MultiTenantContext.TenantInfo.AdminEmail)
                is not ApplicationUser adminUser)
            {
                adminUser = new ApplicationUser()
                {
                    FirstName = TenancyConstants.FirstName,
                    LastName = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Name, //TenancyConstants.LastName,
                    Email = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.AdminEmail,
                    UserName = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.AdminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.AdminEmail.ToUpperInvariant(),
                    NormalizedUserName = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.AdminEmail.ToUpperInvariant(),
                    IsActive = true
                };

                var password = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = password.HashPassword(adminUser, TenancyConstants.DefaultPassword);
                await userManager.CreateAsync(adminUser);
            }

            if (!await userManager.IsInRoleAsync(adminUser, RoleConstants.Admin))
            {
                // Assign user to admin role
                await userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
            }
        }

        private async Task AssignPermissionsToRole(
            IReadOnlyList<CimPermission> rolePermissions,
            ApplicationRole currentRole, CancellationToken cancellationToken)
        {

            var currentClaims = await roleManager.GetClaimsAsync(currentRole);
            foreach (var rolePermission in rolePermissions)
            {
                if (!currentClaims.Any(c => c.Type == ClaimConstants.Permission && c.Value == rolePermission.Name))
                {
                    await applicationDbContext.RoleClaims.AddAsync(new IdentityRoleClaim<string>
                    {
                        RoleId = currentRole.Id,
                        ClaimType = ClaimConstants.Permission,
                        ClaimValue = rolePermission.Name
                    }, cancellationToken);

                    await applicationDbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }


        private async Task AssignCustomPermissionsToRole(string role, string fileName, CancellationToken cancellationToken)
        {
            var directoryPath = Directory.GetCurrentDirectory().ToString().Replace("WebApi", "Infrastructure/Masters");
            
            using (var r = new StreamReader($"{directoryPath}/DataFiles/{fileName}.json"))
            {
                var json = r.ReadToEnd();
                var lstPermissions = JsonConvert.DeserializeObject<List<ScreenPermissions>>(json);

                CreateRolePermissionRequest request = new();
                request.Name = role;
                request.Description = "System Generated Role";
                request.Permissions = lstPermissions;
                
                await roleService.CreateAsync(request);

            }
        }

        private async Task InitialiseMastersAsync(CancellationToken cancellationToken)
        {
            var user = userManager.Users.FirstOrDefaultAsync(u => u.Email == tenantInfoContextAccessor.MultiTenantContext.TenantInfo.AdminEmail);
            var directoryPath = Directory.GetCurrentDirectory().ToString().Replace("WebApi", "Infrastructure/Masters");
            var cntListTypes = await applicationDbContext.ListTypes.CountAsync();
            var dataExists =  cntListTypes> 0 ? true : false;
            if (!dataExists)
            {
                using (var r = new StreamReader($"{directoryPath}/DataFiles/ListType.json"))
                {
                    var json = r.ReadToEnd();
                    var lstListTypes = JsonConvert.DeserializeObject<List<ListType>>(json);

                    foreach (var listType in lstListTypes)
                    {
                        await applicationDbContext.ListTypes.AddAsync(new ListType
                        {
                            Code = listType.Code,
                            IsActive = listType.IsActive,
                            ListName = listType.ListName,
                            IsDeleted = listType.IsDeleted,
                            TenantId = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Id,
                            CreatedBy = Guid.Parse(user.Result.Id),
                            UpdatedBy = Guid.Parse(user.Result.Id),
                            Id = listType.Id
                        }, cancellationToken);

                        await applicationDbContext.SaveChangesAsync(cancellationToken);
                    }
                }

                using (var r = new StreamReader($"{directoryPath}/DataFiles/ListTypeItems.json"))
                {
                    var json = r.ReadToEnd();
                    var lstListTypeItems = JsonConvert.DeserializeObject<List<ListTypeItems>>(json);
                    foreach (var item in lstListTypeItems)
                    {
                        await applicationDbContext.ListTypeItems.AddAsync(new ListTypeItems
                        {
                            Code = item.Code,
                            ItemName = item.ItemName,
                            IsEscalationSupervisor = item.IsEscalationSupervisor,
                            ListTypeId = item.ListTypeId,
                            IsActive = item.IsActive,
                            IsDeleted = item.IsDeleted,
                            TenantId = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Id,
                            CreatedBy = Guid.Parse(user.Result.Id),
                            UpdatedBy = Guid.Parse(user.Result.Id),
                            Id = item.Id
                        }, cancellationToken);
                        await applicationDbContext.SaveChangesAsync(cancellationToken);
                    }
                }

                using (var r = new StreamReader($"{directoryPath}/DataFiles/MasterData.json"))
                {
                    var json = r.ReadToEnd();
                    var lstMasterData = JsonConvert.DeserializeObject<List<MasterData>>(json);
                    foreach (var item in lstMasterData)
                    {
                        await applicationDbContext.MasterData.AddAsync(new MasterData
                        {
                            Code = item.Code,
                            ItemName = item.ItemName,
                            IsEscalationSupervisor = item.IsEscalationSupervisor,
                            ListTypeId = item.ListTypeId,
                            IsActive = item.IsActive,
                            IsDeleted = item.IsDeleted,
                            TenantId = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Id,
                            CreatedBy = Guid.Parse(user.Result.Id),
                            UpdatedBy = Guid.Parse(user.Result.Id),
                            Id = item.Id
                        }, cancellationToken);
                        await applicationDbContext.SaveChangesAsync(cancellationToken);
                    }
                }
            
                using (var r = new StreamReader($"{directoryPath}/DataFiles/Currency.json"))
                {
                    var json = r.ReadToEnd();
                    var lstCurrency = JsonConvert.DeserializeObject<List<Currency>>(json);
                    foreach (var currency in lstCurrency)
                    {
                        await applicationDbContext.Currency.AddAsync(new Currency
                        {
                            Code = currency.Code,
                            Name = currency.Name,
                            MCId = currency.MCId,
                            Minor_Unit = currency.Minor_Unit,
                            N_Code = currency.N_Code,
                            Symbol = currency.Symbol,
                            IsActive = currency.IsActive,
                            IsDeleted = currency.IsDeleted,
                            TenantId = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Id,
                            CreatedBy = Guid.Parse(user.Result.Id),
                            UpdatedBy = Guid.Parse(user.Result.Id),
                            Id = currency.Id
                        }, cancellationToken);
                        await applicationDbContext.SaveChangesAsync(cancellationToken);
                    }
                }
            
                using (var r = new StreamReader($"{directoryPath}/DataFiles/Country.json"))
                {
                    var json = r.ReadToEnd();
                    var lstCountry = JsonConvert.DeserializeObject<List<Country>>(json);
                    foreach (var country in lstCountry)
                    {
                        await applicationDbContext.Country.AddAsync(new Country
                        {
                            Capital = country.Capital,
                            ContinentId = country.ContinentId,
                            CurrencyId = country.CurrencyId,
                            Formal = country.Formal,
                            Iso_2 = country.Iso_2,
                            Iso_3 = country.Iso_3,
                            Name = country.Name,
                            Region = country.Region,
                            Sub_Region = country.Sub_Region,
                            IsActive = country.IsActive,
                            IsDeleted = country.IsDeleted,
                            TenantId = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Id,
                            CreatedBy = Guid.Parse(user.Result.Id),
                            UpdatedBy = Guid.Parse(user.Result.Id),
                            Id = country.Id
                        }, cancellationToken);
                        await applicationDbContext.SaveChangesAsync(cancellationToken);
                    }
                }
                
                using (var r = new StreamReader($"{directoryPath}/DataFiles/Spareparts.json"))
                {
                    var json = r.ReadToEnd();
                    var lstSpareparts = JsonConvert.DeserializeObject<List<Sparepart>>(json);
                    foreach (var spare in lstSpareparts)
                    {
                        await applicationDbContext.Spareparts.AddAsync(new Sparepart
                        {
                            ConfigTypeId = spare.ConfigTypeId,
                            ConfigValueId = spare.ConfigValueId,
                            CountryId = spare.CountryId,
                            CurrencyId = spare.CurrencyId,
                            DescCatalogue = spare.DescCatalogue,
                            HsCode = spare.HsCode,
                            Image = spare.Image,
                            IsObselete = spare.IsObselete,
                            ItemDesc = spare.ItemDesc,
                            PartNo = spare.PartNo,
                            PartType = spare.PartType,
                            Qty = spare.Qty,
                            ReplacePartNoId = spare.ReplacePartNoId,
                            IsActive = spare.IsActive,
                            IsDeleted = spare.IsDeleted,
                            TenantId = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Id,
                            CreatedBy = Guid.Parse(user.Result.Id),
                            UpdatedBy = Guid.Parse(user.Result.Id),
                            Id = spare.Id
                        }, cancellationToken);

                        await applicationDbContext.SaveChangesAsync(cancellationToken);
                    }
                }

            }
            CreateDistributorDesignation("Distributor-Service Operations", user.Result);

            await InitializeCustomeRolesAsync(cancellationToken);
        }

        private void CreateDistributorDesignation(string designation, ApplicationUser user )
        {
            if (!applicationDbContext.ListTypeItems.Any(x => x.ItemName.ToUpper().Equals(designation.ToUpper())))
            {
                var listtypeitem = new ListTypeItems
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = Guid.Parse(user.Id),
                    UpdatedBy = Guid.Parse(user.Id),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsDeleted = false,
                    Code = "DIOPS",
                    ItemName = designation,
                    ListTypeId = applicationDbContext.ListTypes.FirstOrDefault(x => x.ListName.ToUpper() == "DESIGNATION").Id
                };

                applicationDbContext.ListTypeItems.Add(listtypeitem);
                applicationDbContext.SaveChanges();
            }
        }

        private async Task InitializeCustomeRolesAsync(CancellationToken cancellationToken)
        {
            foreach (string roleName in RoleConstants.DefaultRoles)
            {
                //if (roleName == RoleConstants.Customer && await roleManager.Roles.SingleOrDefaultAsync(role => role.Name == roleName, cancellationToken)
                //        is not ApplicationRole incomingRoleC)
                //{
                //    await AssignCustomPermissionsToRole(roleName, "CustomerRole", cancellationToken);
                //}
                //else 
                if (roleName == RoleConstants.Site && await roleManager.Roles.SingleOrDefaultAsync(role => role.Name == roleName, cancellationToken)
                        is not ApplicationRole incomingRoleS)
                {
                    await AssignCustomPermissionsToRole(roleName, "SiteRole", cancellationToken);
                }
                //else if (roleName == RoleConstants.Distributor_Operations && await roleManager.Roles.SingleOrDefaultAsync(role => role.Name == roleName, cancellationToken)
                //        is not ApplicationRole incomingRoleD)
                //{
                //    await AssignCustomPermissionsToRole(roleName, "DistributorRole", cancellationToken);
                //}
                else if (roleName == RoleConstants.Distributor_Operations_Region && await roleManager.Roles.SingleOrDefaultAsync(role => role.Name == roleName, cancellationToken)
                        is not ApplicationRole incomingRoleR)
                {
                    await AssignCustomPermissionsToRole(roleName, "RegionRole", cancellationToken);
                }
                else if (roleName == RoleConstants.Engineer && await roleManager.Roles.SingleOrDefaultAsync(role => role.Name == roleName, cancellationToken)
                        is not ApplicationRole incomingRoleE)
                {
                    await AssignCustomPermissionsToRole(roleName, "EngineerRole", cancellationToken);
                }
            }
        }

    }
}
