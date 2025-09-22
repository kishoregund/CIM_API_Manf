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
    public class TravelExpenseService(ApplicationDbContext context, ICurrentUserService currentUserService) : ITravelExpenseService
    {

        public Task<TravelExpense> GetTravelExpenseEntityByIdAsync(Guid id)
            => context.TravelExpenses.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        public async Task<TravelExpenseResponse> GetTravelExpenseByIdAsync(Guid id)
        {
            var exp = await context.TravelExpenses.FirstOrDefaultAsync(x => x.Id == id);
            return GetTravelExpense(exp);
        }

        public async Task<List<TravelExpenseResponse>> GetTravelExpensesAsync(string businessUnitId, string brandId)
        {
            ServiceRequestService serviceRequestService = new ServiceRequestService(context, currentUserService);
            var serReqs = await serviceRequestService.GetDetailServiceRequestsOnlyAsync(businessUnitId, brandId);
            List<TravelExpense> travelExpenses = new();
            travelExpenses = (from a in context.TravelExpenses.Where(x => !x.IsDeleted).ToList()
                              join b in serReqs on a.ServiceRequestId equals b.Id
                              select a).ToList();


            return travelExpenses.Select(exp => GetTravelExpense(exp)).ToList();

        }

        public TravelExpenseResponse GetTravelExpense(TravelExpense TravelExpense)
        {
            var eng = context.RegionContact.FirstOrDefault(x => x.Id == TravelExpense.EngineerId);

            var mTravelExpense = new TravelExpenseResponse()
            {
                Id = TravelExpense.Id,
                IsActive = TravelExpense.IsActive,
                EndDate = TravelExpense.EndDate,
                EngineerId = TravelExpense.EngineerId,
                EngineerName = eng?.FirstName + " " + eng?.LastName,
                DistributorId = TravelExpense.DistributorId,
                CustomerId = TravelExpense.CustomerId,
                ServiceRequestId = TravelExpense.ServiceRequestId,
                ServiceRequestNo = context.ServiceRequest.FirstOrDefault(x => x.Id == TravelExpense.ServiceRequestId).SerReqNo,
                StartDate = TravelExpense.StartDate,
                GrandCompanyTotal = TravelExpense.GrandCompanyTotal,
                GrandEngineerTotal = TravelExpense.GrandEngineerTotal,
                Designation = TravelExpense.Designation,
                TotalDays = TravelExpense.TotalDays,
            };
            return mTravelExpense;
        }

        public async Task<Guid> CreateTravelExpenseAsync(TravelExpense TravelExpense)
        {
            TravelExpense.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            TravelExpense.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            TravelExpense.CreatedOn = DateTime.Now;
            TravelExpense.UpdatedOn = DateTime.Now;
            TravelExpense.IsActive = true;
            TravelExpense.IsDeleted = false;

            await context.TravelExpenses.AddAsync(TravelExpense);
            await context.SaveChangesAsync();
            return TravelExpense.Id;
        }

        public async Task<bool> DeleteTravelExpenseAsync(Guid id)
        {
            var deletedTravelExpense = await context.TravelExpenses.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedTravelExpense == null) return true;

            deletedTravelExpense.IsDeleted = true;
            deletedTravelExpense.IsActive = false;

            context.Entry(deletedTravelExpense).State = EntityState.Deleted;

            var items = context.TravelExpenseItems.Where(x => x.TravelExpenseId == id);
            foreach (var item in items)
            {
                //item.IsDeleted = true;
                //item.IsActive = false;
                context.Entry(item).State = EntityState.Deleted;
            }
            
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateTravelExpenseAsync(TravelExpense TravelExpense)
        {
            TravelExpense.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            TravelExpense.UpdatedOn = DateTime.Now;

            context.Entry(TravelExpense).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return TravelExpense.Id;
        }

    }
}
