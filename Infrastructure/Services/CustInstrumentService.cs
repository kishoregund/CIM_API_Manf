using Application.Features.Customers;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;
using Application.Features.Customers.Responses;
using Mapster;
using Application.Features.Instruments.Responses;
using Microsoft.Extensions.Configuration;
using Application.Features.Customers.Requests;

namespace Infrastructure.Services
{
    public class CustomerInstrumentService(ApplicationDbContext Context, ICurrentUserService currentUserService, IConfiguration configuration) : ICustInstrumentService
    {
        public async Task<CustomerInstrument> GetCustomerInstrumentAsync(Guid id)
            => await Context.CustomerInstrument.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<CustomerInstrument>> GetCustomerInstrumentsAsync(Guid custSiteId)
            => await Context.CustomerInstrument.Where(p => p.CustSiteId == custSiteId).ToListAsync();

        public async Task<CustomerInstrument> GetCustomerInstrumentByInstrumentAsync(Guid instrumentId)
         => await Context.CustomerInstrument.FirstOrDefaultAsync(p => p.InstrumentId == instrumentId);

        // needed on Customer intrument and AMC instrument
        public async Task<List<CustomerInstrumentResponse>> GetAssignedCustomerInstrumentsAsync(string businessUnitId, string brandId)
        {
            var userProfile = await Context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));

            CommonMethods commonMethods = new CommonMethods(Context, currentUserService, configuration);
            List<Instrument> lstInstruments = new();
            List<Site> lstSites = new();
            if (userProfile != null && userProfile.ContactType.ToUpper() == "DR")
            {
                if (userProfile.SegmentCode == "RDTSP")   /// dist support
                {
                    var lstRegionsProfile = commonMethods.GetDistRegionsByUserIdAsync().Result.ToArray();
                    lstSites = Context.Site.Where(x => lstRegionsProfile.Contains(x.RegionId.ToString())).ToList();
                    lstInstruments = await (from i in Context.Instrument
                                            join ia in Context.InstrumentAllocation on i.Id equals ia.InstrumentId
                                            where businessUnitId.Contains(ia.BusinessUnitId.ToString()) && brandId.Contains(ia.BrandId.ToString())
                                            select i).ToListAsync();
                }
                else  // eng
                {
                    lstSites = Context.Site.Where(x => x.DistId == userProfile.EntityParentId).ToList();
                    lstInstruments = await (from i in Context.Instrument
                                            join ci in Context.CustomerInstrument on i.Id equals ci.InstrumentId
                                            join site in Context.Site on ci.CustSiteId equals site.Id
                                            where site.DistId == userProfile.EntityParentId
                                            select i).ToListAsync();
                }
            }
            else if (userProfile != null && userProfile.ContactType.ToUpper() == "CS")
            {
                var lstSitesProfile = commonMethods.GetSitesByUserIdAsync().Result.ToArray();
                lstInstruments = await Context.Instrument.ToListAsync();
                lstSites = Context.Site.Where(x => lstSitesProfile.Contains(x.Id.ToString())).ToList();
            }
            else  // admin
            {
                lstInstruments = await Context.Instrument.ToListAsync();
                lstSites = await Context.Site.ToListAsync();
            }
            var instruments = (from i in lstInstruments //Context.Instrument.Where(x => businessUnitId.Contains(x.BusinessUnitId.ToString()) && brandId.Contains(x.BrandId.ToString()))
                               join ci in Context.CustomerInstrument on i.Id equals ci.InstrumentId
                               join site in lstSites on ci.CustSiteId equals site.Id
                               //join site in Context.Site.Where(x => lstRegionsprofile.Contains(x.RegionId.ToString())) on ci.CustSiteId equals site.Id
                               select new CustomerInstrumentResponse()
                               {
                                   BaseCurrencyAmt = ci.BaseCurrencyAmt,
                                   InstrumentId = ci.InstrumentId,
                                   Cost = ci.Cost,
                                   CurrencyId = ci.CurrencyId,
                                   CreatedBy = ci.CreatedBy,
                                   CreatedOn = ci.CreatedOn,
                                   CustSiteId = site.Id,
                                   CustSiteName = site.CustRegName,
                                   DateOfPurchase = ci.DateOfPurchase,
                                   EngContact = ci.EngContact,
                                   EngEmail = ci.EngEmail,
                                   EngName = ci.EngName,
                                   EngNameOther = ci.EngNameOther,
                                   Id = ci.Id,
                                   InsMfgDt = ci.InsMfgDt,
                                   InstallBy = ci.InstallBy,
                                   InstallByName = Context.Distributor.FirstOrDefault(x => x.Id == site.DistId).DistName,
                                   InstallByOther = ci.InstallByOther,
                                   InstallDt = ci.InstallDt,
                                   InstruEngineerId = ci.InstruEngineerId,
                                   InstrumentSerNoType = Context.ListTypeItems.FirstOrDefault(x => x.Id.ToString() == i.InsType).ItemName + " - " + i.SerialNos,
                                   IsActive = ci.IsActive,
                                   IsDeleted = ci.IsDeleted,
                                   OperatorId = ci.OperatorId,
                                   ShipDt = ci.ShipDt,
                                   Warranty = ci.Warranty,
                                   WrntyEnDt = ci.WrntyEnDt,
                                   WrntyStDt = ci.WrntyStDt,
                                   InsType = i.InsType,
                                   InsTypeName = Context.ListTypeItems.FirstOrDefault(x => x.Id.ToString() == i.InsType).ItemName,
                                   InsVersion = i.InsVersion,
                                   ManufId = i.ManufId,
                                   SerialNos = i.SerialNos,
                                   MachineEng = Context.SiteContact.Where(x => x.Id == ci.InstruEngineerId).FirstOrDefault(),
                                   OperatorEng = Context.SiteContact.Where(x => x.Id == ci.OperatorId).FirstOrDefault()
                               }).ToList();

