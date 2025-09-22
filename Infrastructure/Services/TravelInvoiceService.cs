using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using Application.Features.Travels;
using Application.Features.Travels.Responses;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;
using Application.Features.AppBasic.Responses;

namespace Infrastructure.Services
{
    public class TravelInvoiceService(ApplicationDbContext context, ICurrentUserService currentUserService) : ITravelInvoiceService
    {

        public Task<TravelInvoice> GetTravelInvoiceEntityAsync(Guid id)
            => context.TravelInvoice.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        public async Task<TravelInvoiceResponse> GetTravelInvoiceByIdAsync(Guid id)
        {
            var exp = await context.TravelInvoice.FirstOrDefaultAsync(x => x.Id == id);
            return GetTravelInvoice(exp);
        }

        public async Task<List<TravelInvoiceResponse>> GetTravelInvoicesAsync(string businessUnitId, string brandId)
        {
            ServiceRequestService serviceRequestService = new ServiceRequestService(context, currentUserService);
            var serReqs = await serviceRequestService.GetDetailServiceRequestsOnlyAsync(businessUnitId, brandId);
            List<TravelInvoice> TravelInvoices = new();
            TravelInvoices = (from a in context.TravelInvoice.Where(x => !x.IsDeleted).ToList()
                              join b in serReqs on a.ServiceRequestId equals b.Id
                              select a).ToList();


            return TravelInvoices.Select(exp => GetTravelInvoice(exp)).ToList();
        }

        public TravelInvoiceResponse GetTravelInvoice(TravelInvoice travelInvoice)
        {
            var eng = context.RegionContact.FirstOrDefault(x => x.Id == travelInvoice.EngineerId);

            var mTravelInvoice = new TravelInvoiceResponse
            {
                Id = travelInvoice.Id,

                AmountBuild = travelInvoice.AmountBuild,
                ServiceRequestNo = context.ServiceRequest.FirstOrDefault(x => x.Id == travelInvoice.ServiceRequestId).SerReqNo,
                DistributorId = travelInvoice.DistributorId,
                EngineerName = eng?.FirstName + " " + eng?.LastName,
                EngineerId = travelInvoice.EngineerId,
                ServiceRequestId = travelInvoice.ServiceRequestId,
            };

            return mTravelInvoice;
        }

        public async Task<Guid> CreateTravelInvoiceAsync(TravelInvoice TravelInvoice)
        {
            TravelInvoice.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            TravelInvoice.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            TravelInvoice.CreatedOn = DateTime.Now;
            TravelInvoice.UpdatedOn = DateTime.Now;
            TravelInvoice.IsActive = true;
            TravelInvoice.IsDeleted = false;

            await context.TravelInvoice.AddAsync(TravelInvoice);
            await context.SaveChangesAsync();
            return TravelInvoice.Id;
        }

        public async Task<bool> DeleteTravelInvoiceAsync(Guid id)
        {
            var deletedTravelInvoice = await context.TravelInvoice.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedTravelInvoice == null) return true;

            deletedTravelInvoice.IsDeleted = true;
            deletedTravelInvoice.IsActive = false;

            context.Entry(deletedTravelInvoice).State = EntityState.Deleted;          
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateTravelInvoiceAsync(TravelInvoice TravelInvoice)
        {
            TravelInvoice.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            TravelInvoice.UpdatedOn = DateTime.Now;

            context.Entry(TravelInvoice).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return TravelInvoice.Id;
        }

    }
}
