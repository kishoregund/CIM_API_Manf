using Application.Exceptions;
using Application.Features.AppBasic.Responses;
using Application.Features.Identity.Users;
using Application.Features.Identity.Users.Models;
using Domain.Entities;
using Domain.Views;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Common;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;

namespace Infrastructure.Identity
{
    public class UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        ApplicationDbContext context, ICurrentUserService currentUserService,
        IMultiTenantContextAccessor<CIMTenantInfo> tenantInfoContextAccessor) : IUserService
    {
        public async Task<string> ActivateOrDeactivateAsync(string contactType, bool activation, Guid contactId)
        {
            var userContact = await context.UserContactMappings.FirstOrDefaultAsync(x => x.ContactType == contactType && x.ContactId == contactId);
            var userInDb = await GetUserAsync(userContact.UserId.ToString());

            userInDb.IsActive = activation;

            await userManager.UpdateAsync(userInDb);

            return userContact.UserId.ToString();
        }

        public async Task<bool> DeleteContactUserAsync(string contactType, bool activation, Guid contactId)
        {
            var userContact = await context.UserContactMappings.FirstOrDefaultAsync(x => x.ContactType == contactType && x.ContactId == contactId);

            var userProfile = await context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userContact.UserId);

            var userRole = await context.UserRoles.FirstOrDefaultAsync(x => x.UserId == userContact.UserId.ToString());

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userContact.UserId.ToString());

            context.Entry(userRole).State = EntityState.Deleted;
            context.Entry(userProfile).State = EntityState.Deleted;
            context.Entry(userContact).State = EntityState.Deleted;
            context.Entry(user).State = EntityState.Deleted;

            await context.SaveChangesAsync();

            var success = DeleteTenantUserAsync(user.Email, userProfile.TenantId);

            return true;
        }

        public async Task<string> AssignRolesAsync(string userId, UserRolesRequest request)
        {
            var userInDb = await GetUserAsync(userId);

            if (await userManager.IsInRoleAsync(userInDb, RoleConstants.Admin)
                && request.UserRoles.Any(ur => !ur.IsAssigned && ur.Name == RoleConstants.Admin))
            {
                var adminUsersCount = (await userManager.GetUsersInRoleAsync(RoleConstants.Admin)).Count;
                if (userInDb.Email == TenancyConstants.Root.Email)
                {
                    if (tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Id == TenancyConstants.Root.Id)
                    {
                        throw new ConflictException("Not allowed to remove Admin Role for a Root Tenant User.");
                    }
                }
                else if (adminUsersCount <= 2)
                {
                    throw new ConflictException("Tenant should have at least three admin users.");
                }
            }

            foreach (var userRole in request.UserRoles)
            {
                if (await roleManager.FindByIdAsync(userRole.RoleId) is not null)
                {
                    if (userRole.IsAssigned)
                    {
                        if (!await userManager.IsInRoleAsync(userInDb, userRole.Name))
                        {
                            await userManager.AddToRoleAsync(userInDb, userRole.Name);
                        }
                    }
                    else
                    {
                        await userManager.RemoveFromRoleAsync(userInDb, userRole.Name);
                    }
                }
            }

            return userInDb.Id;
        }

        public async Task<string> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var userInDb = await GetUserAsync(request.UserId);

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                throw new ConflictException("Passwords do not match.");
            }

            var result = await userManager.ChangePasswordAsync(userInDb, request.CurrentPassword, request.CurrentPassword);

            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to change password.", GetIdentityResultErrorDescriptions(result));
            }
            return userInDb.Id;
        }

        public async Task<string> CreateUserAsync(CreateUserRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                throw new ConflictException("Passwords do not match.");
            }

            if (await IsEmailTakenAsync(request.Email))
            {
                throw new ConflictException("Email already taken.");
            }
            if (request.ContactType == "DR" && !context.BusinessUnit.Any())
            {
                throw new ConflictException("Business Unit and Brand does not exsits.");
            }
            if (request.ContactType == "MSR" && !context.ManfBusinessUnit.Any())
            {
                throw new ConflictException("Manufacturer BU's does not exsits.");
            }
            var newUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
                IsActive = true /// as user is created it should be activated.
            };

            var result = await userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to create user.", GetIdentityResultErrorDescriptions(result));
            }
            else
            {
                var userContactMapping = new UserContactMapping
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse(newUser.Id),
                    ContactId = request.ContactId,
                    ContactType = request.ContactType,
                    TenantId = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Id,
                    CreatedBy = Guid.Parse(currentUserService.GetUserId()),
                    UpdatedBy = Guid.Parse(currentUserService.GetUserId()),
                };

                await context.UserContactMappings.AddAsync(userContactMapping);
                await context.SaveChangesAsync();

                var tenantUser = new UserTenantDto
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    TenantId = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Id  // tenantid should be loggedin tenant
                };

                await CreateTenantUserAsync(tenantUser);

                CommonMethods commonMethods = new(context, currentUserService, configuration);
                var roleId = commonMethods.CreateUserProfile(request, Guid.Parse(newUser.Id));

                UserRoleDto userRole = new();
                userRole.RoleId = roleId.ToString();
                userRole.Description = "System assigned Role";
                userRole.Name = context.Roles.FirstOrDefault(x => x.Id == roleId.ToString()).Name;
                userRole.IsAssigned = true;

                UserRolesRequest roleRequest = new();
                roleRequest.UserRoles = new();
                roleRequest.UserRoles.Add(userRole);
                var id = await AssignRolesAsync(newUser.Id, roleRequest);

            }
            return newUser.Id;
        }

        public async Task<string> DeleteUserAsync(string userId)
        {
            var userInDb = await GetUserAsync(userId);

            context.Users.Remove(userInDb);
            await context.SaveChangesAsync();

            return userInDb.Id;
        }

        public async Task<UserDto> GetUserByIdAsync(string userId, CancellationToken ct)
        {
            var userInDb = await GetUserAsync(userId, ct);

            return userInDb.Adapt<UserDto>();
        }

        public async Task<UserDto> GetUserByContactIdAsync(Guid contactId, string contactType, CancellationToken ct)
        {
            var contactUser = await context.UserContactMappings.FirstOrDefaultAsync(x => x.ContactId == contactId && x.ContactType == contactType);
            if (contactUser != null)
            {
                var userInDb = await GetUserAsync(contactUser.UserId.ToString(), ct);
                return userInDb.Adapt<UserDto>();
            }
            return null;
        }

        public async Task<List<UserRoleDto>> GetUserRolesAsync(string userId, CancellationToken ct)
        {
            var userRoles = new List<UserRoleDto>();

            var userInDb = await GetUserAsync(userId, ct);
            var roles = await roleManager
                .Roles
                .AsNoTracking()
                .ToListAsync(ct);

            foreach (var role in roles)
            {
                userRoles.Add(new UserRoleDto
                {
                    RoleId = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    IsAssigned = await userManager.IsInRoleAsync(userInDb, role.Name)
                });
            }

            return userRoles;
        }

        public async Task<List<UserDto>> GetUsersAsync(CancellationToken ct)
        {
            var usersInDb = await userManager
                .Users
                .Where(x => x.FirstName != "Admin")
                .AsNoTracking()
                .ToListAsync(ct);

            var listUsers = usersInDb.Adapt<List<UserDto>>();

            foreach (UserDto user in listUsers)
            {
                var cm = context.UserContactMappings.FirstOrDefault(x => x.UserId == Guid.Parse(user.Id));
                user.ContactId = cm.ContactId;
                user.ContactType = cm.ContactType;
            }

            return listUsers;
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;

            //if (await _userManager.FindByEmailAsync(email) is not null)
            //{
            //    return true;
            //}
            //return false;
        }

        public async Task<string> UpdateUserAsync(UpdateUserRequest request)
        {
            var userInDb = await GetUserAsync(request.Id);

            userInDb.FirstName = request.FirstName;
            userInDb.LastName = request.LastName;
            userInDb.PhoneNumber = request.PhoneNumber;

            var result = await userManager.UpdateAsync(userInDb);

            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to update user.", GetIdentityResultErrorDescriptions(result));
            }

            await signInManager.RefreshSignInAsync(userInDb);

            return userInDb.Id;
        }

        public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken ct)
        {
            var userInDb = await GetUserAsync(userId, ct);
            var userRoles = await userManager.GetRolesAsync(userInDb);

            var permissions = new List<string>();

            foreach (var role in await roleManager
                .Roles
                .Where(r => userRoles.Contains(r.Name))
                .ToListAsync(ct))
            {
                permissions.AddRange(await context
                    .RoleClaims
                    .Where(rc => rc.RoleId == role.Id && rc.ClaimType == ClaimConstants.Permission)
                    .Select(rc => rc.ClaimValue)
                    .ToListAsync(ct));
            }

            // Verify if is needed - .Distinct().ToList();
            return permissions.Distinct().ToList();
        }

        public async Task<UserDetailsDto> GetUserDetailsAsync(string email, CancellationToken ct)
        {

            var userDto = await GetUserByEmailAsync(email, ct);
            var userInDb = await GetUserAsync(userDto.Id, ct);
            var userRoles = await userManager.GetRolesAsync(userInDb);

            var permissions = new List<string>();

            foreach (var role in await roleManager
                .Roles
                .Where(r => userRoles.Contains(r.Name))
                .ToListAsync(ct))
            {
                permissions.AddRange(await context
                    .RoleClaims
                    .Where(rc => rc.RoleId == role.Id && rc.ClaimType == ClaimConstants.Permission)
                    .Select(rc => rc.ClaimValue)
                    .ToListAsync(ct));
            }

            //return permissions.Distinct().ToList();
            return new UserDetailsDto
            {
                User = userDto,
                UserRole = userRoles[0],
                Permissions = permissions.Distinct().ToList(),
            };
        }

        public async Task<VW_UserProfile> GetLoggedInUserDetailsAsync(string email, CancellationToken ct)
        {
            VW_UserProfile profile = new();
            try
            {
                var userDto = await GetUserByEmailAsync(email, ct);
                var userInDb = await GetUserAsync(userDto.Id, ct);
                var userRoles = await userManager.GetRolesAsync(userInDb);
                if (userRoles.Count > 0 && userRoles[0] == RoleConstants.Admin)
                {

                    profile.UserId = Guid.Parse(userInDb.Id);
                    profile.UserName = userInDb.FirstName + " " + userInDb.LastName;
                    profile.Email = userInDb.Email;
                    profile.FirstName = userInDb.FirstName;
                    profile.LastName = userInDb.LastName;
                    profile.UserRole = userRoles[0].ToString();
                    profile.IsManfSubscribed = GetSubsribedBy(email);

                    return profile;
                }

                profile = context.VW_UserProfile.FirstOrDefault(x => x.Email == email);
            }
            catch (Exception ex)
            {

            }
            return profile;
        }

        public async Task<bool> IsPermissionAssigedAsync(string userId, string permission, CancellationToken ct)
        => (await GetPermissionsAsync(userId, ct)).Contains(permission);

        private async Task<ApplicationUser> GetUserAsync(string userId, CancellationToken ct = default)
        => await userManager
                .Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(ct) ?? throw new NotFoundException("User does not exists.");

        private async Task<UserDto> GetUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager
                     .Users
                     .Where(u => u.Email == email)
                     .FirstOrDefaultAsync(ct);

            return new UserDto
            {
                Id = user.Id,
                Email = email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                IsActive = user.IsActive,
            };
        }

        private List<string> GetIdentityResultErrorDescriptions(IdentityResult identityResult)
        {
            var errorDescriptions = new List<string>();
            foreach (var error in identityResult.Errors)
            {
                errorDescriptions.Add(error.Description);
            }

            return errorDescriptions;
        }

        public async Task<string> CreateTenantUserAsync(UserTenantDto cimTenantUserInfo)
        {
            SqlConnection con = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            con.Open();
            string qry = "insert into Multitenancy.TenantUsers (Id, Email, TenantId) values ('" + cimTenantUserInfo.Id + "','" + cimTenantUserInfo.Email + "', '" + cimTenantUserInfo.TenantId + "')";
            SqlCommand cmd = new SqlCommand(qry, con);
            await cmd.ExecuteNonQueryAsync();
            con.Close();
            return "Success";
        }

        public async Task<string> DeleteTenantUserAsync(string email, string tenantId)
        {
            SqlConnection con = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            con.Open();
            string qry = "delete from Multitenancy.TenantUsers where email= '" + email + "' and tenantid = '" + tenantId + "'";
            SqlCommand cmd = new SqlCommand(qry, con);
            await cmd.ExecuteNonQueryAsync();
            con.Close();
            return "Success";
        }

        private bool GetSubsribedBy(string email)
        {
            SqlConnection con = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select SubscribedBy from CIMSaaS.Multitenancy.Tenants where AdminEmail ='" + email + "'", con);
            da.Fill(dt);
            var isManfSubsribed = dt.Rows.Count > 0 ? bool.Parse(dt.Rows[0][0].ToString()) : false;
            con.Close();
            return isManfSubsribed;
        }


        public async Task<List<string>> GetUserRegionsAsync()
        {
            CommonMethods commonMethods = new CommonMethods(context, currentUserService, configuration);
            return await commonMethods.GetDistRegionsByUserIdAsync();
        }



        //public async Task<UserTenantDto> GetUserTenantAsync(string emailId)
        //{
        //    SqlConnection con = new(configuration.GetConnectionString("DefaultConnection"));
        //    await con.OpenAsync();
        //    DataTable dt = new();
        //    SqlDataAdapter da = new("select * from Multitenancy.TenantUsers where email = '" + emailId + "'", con);
        //    da.Fill(dt);
        //    await con.CloseAsync();

        //    UserTenantDto usrInfo = new();
        //    if (dt.Rows.Count > 0)
        //    {
        //        usrInfo.Email = emailId;
        //        usrInfo.TenantId = dt.Rows[0]["TenantId"].ToString();
        //    }
        //    return usrInfo;
        //}
    }
}