            return instruments;
        }

        public async Task<List<InstrumentResponse>> GetCustomersInstrumentBySiteAsync(Guid siteId)
        {
            //CommonMethods commonMethods = new CommonMethods(Context, currentUserService, configuration);
            List<InstrumentResponse> lstInstruments = new();
            List<Site> lstSites = new();
            var userProfile = await Context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));
            if (userProfile.SegmentCode == "RDTSP")   /// dist support
            {
                var lstRegionsProfile = userProfile.DistRegions; //commonMethods.GetDistRegionsByUserIdAsync().Result.ToArray();
                lstSites = Context.Site.Where(x => lstRegionsProfile.Contains(x.RegionId.ToString()) && x.Id == siteId).ToList();
                if (lstSites.Count > 0)
                {
                    lstInstruments = await (from i in Context.Instrument
                                            join ia in Context.InstrumentAllocation on i.Id equals ia.InstrumentId
                                            join ci in Context.CustomerInstrument on i.Id equals ci.InstrumentId
                                            join ma in Context.Manufacturer on i.ManufId equals ma.Id
                                            join it in Context.ListTypeItems on i.InsType equals it.Id.ToString()
                                            where userProfile.BusinessUnitIds.Contains(ia.BusinessUnitId.ToString()) && userProfile.BrandIds.Contains(ia.BrandId.ToString())
                                            && ci.CustSiteId == siteId
                                            select new InstrumentResponse
                                            {
                                                Id = i.Id,
                                                InsTypeName = it.ItemName,
                                                BrandId = ia.Id,
                                                //BrandName = br.BrandName,
                                                BusinessUnitId = ia.Id,
                                                //BusinessUnitName = b.BusinessUnitName,
                                                Image = i.Image,
                                                InsMfgDt = i.InsMfgDt,
                                                InsType = i.InsType,
                                                InsVersion = i.InsVersion,
                                                IsActive = i.IsActive,
                                                IsDeleted = i.IsDeleted,
                                                ManufId = i.ManufId,
                                                ManufName = ma.ManfName,
                                                SerialNos = i.SerialNos,
                                                CreatedBy = i.CreatedBy,
                                                CreatedOn = i.CreatedOn
                                            }).ToListAsync();
                }
            }
            else
            {
                lstInstruments = await (from i in Context.Instrument
                                        join ci in Context.CustomerInstrument on i.Id equals ci.InstrumentId
                                        join m in Context.Manufacturer on i.ManufId equals m.Id
                                        join it in Context.ListTypeItems on i.InsType equals it.Id.ToString()
                                        where ci.CustSiteId == siteId
                                        select new InstrumentResponse
                                        {
                                            Id = i.Id,
                                            InsTypeName = it.ItemName,
                                            //BrandId = ia.Id,
                                            //BrandName = br.BrandName,
                                            //BusinessUnitId = ia.Id,
                                            //BusinessUnitName = b.BusinessUnitName,
                                            Image = i.Image,
                                            InsMfgDt = i.InsMfgDt,
                                            InsType = i.InsType,
                                            InsVersion = i.InsVersion,
                                            IsActive = i.IsActive,
                                            IsDeleted = i.IsDeleted,
                                            ManufId = i.ManufId,
                                            ManufName = m.ManfName,
                                            SerialNos = i.SerialNos,
                                            CreatedBy = i.CreatedBy,
                                            CreatedOn = i.CreatedOn
                                        }).ToListAsync();
            }
            return lstInstruments;
        }

        public async Task<Guid> CreateCustomerInstrumentAsync(CustomerInstrument CustomerInstrument)
        {

            CustomerInstrument.CreatedOn = DateTime.Now;
            CustomerInstrument.UpdatedOn = DateTime.Now;
            CustomerInstrument.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            CustomerInstrument.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await Context.CustomerInstrument.AddAsync(CustomerInstrument);
            await Context.SaveChangesAsync();
            return CustomerInstrument.Id;
        }

        public async Task<bool> DeleteCustomerInstrumentAsync(Guid id)
        {

            var deletedInstrument = await Context.CustomerInstrument.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedInstrument == null) return true;

            deletedInstrument.IsDeleted = true;
            deletedInstrument.IsActive = false;

            Context.Entry(deletedInstrument).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateCustomerInstrumentAsync(CustomerInstrument CustomerInstrument)
        {
            CustomerInstrument.UpdatedOn = DateTime.Now;
            CustomerInstrument.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            Context.Entry(CustomerInstrument).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return CustomerInstrument.Id;
        }

        public async Task<bool> IsDuplicateAsync(CustomerInstrumentRequest customerInstrument)
        {
            return await Context.CustomerInstrument.AnyAsync(x => x.CustSiteId == customerInstrument.CustSiteId && x.InstrumentId == customerInstrument.InstrumentId);
        }
    }
}