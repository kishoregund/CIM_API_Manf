
using Application.Features.Customers;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using Domain.Entities;
using Application.Features.Customers.Responses;
using Infrastructure.Identity;
using Application.Features.Identity.Roles;
using Infrastructure.Common;
using Infrastructure.Identity.Constants;
using Application.Features.Distributors.Responses;
using Mapster;
using Domain.Views;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class CustomerService(ApplicationDbContext Context, ICurrentUserService currentUserService, IRoleService roleService
        , IConfiguration configuration) : ICustomerService
    {
        CommonMethods commonMethods = new CommonMethods(Context, currentUserService, configuration);
        public async Task<Domain.Entities.Customer> GetCustomerAsync(Guid id)
            => await Context.Customer.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<CustomerResponse>> GetCustomersAsync()
        {
            var customers = await (from c in Context.Customer
                                   join d in Context.Distributor on c.DefDistId equals d.Id
                                   select new CustomerResponse()
                                   {
                                       AddrCountryId = d.AddrCountryId,
                                       Area = c.Area,
                                       City = c.City,
                                       Code = c.Code,
                                       DistributorName = d.DistName,
                                       GeoLat = c.GeoLat.ToString(),
                                       GeoLong = c.GeoLong.ToString(),
                                       Id = c.Id,
                                       Place = c.Place,
                                       Street = c.Street,
                                       Zip = c.Zip,
                                       DefDistId = c.DefDistId,
                                       CountryId = c.CountryId,
                                       CustName = c.CustName,
                                       DefDistRegionId = c.DefDistRegionId,
                                       IndustrySegment = c.IndustrySegment,
                                       IsActive = c.IsActive,
                                       IsDeleted = c.IsDeleted,
                                   }).ToListAsync();

            return customers;
        }

        public static bool IsGuid(string value)
        {
            Guid x;
            return Guid.TryParse(value, out x);
        }

        public async Task<List<CustomerResponse>> GetCustomersByUserIdAsync(Guid userId)
        {
            string privilageCode = string.Empty;
            List<Customer> customers = new();
            var userProfile = Context.VW_UserProfile.FirstOrDefault(x => x.UserId == userId);
            if (userProfile != null)
            {
                //check out permission if all data or user data
                List<string> permissions = await roleService.GetScreenPermissionByRoleIdAsync(userProfile.RoleId.ToString(), CimFeature.Customer);
                foreach (string roleClaim in permissions)
                {
                    string[] strPerm = roleClaim.Split('.');
                    if (IsGuid(strPerm[2]))
                    {
                        privilageCode = Context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == Guid.Parse(strPerm[2])).ItemCode;
                    }
                }


                if (userProfile.ContactType == "DR")
                {
                    //if (privilageCode.ToUpper() != "PARTS") // is not All Data
                    //    customers = await Context.Customer.Where(x => x.CreatedBy == userId).ToListAsync();
                    //else
                    var regions = userProfile.DistRegions.Split(',');
                    customers = await (from c in Context.Customer.Where(x => x.DefDistId == userProfile.EntityParentId)
                                       join s in Context.Site on c.Id equals s.CustomerId
                                       where regions.Contains(s.RegionId.ToString())
                                       select c).Distinct().ToListAsync();
                }
                else
                    customers = await Context.Customer.Where(x => x.Id == userProfile.EntityParentId).ToListAsync();

            }
            else if (Context.Users.FirstOrDefault(x => x.Id == userId.ToString()).FirstName == "Admin")
            {
                customers = await Context.Customer.ToListAsync();
            }

            var custResponse = customers.Adapt<List<CustomerResponse>>();
            foreach (var cust in custResponse)
            {
                cust.DistributorName = Context.Distributor.FirstOrDefaultAsync(x => x.Id == cust.DefDistId).Result.DistName;
            }

            return custResponse;
        }

        public async Task<Guid> CreateCustomerAsync(Domain.Entities.Customer Customer)
        {
            var country = Context.Country.Find(Customer.AddrCountryId);
            Customer.Code = $"C{(Customer.CustName.Length >= 3 ? Customer.CustName.Substring(0, 3).ToUpper() : Customer.CustName.Substring(0, 1).ToUpper())}{country?.Iso_2}{DateTime.Now.ToString("yy") + DateTime.Now.DayOfYear + DateTime.Now.ToString("HHmm")}";
            Customer.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            Customer.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            Customer.CreatedOn = DateTime.Now;
            Customer.UpdatedOn = DateTime.Now;
            Customer.IsActive = true;
            Customer.IsDeleted = false;

            await Context.Customer.AddAsync(Customer);
            await Context.SaveChangesAsync();

            return Customer.Id;
        }

        public async Task<bool> DeleteCustomerAsync(Guid id)
        {

            var deletedCustomer = await Context
                .Customer.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deletedCustomer != null)
            {
                deletedCustomer.IsDeleted = true;
                deletedCustomer.IsActive = false;

                Context.Entry(deletedCustomer).State = EntityState.Deleted;
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Guid> UpdateCustomerAsync(Domain.Entities.Customer Customer)
        {
            Customer.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            Customer.UpdatedOn = DateTime.Now;
            Context.Entry(Customer).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return Customer.Id;
        }

        public async Task<bool> IsDuplicateAsync(string custName)
          => await Context.Customer.AnyAsync(x => x.CustName.ToUpper() == custName.ToUpper());
    }
}

