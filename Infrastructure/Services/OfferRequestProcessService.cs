using Application.Features.Identity.Users;
using Application.Features.Spares;
using Application.Features.Spares.Responses;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using System.Collections.Generic;
using System.Text;


namespace Infrastructure.Services
{
    public class OfferRequestProcessService(ApplicationDbContext context, ICurrentUserService currentUserService, CommonMethods commonMethods) : IOfferRequestProcessService
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

            _ = SendOfferRequestProcessEmailAsync(OfferRequestProcess, "CREATE");

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

            _ = SendOfferRequestProcessEmailAsync(OfferRequestProcess, "UPDATE");

            return OfferRequestProcess.Id;
        }

        private async Task SendOfferRequestProcessEmailAsync(OfferRequestProcess offerProcess, string action)
        {
            try
            {
                var offerRequest = await context.OfferRequest
                    .FirstOrDefaultAsync(x => x.Id == offerProcess.OfferRequestId && !x.IsDeleted);

                if (offerRequest == null) return;

                var distributor = await context.Distributor
                    .FirstOrDefaultAsync(x => x.Id == offerRequest.DistributorId && x.IsActive);

                if (distributor == null) return;

                var regions = await context.Regions
                    .Where(x => x.DistId == distributor.Id && x.IsActive)
                    .Select(x => x.Id)
                    .ToListAsync();

                if (regions.Count == 0) return;

                var regionContacts = await context.RegionContact
                    .Where(x => regions.Contains(x.RegionId) && x.IsActive)
                    .ToListAsync();

                if (regionContacts.Count == 0) return;

                var stageName = context.VW_ListItems
                    .FirstOrDefault(x => x.ListTypeItemId == offerProcess.Stage)?.ItemName ?? "Unknown Stage";

                var emailAddresses = string.Join(",", regionContacts.Select(x => x.PrimaryEmail).Where(x => !string.IsNullOrEmpty(x)).Distinct());

                if (string.IsNullOrEmpty(emailAddresses)) return;

                var emailBody = BuildOfferRequestProcessEmailBody(offerRequest, offerProcess, stageName, action);
                var subject = $"Offer Request {action} - {offerRequest.OffReqNo}";

                commonMethods.SendEmailMethod(emailAddresses, emailBody, subject);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SendOfferRequestProcessEmailAsync] Error: {ex.Message}");
            }
        }

        private string BuildOfferRequestProcessEmailBody(OfferRequest offerRequest, OfferRequestProcess offerProcess, string stageName, string action)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html><body style='font-family: Arial, sans-serif;'>");
            sb.AppendLine("<div style='max-width: 600px; margin: 0 auto;'>");

            sb.AppendLine($"<h2 style='color: #333;'>Offer Request {action}</h2>");
            sb.AppendLine("<hr style='border: none; border-top: 1px solid #ccc;'/>");

            sb.AppendLine("<table style='width: 100%; border-collapse: collapse;'>");
            sb.AppendLine($"<tr><td style='padding: 8px; font-weight: bold; width: 40%;'>Offer Request No:</td><td style='padding: 8px;'>{offerRequest.OffReqNo}</td></tr>");
            sb.AppendLine($"<tr><td style='padding: 8px; font-weight: bold;'>Stage:</td><td style='padding: 8px;'>{stageName}</td></tr>");
            sb.AppendLine($"<tr><td style='padding: 8px; font-weight: bold;'>Total Amount:</td><td style='padding: 8px;'>{offerRequest.TotalAmount} {GetCurrencyCode(offerRequest.CurrencyId)}</td></tr>");
            sb.AppendLine($"<tr><td style='padding: 8px; font-weight: bold;'>Action:</td><td style='padding: 8px; color: #d9534f;'><strong>{action}</strong></td></tr>");
            sb.AppendLine($"<tr><td style='padding: 8px; font-weight: bold;'>Updated On:</td><td style='padding: 8px;'>{offerProcess.UpdatedOn:dd-MMM-yyyy HH:mm:ss}</td></tr>");

            if (!string.IsNullOrEmpty(offerProcess.Comments))
            {
                sb.AppendLine($"<tr><td style='padding: 8px; font-weight: bold;'>Comments:</td><td style='padding: 8px;'>{offerProcess.Comments}</td></tr>");
            }

            sb.AppendLine("</table>");

            sb.AppendLine("<hr style='border: none; border-top: 1px solid #ccc;'/>");
            sb.AppendLine("<p style='color: #666; font-size: 12px;'><em>This is an automated email from Avante Grade CIM System. Please do not reply to this email.</em></p>");
            sb.AppendLine("</div>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }

        private string GetCurrencyCode(Guid currencyId)
        {
            return context.Currency
                .FirstOrDefault(x => x.Id == currencyId)?.Code ?? "Unknown";
        }
    }
}
