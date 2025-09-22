using Application.Features.Identity.Users;
using Application.Features.Spares;
using Application.Features.Spares.Responses;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using System.Collections.Generic;


namespace Infrastructure.Services
{
    public class OfferRequestProcessService(ApplicationDbContext context, ICurrentUserService currentUserService) : IOfferRequestProcessService
    {
        public async Task<Guid> CreateOfferRequestProcessAsync(OfferRequestProcess OfferRequestProcess)
        {
            OfferRequestProcess.UserId = Guid.Parse(currentUserService.GetUserId());
            OfferRequestProcess.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            OfferRequestProcess.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            OfferRequestProcess.CreatedOn = DateTime.Now;
            OfferRequestProcess.UpdatedOn = DateTime.Now;
            OfferRequestProcess.IsActive = true;
            OfferRequestProcess.IsDeleted = false;

            await context.OfferRequestProcess.AddAsync(OfferRequestProcess);
            await context.SaveChangesAsync();
            return OfferRequestProcess.Id;
        }

        public async Task<bool> DeleteOfferRequestProcessAsync(Guid id)
        {
            var deletedSparepart = await context.OfferRequestProcess.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedSparepart == null) return true;

            deletedSparepart.IsDeleted = true;
            deletedSparepart.IsActive = false;

            context.Entry(deletedSparepart).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<OfferRequestProcessResponse> GetOfferRequestProcessAsync(Guid id)
        {
            var process = await context.OfferRequestProcess.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
            return GetOfferRequestProcess(process);
        }
        public async Task<OfferRequestProcess> GetOfferRequestProcessEntityAsync(Guid id)
         => await context.OfferRequestProcess.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        public async Task<List<OfferRequestProcessResponse>> GetOfferRequestProcessesAsync(Guid offerRequestId)
        {
            var process = await context.OfferRequestProcess.Where(x => x.OfferRequestId == offerRequestId).OrderBy(x => x.StageIndex).ToListAsync();
            List<OfferRequestProcessResponse> lstProcesses = new();
            foreach (OfferRequestProcess pro in process)
            {
                lstProcesses.Add(GetOfferRequestProcess(pro));
            }

            return lstProcesses;
        }
        public OfferRequestProcessResponse GetOfferRequestProcess(OfferRequestProcess OfferRequestProcess)
        {
            var mOfferRequestProcess = new OfferRequestProcessResponse()
            {
                Id = OfferRequestProcess.Id,
                CreatedBy = OfferRequestProcess.CreatedBy,
                CreatedOn = OfferRequestProcess.CreatedOn,
                IsActive = OfferRequestProcess.IsActive,
                UserId = OfferRequestProcess.UserId,
                Comments = OfferRequestProcess.Comments,
                OfferRequestId = OfferRequestProcess.OfferRequestId,
                Stage = OfferRequestProcess.Stage,
                StageIndex = OfferRequestProcess.StageIndex,
                StageName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == OfferRequestProcess.Stage)?.ItemName,
                Index = OfferRequestProcess.Index,
                IsCompleted = OfferRequestProcess.IsCompleted,
                PaymentTypeId = OfferRequestProcess.PaymentTypeId,
                PaymentType = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == OfferRequestProcess.PaymentTypeId)?.ItemName,
                PayAmt = OfferRequestProcess.PayAmt,
                BaseCurrencyAmt = OfferRequestProcess.BaseCurrencyAmt,
                PayAmtCurrencyId = OfferRequestProcess.PayAmtCurrencyId,
                PayAmtCurrency = context.Currency.FirstOrDefault(x => x.Id == OfferRequestProcess.PayAmtCurrencyId)?.Code
            };


            return mOfferRequestProcess;
        }

        public async Task<Guid> UpdateOfferRequestProcessAsync(OfferRequestProcess OfferRequestProcess)
        {
            OfferRequestProcess.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            OfferRequestProcess.UpdatedOn = DateTime.Now;

            context.Entry(OfferRequestProcess).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return OfferRequestProcess.Id;
        }
    }
}
