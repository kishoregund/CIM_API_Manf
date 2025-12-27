using Application.Features.Identity.Users;
using Application.Features.Spares;
using Application.Features.Spares.Responses;
using Domain.Entities;
using Domain.Views;
using Infrastructure.Common;
using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;


namespace Infrastructure.Services
{
    public class OfferRequestService(ApplicationDbContext context, ICurrentUserService currentUserService, IConfiguration configuration) : IOfferRequestService
    {
        public async Task<Guid> CreateOfferRequestAsync(OfferRequest OfferRequest)
        {
            var resultNO = context.OfferRequest.Where(p => p.CreatedOn.Year == DateTime.Now.Year && p.CreatedOn.Month == DateTime.Now.Month)
                                                                        .GroupBy(p => p.CreatedOn.Month)
                                                                        .Select(p => new OfferRequestNo { Month = p.Key, Count = p.Count() }).ToList();

            var offreqnoFormat = $"OR{DateTime.Now.Year.ToString().Substring(2, 2)}{DateTime.Now.Month.ToString()}";

            if (resultNO.Count > 0)
            {
                offreqnoFormat += (resultNO[0].Count + 1).ToString();
            }
            else
            {
                offreqnoFormat += "1";
            }

            OfferRequest.OffReqNo = offreqnoFormat;
            OfferRequest.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            OfferRequest.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            OfferRequest.CreatedOn = DateTime.Now;
            OfferRequest.UpdatedOn = DateTime.Now;            
            OfferRequest.IsActive = true;
            OfferRequest.IsDeleted = false;

            await context.OfferRequest.AddAsync(OfferRequest);
            await context.SaveChangesAsync();

            await SendEmailAsync(offreqnoFormat);
            return OfferRequest.Id;
        }

        private async Task<bool> SendEmailAsync(string offreqnoFormat)
        {
            try
            {
                var email = "";
                CommonMethods commonMethods = new CommonMethods(context, currentUserService, configuration);

                VW_UserProfile userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));
                if (userProfile.ContactType == "DR")
                {
                    email = userProfile.Email;
                }
                else
                {
                    Site site = await context.Site.Where(x => x.Id == userProfile.EntityChildId).FirstOrDefaultAsync();
                    var lstEmails = await (from users in context.VW_UserProfile.Where(x => x.SegmentCode == "RDTSP" && x.EntityChildId == site.RegionId)
                                           select users.Email).ToListAsync();

                    email = lstEmails.Aggregate("", (current, t) => current + (t + ","));
                    if (email.Length > 1) email = email.Remove(email.Length - 1, 1);
                }
                var body = $"Hi,<br><br> Greetings! <br><br>PO having Request No.: {offreqnoFormat} has been raised by Customer. <br>"
                        + "Kindly look into it and update at the earliest. "
                        + "<br><br><br>Thank you,<br>Avante Team<br>";
                body += "<br><br><br><br> *This is a system generated email and you will get the exact schedule date and time from engineer and service coordinator";
                const string subject = "PO Raised!";

                commonMethods.SendEmailMethod(email, body, subject);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteOfferRequestAsync(Guid id)
        {
            var deletedSparepart = await context.OfferRequest.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedSparepart == null) return true;

            deletedSparepart.IsDeleted = true;
            deletedSparepart.IsActive = false;

            context.Entry(deletedSparepart).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<OfferRequestResponse> GetOfferRequestAsync(Guid id)
        {
            var offer = await context.OfferRequest.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
            return GetOfferRequest(offer);
        }

        public async Task<OfferRequest> GetOfferRequestEntityAsync(Guid id)
         => await context.OfferRequest.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        public async Task<List<OfferRequestResponse>> GetOfferRequestsAsync()
        {
            try
            {
                var commonMethods = new CommonMethods(context, currentUserService, configuration);
                var userProfile = await context.UserProfiles.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());
                var lstRegionsprofile = await commonMethods.GetDistRegionsByUserIdAsync();
                var role = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == userProfile.SegmentId && x.ListCode == "SEGMENTS")?.ItemCode;

