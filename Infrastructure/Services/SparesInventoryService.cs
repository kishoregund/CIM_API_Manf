
using Application.Features.Customers;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System;
using Infrastructure.Common;
using Application.Features.Customers.Responses;
using Mapster;
using Application.Features.Spares.Responses;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Domain.Views;
using Microsoft.AspNetCore.DataProtection;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class SparesInventoryService(ApplicationDbContext Context, ICurrentUserService currentUserService, IConfiguration configuration) : ICustSPInventoryService
    {

        public async Task<CustSPInventory> GetCustSPInventoryEntityAsync(Guid id)
            => await Context.CustSPInventory.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<CustSPInventoryResponse> GetCustSPInventoryAsync(Guid id)
        {
            var custSPInventory = await Context.CustSPInventory.Where(x => x.Id == id).OrderBy(x => x.QtyAvailable).FirstOrDefaultAsync();
            return GetCustSPInventory(custSPInventory);
        }

        public async Task<List<CustSPInventoryResponse>> GetCustSPInventorysAsync(Guid contactId, Guid customerId)
        {
            var lstCustSPInventory = new List<CustSPInventoryResponse>();
            var userProfile = await Context.VW_UserProfile.FirstOrDefaultAsync(x => x.ContactId == contactId);

            if (userProfile == null || userProfile.FirstName == "Admin")
            {
                var custSPIn = Context.CustSPInventory.OrderBy(x => x.QtyAvailable).ToList();

                foreach (var cSPI in custSPIn)
                {
                    var spIn = GetCustSPInventory(cSPI);
                    if (spIn != null) lstCustSPInventory.Add(spIn);
                }

                return lstCustSPInventory;
            }

            var commonMethods = new CommonMethods(Context, currentUserService, configuration);
            List<CustSPInventory> custSPInventory = new();
            if (userProfile.ContactType == "DR")
            {
                var bUs = userProfile.BusinessUnitIds.Split(','); // commonMethods.GetBusinessUnitList(userId, bUId);
                var brands = userProfile.BrandIds.Split(','); // commonMethods.GetBrandList(userId, brandId);

                custSPInventory = await (from a in Context.CustSPInventory.Where(x => !x.IsDeleted)
                                         join b in Context.Instrument.Where(x => bUs.Contains(x.BusinessUnitId.ToString()) && brands.Contains(x.BrandId.ToString()))
                                             on a.InstrumentId equals b.Id
                                         select a).ToListAsync();
            }
            else
            {
                custSPInventory = await (from a in Context.CustSPInventory.Where(x => !x.IsDeleted)
                                         join b in Context.Instrument on a.InstrumentId equals b.Id
                                         select a).ToListAsync();
            }
            //var privilage = Context.Vw_Privilages.FirstOrDefault(x =>
            //    x.UserId == userId && x.ScreenCode == "CTSPI" && x.UserName != "admin");

            //if (privilage != null && privilage.PrivilageCode != "PARTS" &&
            //    (privilage._create || privilage._read || privilage._update || privilage._delete))
            //{
            //    custSPInventory = custSPInventory.Where(x => x.Createdby == userId);
            //}

            //var custPrivilage = _context.Vw_Privilages.FirstOrDefault(x =>
            //    x.UserId == userId && x.ScreenCode == "CTSPI" && x.UserName != "admin");
            var customer = Context.Customer.Where(x => !x.IsDeleted);

            //if (custPrivilage != null && custPrivilage.PrivilageCode != "PARTS" && (custPrivilage._create ||
            //        custPrivilage._read || custPrivilage._update || custPrivilage._delete))
            //{
            //    customer = customer.Where(x => x.Createdby == userId);
            //}

            //var contactMapped = _context.Contactmapping.FirstOrDefault(x => x.Contactid == contactId);
            //var userType = _context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == contactMapped.Mappedfor)
            //    ?.Itemname;

            var lstRegionsprofile = commonMethods.GetDistRegionsByUserIdAsync().Result; //regionsprofile.Split(',');

            if (userProfile.ContactType == "CS")
            {
                
                var custSPIn = (from spi in custSPInventory.Where(x => userProfile.CustSites.Contains(x.SiteId.ToString()))
                                join site in Context.Site on spi.SiteId equals site.Id
                                select new { spi, site })
                    .Where(x => x.spi.IsDeleted == false
                                && lstRegionsprofile.Contains(x.site.DistId.ToString())
                                && x.site.CustomerId == userProfile.EntityParentId)
                    .OrderByDescending(x => x.spi.QtyAvailable)
                    .ToList();

                foreach (var cSPI in custSPIn)
                {
                    var spIn = GetCustSPInventory(cSPI.spi);
                    if (spIn != null) lstCustSPInventory.Add(spIn);
                }
            }
            else if (userProfile.ContactType == "DR" && customerId != Guid.Empty)
            {
                var custSPIn = (from spi in custSPInventory
                                join site in Context.Site on spi.SiteId equals site.Id
                                join cust in customer on spi.CustomerId equals cust.Id
                                select new { spi, cust, site })
                    .Where(x => x.spi.CustomerId == customerId && lstRegionsprofile.Contains(x.site.DistId.ToString()))
                    .OrderBy(x => x.spi.QtyAvailable)
                    .ToList();

                foreach (var cSPI in custSPIn)
                {
                    var spIn = GetCustSPInventory(cSPI.spi);
                    if (spIn != null) lstCustSPInventory.Add(spIn);
                }
            }
            else if (userProfile.ContactType == "DR" && customerId == Guid.Empty)
            {
                var custSPIn = (from spi in custSPInventory
                                join site in Context.Site on spi.SiteId equals site.Id
                                join cust in customer on site.CustomerId equals cust.Id
                                select new { spi, cust, site })
                    .Where(x => lstRegionsprofile.Contains(x.site.DistId.ToString()))
                    .OrderBy(x => x.spi.QtyAvailable)
                    .ToList();

                foreach (var cSPI in custSPIn)
                {
                    var spIn = GetCustSPInventory(cSPI.spi);
                    if (spIn != null) lstCustSPInventory.Add(spIn);
                }
            }
            //else if (userType.ToLower() == "site")
            //{
            //    var custSPIn = (from spi in custSPInventory
            //                    join site in _context.Site on spi.SiteId equals site.Id
            //                    select new { spi, site })
            //        .Where(x => x.site.Id == contactMapped.Parentid && x.spi.SiteId != "")
            //        .OrderBy(x => x.spi.QtyAvailable)
            //        .ToList();

            //    foreach (var cSPI in custSPIn)
            //    {
            //        var spIn = GetCustSPInventory(cSPI.spi, userId, bUId, brandId);
            //        if (spIn != null) lstCustSPInventory.Add(spIn);
            //    }

            //}

            return lstCustSPInventory;
        }

        private CustSPInventoryResponse GetCustSPInventory(CustSPInventory custSPInventory)
        {
            var sparepart = Context.Spareparts.FirstOrDefault(x => x.Id == custSPInventory.SparePartId).Adapt<SparepartResponse>();//  sp.GetSparePart(custSPInventory.SparePartId, userId);
            var ins = Context.Instrument.FirstOrDefault(x => x.Id == custSPInventory.InstrumentId);
            if (sparepart == null) return null;
            var mCustSPInventory = new CustSPInventoryResponse()
            {
                Id = custSPInventory.Id,
                IsActive = custSPInventory.IsActive,
                QtyAvailable = custSPInventory.QtyAvailable,
                CustomerId = custSPInventory.CustomerId,
                SparePartId = custSPInventory.SparePartId,
                SiteId = custSPInventory.SiteId,
                CustomerName = Context.Customer.FirstOrDefault(x => x.Id == custSPInventory.CustomerId)?.CustName,
                SparePart = sparepart,
                PartNo = sparepart.PartNo,
                HscCode = sparepart.HsCode,
                PartNoDesc = $"{sparepart.PartNo} => {sparepart.ItemDesc}",
                InstrumentId = custSPInventory.InstrumentId,
                InstrumentName = Context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == ins.InsType)?.ItemName + " - " + ins?.SerialNos,
            };
            return mCustSPInventory;
        }

        public async Task<Guid> CreateCustSPInventoryAsync(CustSPInventory CustSPInventory)
        {
            CustSPInventory.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            CustSPInventory.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            CustSPInventory.CreatedOn = DateTime.Now;
            CustSPInventory.UpdatedOn = DateTime.Now;
            await Context.CustSPInventory.AddAsync(CustSPInventory);
            await Context.SaveChangesAsync();

            return CustSPInventory.Id;
        }

        public async Task<bool> DeleteCustSPInventoryAsync(Guid id)
        {

            var deletedCustSPInventory = await Context
                .CustSPInventory.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deletedCustSPInventory != null)
            {
                deletedCustSPInventory.IsDeleted = true;
                deletedCustSPInventory.IsActive = false;

                Context.Entry(deletedCustSPInventory).State = EntityState.Deleted;
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Guid> UpdateCustSPInventoryAsync(CustSPInventory CustSPInventory)
        {
            
            CustSPInventory.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            CustSPInventory.UpdatedOn = DateTime.Now;

            Context.Entry(CustSPInventory).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return CustSPInventory.Id;
        }

        public async Task<List<VW_SparepartConsumedHistory>> GetSparepartConsumedHistoryAsync(Guid id)
            => await Context.VW_SparepartConsumedHistory.Where(x => x.CustomerSPInventoryId == id).OrderByDescending(x => x.ServiceReportDate).ToListAsync();

        public async Task<List<CustSPInventoryResponse>> GetCustSPInventoryForServiceReportAsync(Guid serviceReportId)
        {
            ServiceRequest serviceRequest = await (from sr in Context.ServiceRequest
                                                   join srp in Context.ServiceReport on sr.Id equals srp.ServiceRequestId
                                                   where srp.Id == serviceReportId
                                                   select sr).FirstOrDefaultAsync();

            var inventory = await Context.CustSPInventory.Where(x =>
              (x.SiteId == serviceRequest.SiteId || x.CustomerId == serviceRequest.CustId) && x.InstrumentId.ToString() == serviceRequest.MachinesNo).ToListAsync();

            List<CustSPInventoryResponse> lstCustSPInventoryResponses = new();
            foreach (CustSPInventory sp in inventory)
            {
                lstCustSPInventoryResponses.Add(GetCustSPInventory(sp));
            }

            return lstCustSPInventoryResponses;
        }
    }
}

