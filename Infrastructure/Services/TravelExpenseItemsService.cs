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
    public class TravelExpenseItemservice(ApplicationDbContext context, ICurrentUserService currentUserService) : ITravelExpenseItemsService
    {

        public Task<TravelExpenseItems> GetTravelExpenseItemsEntityAsync(Guid id)
            => context.TravelExpenseItems.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        public async Task<TravelExpenseItemsResponse> GetTravelExpenseItemsByIdAsync(Guid id)
        {
            var exp = await context.TravelExpenseItems.FirstOrDefaultAsync(x => x.Id == id);
            return GetTravelExpenseItems(exp);
        }

        public async Task<List<TravelExpenseItemsResponse>> GetTravelExpenseItemsAsync(Guid travelExpenseId)
        {
            var expenseItems = await context.TravelExpenseItems.Where(x=>x.TravelExpenseId == travelExpenseId).ToListAsync();
            List<TravelExpenseItemsResponse> travelExpenseItemsResponses = new();
            foreach (TravelExpenseItems item in expenseItems)
            {
                travelExpenseItemsResponses.Add(GetTravelExpenseItems(item));
            }

            return travelExpenseItemsResponses;
            //return  expenseItems.Select(exp => GetTravelExpenseItems(exp)).Where(x => x != null).ToList();
        }

        public TravelExpenseItemsResponse GetTravelExpenseItems(TravelExpenseItems travelExpenseItems)
        {            
            var mTravelExpenseItems = new TravelExpenseItemsResponse()
            {
                Id = travelExpenseItems.Id,
                IsActive = travelExpenseItems.IsActive,
                CreatedBy = travelExpenseItems.CreatedBy,
                CreatedOn = travelExpenseItems.CreatedOn,
                TravelExpenseId = travelExpenseItems.TravelExpenseId,
                ExpDate = travelExpenseItems.ExpDate,
                ExpDetails = travelExpenseItems.ExpDetails,
                BcyAmt = travelExpenseItems.BcyAmt,
                UsdAmt = travelExpenseItems.UsdAmt,
                Currency = travelExpenseItems.Currency,
                CurrencyName = context.Currency.FirstOrDefault(x => x.Id == travelExpenseItems.Currency)?.Code,
                IsBillsAttached = travelExpenseItems.IsBillsAttached,
                Remarks = travelExpenseItems.Remarks,
                ExpNature = travelExpenseItems.ExpNature,
                ExpenseBy = travelExpenseItems.ExpenseBy,
                ExpNatureName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == travelExpenseItems.ExpNature)?.ItemName,
                ExpenseByName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == travelExpenseItems.ExpenseBy)?.ItemName
            };
            return mTravelExpenseItems;
        }


        public async Task<Guid> CreateTravelExpenseItemsAsync(TravelExpenseItems TravelExpenseItems)
        {
            TravelExpenseItems.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            TravelExpenseItems.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            TravelExpenseItems.CreatedOn = DateTime.Now;
            TravelExpenseItems.UpdatedOn = DateTime.Now;
            TravelExpenseItems.IsActive = true;
            TravelExpenseItems.IsDeleted = false;

            await context.TravelExpenseItems.AddAsync(TravelExpenseItems);
            await context.SaveChangesAsync();
            return TravelExpenseItems.Id;
        }

        public async Task<bool> DeleteTravelExpenseItemsAsync(Guid id)
        {
            var deletedTravelExpenseItems = await context
                .TravelExpenseItems.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedTravelExpenseItems == null) return true;

            deletedTravelExpenseItems.IsDeleted = true;
            deletedTravelExpenseItems.IsActive = false;

            context.Entry(deletedTravelExpenseItems).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateTravelExpenseItemsAsync(TravelExpenseItems TravelExpenseItems)
        {
            TravelExpenseItems.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            TravelExpenseItems.UpdatedOn = DateTime.Now;

            context.Entry(TravelExpenseItems).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return TravelExpenseItems.Id;
        }

    }
}