                var lstSites = await commonMethods.GetSitesByUserIdAsync();

                //var privilage = context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "OFREQ" && x.UserName != "admin");
                List<OfferRequest> offerRequests = new();
                var offReq = context.OfferRequest.Where(x => !x.IsDeleted && lstSites.Contains(x.CustomerSiteId.ToString())).ToList();

                //if (privilage != null && privilage.PrivilageCode != "PARTS" &&
                //    (privilage._create || privilage._read || privilage._update || privilage._delete))
                //    offReq = offReq.Where(x => x.Createdby == userId);

                if (role != null && role == "RCUST")
                {
                    offerRequests = (from ofReq in offReq
                                     join distReg in context.Regions on ofReq.DistributorId equals distReg.DistId
                                     where lstRegionsprofile.Contains(distReg.DistId.ToString())
                                     select ofReq)
                        .DistinctBy(x => x.Id)
                        .OrderByDescending(x => x.CreatedOn).ToList();
                }
                else
                {
                    offerRequests = (from ofReq in offReq
                                        join distReg in context.Regions on ofReq.DistributorId equals distReg.DistId
                                        where lstRegionsprofile.Contains(distReg.Id.ToString())
                                        select ofReq)
                        //select new { ofReq, distReg })
                        //.Where(x => lstRegionsprofile.Contains(x.distReg.Id.ToString()) && lstSites.Contains(x.ofReq.CustomerSiteId.ToString()))
                        .DistinctBy(x => x.Id)
                        .OrderByDescending(x => x.CreatedOn).ToList();
                }

                
                return offerRequests.Select(Of => GetOfferRequest(Of)).Where(x => x != null).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public OfferRequestResponse GetOfferRequest(OfferRequest offerRequest)
        {
            var stageLst = context.OfferRequestProcess.Where(x => !x.IsDeleted && x.OfferRequestId == offerRequest.Id && x.IsCompleted == true)
                .OrderByDescending(x => x.UpdatedOn)
                .ToList();

            var stage="";
            var isCompleted = false;
            var isShipment = false;
            //var stageLst = ofReqP.Where(x => x.OfferRequestId == offerRequest.Id && x.IsCompleted == true).OrderByDescending(x => x.UpdatedOn).ToList();
            var compDate = "";
            var compComments = "";
            decimal totalAmt = 0;

            if (stageLst.Count > 0)
            {
                stage = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == stageLst[stageLst.Count - 1].Stage)?.ItemName;
                stage = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stage);
                stage = stage.Replace('_', ' ');

