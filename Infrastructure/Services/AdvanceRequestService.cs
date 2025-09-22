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

namespace Infrastructure.Services
{
    public class AdvanceRequestService(ApplicationDbContext context, ICurrentUserService currentUserService) : IAdvanceRequestService
    {

        public Task<AdvanceRequest> GetAdvanceRequestEntityByIdAsync(Guid id)
            => context.AdvanceRequest.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        public async Task<AdvanceRequestResponse> GetAdvanceRequestByIdAsync(Guid id)
        {
            var exp = await context.AdvanceRequest.FirstOrDefaultAsync(x => x.Id == id);
            return GetAdvanceRequest(exp);
        }

        public async Task<List<AdvanceRequestResponse>> GetAdvanceRequestsAsync(string businessUnitId, string brandId)
        {
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));
            List<AdvanceRequestResponse> AdvanceRequests = new();
            if (userProfile != null && userProfile.SegmentCode == "RDTSP")
            {
                ServiceRequestService serviceRequestService = new ServiceRequestService(context, currentUserService);
                var serReqs = await serviceRequestService.GetDetailServiceRequestsOnlyAsync(businessUnitId, brandId);
                var advance = (from a in context.AdvanceRequest.Where(x => !x.IsDeleted).ToList()
                               join b in serReqs on a.ServiceRequestId equals b.Id
                               select a).ToList();
                AdvanceRequests = advance.Select(exp => GetAdvanceRequest(exp)).ToList();
            }
            else if (userProfile != null && userProfile.SegmentCode == "RENG")
            {
                var advance = (from a in context.AdvanceRequest
                               join sr in context.ServiceRequest on a.ServiceRequestId equals sr.Id
                               where sr.AssignedTo == userProfile.ContactId
                               select a).ToList();
                AdvanceRequests = advance.Select(exp => GetAdvanceRequest(exp)).ToList();
            }
            else
            {
                AdvanceRequests = context.AdvanceRequest.Select(exp => GetAdvanceRequest(exp)).ToList();
            }

            return AdvanceRequests;
        }

        public AdvanceRequestResponse GetAdvanceRequest(AdvanceRequest AdvanceRequest)
        {
            var eng = context.RegionContact.FirstOrDefault(x => x.Id == AdvanceRequest.EngineerId);

            var nAdvanceRequest = new AdvanceRequestResponse()
            {
                Id = AdvanceRequest.Id,
                EngineerName = eng?.FirstName + " " + eng.LastName,
                ServiceRequestNo = context.ServiceRequest.FirstOrDefault(x => x.Id == AdvanceRequest.ServiceRequestId)?.SerReqNo,
                DistributorId = AdvanceRequest.DistributorId,
                AdvanceAmount = AdvanceRequest.AdvanceAmount,
                AdvanceCurrency = AdvanceRequest.AdvanceCurrency,
                ClientNameLocation = AdvanceRequest.ClientNameLocation,
                ServiceRequestId = AdvanceRequest.ServiceRequestId,
                CustomerId = AdvanceRequest.CustomerId,
                EngineerId = AdvanceRequest.EngineerId,
                IsBillable = AdvanceRequest.IsBillable,
                OfficeLocationId = AdvanceRequest.OfficeLocationId,
                ReportingManager = AdvanceRequest.ReportingManager,
                UnderTaking = AdvanceRequest.UnderTaking,
            };

            return nAdvanceRequest;
        }

        public async Task<Guid> CreateAdvanceRequestAsync(AdvanceRequest AdvanceRequest)
        {
            AdvanceRequest.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            AdvanceRequest.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            AdvanceRequest.CreatedOn = DateTime.Now;
            AdvanceRequest.UpdatedOn = DateTime.Now;
            AdvanceRequest.IsActive = true;
            AdvanceRequest.IsDeleted = false;

            await context.AdvanceRequest.AddAsync(AdvanceRequest);
            await context.SaveChangesAsync();
            return AdvanceRequest.Id;
        }

        public async Task<bool> DeleteAdvanceRequestAsync(Guid id)
        {
            var deletedAdvanceRequest = await context.AdvanceRequest.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedAdvanceRequest == null) return true;

            deletedAdvanceRequest.IsDeleted = true;
            deletedAdvanceRequest.IsActive = false;

            context.Entry(deletedAdvanceRequest).State = EntityState.Deleted;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateAdvanceRequestAsync(AdvanceRequest AdvanceRequest)
        {
            AdvanceRequest.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            AdvanceRequest.UpdatedOn = DateTime.Now;

            context.Entry(AdvanceRequest).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return AdvanceRequest.Id;
        }

    }
}
