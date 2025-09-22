using Application.Features.Identity.Users;
using Application.Features.Spares;
using Application.Features.Spares.Requests;
using Application.Features.Spares.Responses;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Mapster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Services
{
    public class SparepartsOfferRequestService(ApplicationDbContext context, ICurrentUserService currentUserService) : ISparepartsOfferRequestService
    {
        public async Task<bool> CreateSparepartsOfferRequestAsync(List<SparepartOfferRequestRequest> SparepartsOfferRequest)
        {
            try
            {
                foreach (SparepartOfferRequestRequest spare in SparepartsOfferRequest)
                {
                    spare.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
                    spare.UpdatedOn = DateTime.Now;
                    spare.TenantId = string.IsNullOrEmpty(spare.TenantId) ? currentUserService.GetUserTenant() : spare.TenantId;
                    if (!context.SparepartsOfferRequest.Any(x => x.PartNo == spare.PartNo && x.OfferRequestId == spare.OfferRequestId))
                    {
                        spare.CreatedBy = Guid.Parse(currentUserService.GetUserId());
                        spare.CreatedOn = DateTime.Now;
                        spare.IsActive = true;
                        spare.IsDeleted = false;

                        await context.SparepartsOfferRequest.AddAsync(spare.Adapt<SparepartsOfferRequest>());
                    }
                    else
                    {
                        context.Entry(spare.Adapt<SparepartsOfferRequest>()).State = EntityState.Modified;
                    }
                }
                await context.SaveChangesAsync();
                //return SparepartsOfferRequest.Id;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            return true;
        }

        public async Task<bool> DeleteSparepartsOfferRequestAsync(Guid id)
        {
            var deletedSparepart = await context.SparepartsOfferRequest.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedSparepart == null) return true;

            deletedSparepart.IsDeleted = true;
            deletedSparepart.IsActive = false;

            context.Entry(deletedSparepart).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<SparepartsOfferRequestResponse> GetSparepartsOfferRequestAsync(Guid id)
        {
            var spareOffReq = await context.SparepartsOfferRequest.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
            return GetSparePartsOfferRequest(spareOffReq);
        }

        public async Task<SparepartsOfferRequest> GetSparepartsOfferRequestEntityAsync(Guid id)
         => await context.SparepartsOfferRequest.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        public async Task<List<SparepartsOfferRequestResponse>> GetSparepartsOfferRequestsAsync(Guid offerRequestId)
        {
           var spOffRequests = await context.SparepartsOfferRequest.Where(x=>x.OfferRequestId == offerRequestId).ToListAsync();
            List<SparepartsOfferRequestResponse> lstSpareOffRequests = new();
            foreach (var sp in spOffRequests)
            {
                lstSpareOffRequests.Add(GetSparePartsOfferRequest(sp));
            }
            return lstSpareOffRequests;
        }

        public SparepartsOfferRequestResponse GetSparePartsOfferRequest(SparepartsOfferRequest sparepartsofferrequest)
        {
            var sparePart = context.Spareparts.FirstOrDefault(x => x.Id == sparepartsofferrequest.SparePartId);

            var mSparePartsOfferRequest = new SparepartsOfferRequestResponse()
            {
                Id = sparepartsofferrequest.Id,
                IsActive = sparepartsofferrequest.IsActive,
                PartNo = sparePart?.PartNo,
                HsCode = sparepartsofferrequest.HsCode,
                Qty = sparepartsofferrequest.Qty,
                Price = sparepartsofferrequest.Price,
                Amount = sparepartsofferrequest.Amount,
                DiscountPercentage = sparepartsofferrequest.DiscountPercentage,
                AfterDiscount = sparepartsofferrequest.AfterDiscount,
                SparePartId = sparepartsofferrequest.SparePartId,
                ItemDescription = sparePart?.ItemDesc,
                OfferRequestId = sparepartsofferrequest.OfferRequestId,
                CurrencyId = sparepartsofferrequest.CurrencyId,
                CountryId = sparepartsofferrequest.CountryId,
            };

            return mSparePartsOfferRequest;
        }

        public async Task<Guid> UpdateSparepartsOfferRequestAsync(SparepartsOfferRequest SparepartsOfferRequest)
        {
            SparepartsOfferRequest.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SparepartsOfferRequest.UpdatedOn = DateTime.Now;

            context.Entry(SparepartsOfferRequest).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return SparepartsOfferRequest.Id;
        }
    }
}
