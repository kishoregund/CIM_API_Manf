using Application.Features.Identity.Users;
using Application.Features.Spares;
using Application.Features.Spares.Responses;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Console;
using System.Collections.Generic;
using System.Text;


namespace Infrastructure.Services
{
    public class OfferRequestProcessService(ApplicationDbContext context, ICurrentUserService currentUserService, CommonMethods commonMethods, IConfiguration configuration) : IOfferRequestProcessService
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

            // Send notifications to distributors and customers
            _ = SendOfferRequestProcessEmailAsync(OfferRequestProcess, "CREATE");
            _ = SendOfferRequestProcessToCustomerAsync(OfferRequestProcess, "CREATE");

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

            // Send notifications to distributors and customers
            _ = SendOfferRequestProcessEmailAsync(OfferRequestProcess, "UPDATE");
            _ = SendOfferRequestProcessToCustomerAsync(OfferRequestProcess, "UPDATE");

            return OfferRequestProcess.Id;
        }

        private async Task SendOfferRequestProcessEmailAsync(OfferRequestProcess offerProcess, string action)
        {
            try
            {
                var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));
                if (userProfile.SegmentCode == "RCUST")
                {
                    var offerRequest = await context.OfferRequest
                        .FirstOrDefaultAsync(x => x.Id == offerProcess.OfferRequestId && !x.IsDeleted);

                    if (offerRequest == null) return;
                                        
                    var regions = await context.Regions
                        .Where(x => x.DistId == offerRequest.DistributorId && x.IsActive)
                        .Select(x => x.Id)
                        .ToListAsync();

                    if (regions.Count == 0) return;


                    var regionContacts = await context.VW_UserProfile
                        .Where(x => regions.Contains(x.EntityChildId) && x.EntityParentId == offerRequest.DistributorId)
                        .ToListAsync();

                    if (regionContacts.Count == 0) return;

                    var stageName = context.VW_ListItems
                        .FirstOrDefault(x => x.ListTypeItemId == offerProcess.Stage)?.ItemName ?? "Unknown Stage";

                    var emailAddresses = string.Join(",", regionContacts.Select(x => x.Email).Where(x => !string.IsNullOrEmpty(x)).Distinct());

                    if (string.IsNullOrEmpty(emailAddresses)) return;

                    var emailBody = BuildOfferRequestProcessEmailBody(offerRequest, offerProcess, stageName, action);
                    var subject = $"Offer Request {action} - {offerRequest.OffReqNo}";

                    commonMethods.SendEmailMethod(emailAddresses, emailBody, subject);
                }
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

        /// <summary>
        /// Sends offer request process notifications to customer site contacts
        /// </summary>
        private async Task SendOfferRequestProcessToCustomerAsync(OfferRequestProcess offerProcess, string action)
        {
            try
            {
                var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));
                if (userProfile.SegmentCode == "RDTSP")
                {
                    var offerRequest = await context.OfferRequest
                    .FirstOrDefaultAsync(x => x.Id == offerProcess.OfferRequestId && !x.IsDeleted);

                    if (offerRequest == null) return;

                    // Get all site contacts for the customer by joining through Site
                    var siteContacts = await context.VW_UserProfile.Where(x => x.EntityParentId == offerRequest.CustomerId
                    && x.EntityChildId == offerRequest.CustomerSiteId).ToListAsync();

                    if (siteContacts.Count == 0) return;

                    var stageName = context.VW_ListItems
                        .FirstOrDefault(x => x.ListTypeItemId == offerProcess.Stage)?.ItemName ?? "Unknown Stage";

                    var emailAddresses = siteContacts
                        .Select(x => x.Email)
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Distinct()
                        .ToList();

                    if (emailAddresses.Count == 0) return;

                    // Create in-app notifications for site contacts
                    foreach (var contact in siteContacts)
                    {
                        try
                        {
                            var notification = new Notifications
                            {
                                Id = Guid.NewGuid(),
                                Remarks = $"Offer Request {offerRequest.OffReqNo} has been {action.ToLower()} at stage '{stageName}'. {(!string.IsNullOrEmpty(offerProcess.Comments) ? $"Comments: {offerProcess.Comments}" : "")}",
                                RoleId = contact.RoleId,
                                RaisedBy = currentUserService.GetUserId(),
                                UserId = contact.UserId,
                                IsActive = true,
                                IsDeleted = false,
                                CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                                CreatedOn = DateTime.Now,
                                UpdatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                                UpdatedOn = DateTime.Now
                            };

                            await context.Notifications.AddAsync(notification);
                            await context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[SendOfferRequestProcessToCustomer] Error creating notification: {ex.Message}");
                        }
                    }

                    // Send email to customer contacts
                    var emailBody = BuildOfferRequestProcessCustomerEmailBody(offerRequest, offerProcess, stageName, action);
                    var subject = $"Offer Request {action} - {offerRequest.OffReqNo}";
                    var emailAddressesString = string.Join(",", emailAddresses);

                    commonMethods.SendEmailMethod(emailAddressesString, emailBody, subject);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SendOfferRequestProcessToCustomerAsync] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Builds professional email body for customer notification
        /// </summary>
        private string BuildOfferRequestProcessCustomerEmailBody(OfferRequest offerRequest, OfferRequestProcess offerProcess, string stageName, string action)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset='utf-8' />");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Arial, sans-serif; color: #333; }");
            sb.AppendLine(".container { max-width: 600px; margin: 0 auto; padding: 20px; }");
            sb.AppendLine(".header { background-color: #0c5460; color: white; padding: 15px; border-radius: 5px; }");
            sb.AppendLine(".content { margin: 20px 0; }");
            sb.AppendLine(".info-table { width: 100%; border-collapse: collapse; margin: 15px 0; }");
            sb.AppendLine(".info-table tr { background-color: #f8f9fa; }");
            sb.AppendLine(".info-table td { padding: 10px; border: 1px solid #ddd; }");
            sb.AppendLine(".info-table .label { font-weight: bold; width: 35%; }");
            sb.AppendLine(".status { padding: 10px; background-color: #d1ecf1; border-left: 4px solid #0c5460; margin: 15px 0; }");
            sb.AppendLine(".footer { margin-top: 20px; padding-top: 15px; border-top: 1px solid #ddd; font-size: 12px; color: #666; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<div class='container'>");

            // Header
            sb.AppendLine("<div class='header'>");
            sb.AppendLine("<h2 style='margin: 0;'>📋 Offer Request Update</h2>");
            sb.AppendLine("</div>");

            // Status
            sb.AppendLine("<div class='status'>");
            sb.AppendLine($"<strong>✓ ACTION:</strong> Your offer request <strong>{offerRequest.OffReqNo}</strong> has been <strong style='color: #0c5460;'>{action.ToUpper()}</strong>");
            sb.AppendLine("</div>");

            // Content
            sb.AppendLine("<div class='content'>");
            sb.AppendLine("<h3>Offer Details:</h3>");
            sb.AppendLine("<table class='info-table'>");
            sb.AppendLine($"<tr><td class='label'>Offer Request No:</td><td>{offerRequest.OffReqNo}</td></tr>");
            sb.AppendLine($"<tr><td class='label'>Current Stage:</td><td><strong style='color: #0c5460;'>{stageName}</strong></td></tr>");
            sb.AppendLine($"<tr><td class='label'>Total Amount:</td><td>{offerRequest.TotalAmount} {GetCurrencyCode(offerRequest.CurrencyId)}</td></tr>");
            sb.AppendLine($"<tr><td class='label'>Last Updated:</td><td>{offerProcess.UpdatedOn:dd-MMM-yyyy HH:mm:ss}</td></tr>");

            if (!string.IsNullOrEmpty(offerProcess.Comments))
            {
                sb.AppendLine($"<tr><td class='label'>Additional Info:</td><td>{offerProcess.Comments}</td></tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine("</div>");

            // Action Items
            sb.AppendLine("<div style='margin-top: 20px; padding: 15px; background-color: #e7f3ff; border-left: 4px solid #2196F3;'>");
            sb.AppendLine("<h3 style='margin-top: 0;'>What's Next:</h3>");
            sb.AppendLine("<ul style='margin-bottom: 0;'>");
            sb.AppendLine("<li>Review the offer details at your convenience</li>");
            sb.AppendLine("<li>Contact our team if you have any questions</li>");
            sb.AppendLine("<li>We'll keep you updated on any further changes</li>");
            sb.AppendLine("</ul>");
            sb.AppendLine("</div>");

            // Footer
            sb.AppendLine("<div class='footer'>");
            sb.AppendLine("<p><em>This is an automated notification from Avante Grade CIM System. Please do not reply to this email.</em></p>");
            sb.AppendLine("<p><em>If you have questions, please contact our support team.</em></p>");
            sb.AppendLine("</div>");

            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
