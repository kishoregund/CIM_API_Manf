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
    public class BankDetailsService(ApplicationDbContext context, ICurrentUserService currentUserService) : IBankDetailsService
    {

        public Task<BankDetails> GetBankDetailsEntityByIdAsync(Guid id)
            => context.BankDetails.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        public async Task<BankDetails> GetBankDetailsByIdAsync(Guid id)
        {
            var exp = await context.BankDetails.FirstOrDefaultAsync(x => x.Id == id);
            return exp;
        }

        public async Task<BankDetails> GetBankDetailsByContactIdAsync(Guid contactId)
        {
            var exp = await context.BankDetails.FirstOrDefaultAsync(x => x.ContactId == contactId);
            return exp;
        }

        public async Task<Guid> CreateBankDetailsAsync(BankDetails BankDetails)
        {
            BankDetails.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            BankDetails.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            BankDetails.CreatedOn = DateTime.Now;
            BankDetails.UpdatedOn = DateTime.Now;
            BankDetails.IsActive = true;
            BankDetails.IsDeleted = false;

            await context.BankDetails.AddAsync(BankDetails);
            await context.SaveChangesAsync();
            return BankDetails.Id;
        }

        public async Task<bool> DeleteBankDetailsAsync(Guid id)
        {
            var deletedBankDetails = await context.BankDetails.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedBankDetails == null) return true;

            deletedBankDetails.IsDeleted = true;
            deletedBankDetails.IsActive = false;

            context.Entry(deletedBankDetails).State = EntityState.Deleted;

            return true;
        }

        public async Task<Guid> UpdateBankDetailsAsync(BankDetails BankDetails)
        {
            BankDetails.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            BankDetails.UpdatedOn = DateTime.Now;

            context.Entry(BankDetails).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return BankDetails.Id;
        }

    }
}