                var compStage = context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "COMPT" && x.ListCode == "OFRQP");
                var shipmentStage = context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "SHDOC" && x.ListCode == "OFRQP");

                OfferRequestProcess completedStage = null;
                OfferRequestProcess shipmntStage = null;

                
                if (compStage != null)
                    completedStage = stageLst.FirstOrDefault(x => x.Stage == compStage.ListTypeItemId);

                if (compStage != null)
                    shipmntStage = stageLst.FirstOrDefault(x => x.Stage == shipmentStage.ListTypeItemId);

                if (completedStage != null)
                {
                    isCompleted = true;
                    compDate = completedStage.CreatedOn.ToString("MM/dd/yyyy");
                    compComments = completedStage.Comments;
                }

                isShipment = shipmntStage != null;

                foreach (var i in stageLst)
                {
                    totalAmt += Convert.ToDecimal(i.BaseCurrencyAmt * i.PayAmt);
                }

            }



            var mOfferrequest = new OfferRequestResponse()
            {
                Id = offerRequest.Id,
                Createdby = offerRequest.CreatedBy,
                CreatedOn = offerRequest.CreatedOn,
                UpdatedBy = offerRequest.UpdatedBy,
                UpdatedOn = offerRequest.UpdatedOn,
                IsActive = offerRequest.IsActive,
                DistributorId = offerRequest.DistributorId,
                Distributor = context.Distributor.FirstOrDefault(x => x.Id == offerRequest.DistributorId).DistName,
                TotalAmount = totalAmt,
                CustomerSiteId = offerRequest.CustomerSiteId,
                CurrencyId = offerRequest.CurrencyId,
                Status = offerRequest.Status,
                PoDate = offerRequest.PoDate,
                SpareQuoteNo = offerRequest.SpareQuoteNo,
                OtherSpareDesc = offerRequest.OtherSpareDesc,
                OffReqNo = offerRequest.OffReqNo,
                PaymentTerms = offerRequest.PaymentTerms,
                CustomerId = offerRequest.CustomerId,
                CustomerName = context.Customer.FirstOrDefault(x => x.Id == offerRequest.CustomerId)?.CustName,
                InstrumentsList = offerRequest.Instruments,
                StageName = stage,
                IsCompleted = isCompleted,
                CompletedComments = compComments,
                CompletedDate = compDate,
                IsShipment = isShipment,
                IsDistUpdated = offerRequest.IsDistUpdated,

                InspectionChargesCurr = offerRequest.InspectionChargesCurr,
                InspectionChargesAmt = offerRequest.InspectionChargesAmt,
                AirFreightChargesAmt = offerRequest.AirFreightChargesAmt,
                AirFreightChargesCurr = offerRequest.AirFreightChargesCurr,
                LcAdministrativeChargesAmt = offerRequest.LcAdministrativeChargesAmt,
                LcAdministrativeChargesCurr = offerRequest.LcadministrativeChargesCurr,
                BasePCurrencyAmt = offerRequest.BasePCurrencyAmt,
                TotalAmt = offerRequest.TotalAmt,
                TotalCurr = offerRequest.TotalCurr,

            };

            return mOfferrequest;
        }

        public async Task<Guid> UpdateOfferRequestAsync(OfferRequest OfferRequest)
        {
            OfferRequest.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            OfferRequest.UpdatedOn = DateTime.Now;

            context.Entry(OfferRequest).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return OfferRequest.Id;
        }
        public async Task<List<SparepartsOfferRequestResponse>> GetSparepartsByInstrumentPartNoAsync(string instrumentIds, string partNo)
        {
            var sps = new List<Sparepart>();

            var sp = await context.Spareparts.Where(x => !x.IsDeleted).ToListAsync();

            var listInstrument = instrumentIds.ToUpper().Split(",");
            var lstSpares = await context.InstrumentSpares.Where(x => listInstrument.Contains(x.InstrumentId.ToString().ToUpper())).ToListAsync();

            foreach (var ins in lstSpares.Select(item => sp
                         .Where(x => x.PartNo.ToLower().StartsWith(partNo.ToLower()) && x.Id == item.SparepartId)
                         .OrderBy(x => x.PartNo).ToList()))
            {
                sps.AddRange(ins);
            }

            return (sps.Select(inst => GetSparePartsByPartNo(inst)).Where(x => x != null).ToList()).Adapt<List<SparepartsOfferRequestResponse>>();
        }

        public SparepartsOfferRequestResponse GetSparePartsByPartNo(Sparepart sparePartsOfferRequest)
        {
            var mSparePartsOfferRequest = new SparepartsOfferRequestResponse
            {
                Id = sparePartsOfferRequest.Id,
                HsCode = sparePartsOfferRequest.HsCode,
                PartNo = sparePartsOfferRequest.PartNo,
                Qty = sparePartsOfferRequest.Qty,
                Price = sparePartsOfferRequest.Price,
                DiscountPercentage = 0,
                AfterDiscount = 0,
                Amount = 0,
                ItemDescription = sparePartsOfferRequest.ItemDesc,// context.Spareparts.FirstOrDefault(x => x.Id == sparePartsOfferRequest.Id)?.ItemDesc,
                Currency = context.Currency.FirstOrDefault(x => x.Id == sparePartsOfferRequest.CurrencyId)?.Code,
            };
            return mSparePartsOfferRequest;
        }

    }

    public class OfferRequestNo
    {
        public int Month { get; set; }
        public int Count { get; set; }
    }
}
