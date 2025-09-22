using Application.Features.AMCS;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using Application.Models;
using Infrastructure.Common;
using System.ComponentModel.Design;
using Application.Features.AMCS.Responses;

namespace Infrastructure.Services
{
    public class AmcStagesService(ApplicationDbContext context, ICurrentUserService currentUserService) : IAmcStagesService
    {
        public async Task<Guid> CreateAmcStages(AMCStages amcStages)
        {
            var existing = context.AMCStages.Any(x => x.Stage == amcStages.Stage && x.AMCId == amcStages.AMCId && x.Index == amcStages.Index);

            if (existing)
            {
                amcStages.Index++;
            }

            amcStages.CreatedOn = DateTime.Now;
            amcStages.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            amcStages.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            amcStages.UpdatedOn = DateTime.Now;
            amcStages.IsActive = true;
            amcStages.IsDeleted = false;

            await context.AMCStages.AddAsync(amcStages);
            await context.SaveChangesAsync();

            ///Send Email Notification
            #region Email Notification

            /*
            //var commonMethod = new CommonMethods(_context);

            //var users = _context.Users.ToList();
            //var vwListType = _context.VW_ListItems.ToList();
            var amc = context.AMC.FirstOrDefault(x => x.Id == amcStages.AMCId);
            //var vw_UserProfiles = _context.VW_UserProfiles.ToList();

            //mAmcStages.Stage = vwListType.FirstOrDefault(x => x.ListTypeItemId == mAmcStages.Stage)?.Itemname;

            var body = $"AMC Process has been Updated to <b>{context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == amcStages.Stage)?.ItemName}</b>";
            var email = "";
            var cc = "";

            var loggedInUserRole = context.VW_UserProfile.FirstOrDefault(x => x.UserId == amcStages.CreatedBy);
            //var loggedInUserRole = vwListType.FirstOrDefault(x => x.ListTypeItemId == loggedInUserRoleId.Roleid)?.ItemCode;

            var createdUserRole = context.VW_UserProfile.FirstOrDefault(x => x.UserId == amc.CreatedBy);
            //var createdUserRole = vwListType.FirstOrDefault(x => x.ListTypeItemId == createdUserRoleId.Roleid)?.ItemCode;

            if (loggedInUserRole.SegmentCode == "RDTSP")
            {
                email = loggedInUserRole.Email;//  users.FirstOrDefault(x => x.Id == userId)?.Email;

                if (createdUserRole.SegmentCode == "RCUST") cc = createdUserRole.Email; //users.FirstOrDefault(x => x.Id == amc.Createdby)?.Email;

                else if (createdUserRole.SegmentCode == "RDTSP")
                {
                    //var siteObj = new SiteDL(_context);
                    var siteContacts = context.SiteContact.Where(x => x.Id == amc.CustSite).ToList();//  siteObj.GetSite(amc.CustSite, "");
                    foreach (var item in siteContacts) cc += item.PrimaryEmail + ",";
                }
            }

            else if (loggedInUserRole.SegmentCode == "RCUST")
            {
                email = createdUserRole.Email; // users.FirstOrDefault(x => x.Id == amc.Createdby)?.Email;

                if (createdUserRole.SegmentCode == "RCUST") cc = createdUserRole.Email;// users.FirstOrDefault(x => x.Id == amc.Createdby)?.Email;

                else if (createdUserRole.SegmentCode == "RDTSP")
                {
                    var cust = context.Customer.FirstOrDefault(x => x.Id == amc.BillTo);
                    var vw_UserProfiles = context.VW_UserProfile.Where(x=>x.SegmentCode == "RDTSP").ToList();

                    foreach (var item in vw_UserProfiles)
                    {
                        var regions = item.DistRegions?.Split(',');
                        if (regions != null && regions.Count() > 0 && regions.Contains(cust?.DefDistRegionId.ToString())) cc += item.Email + ",";
                    }
                }
            }

            //if (!string.IsNullOrEmpty(email)) commonMethod.sendMail(_appSettings, body, email, "AMC Process Changed", cc);
            */
            #endregion

            #region Notification
            //var notification = new NotificationDL(_context);
            //var offerNo = _context.AMC.FirstOrDefault(x => x.Id == mAmcStages.ParentId);
            //notification.MapUserToNotification(userId, $"AMC  {offerNo?.Servicequote}'s Process has been updated", companyId, offerNo.Createdby);
            #endregion

            return amcStages.Id;
        }

        public async Task<bool> DeleteAmcStages(Guid id)
        {
            var deletedAmcStage = context.AMCStages.FirstOrDefault(x => x.Id == id);

            if (deletedAmcStage == null) return true;

            //deletedAmcStage.IsActive = false;
            //deletedAmcStage.IsDeleted = true;

            context.Entry(deletedAmcStage).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<AmcStagesResponse>> GetAllByAmcIdAsync(Guid amcId)
        {
            var amcStages = await (from s in context.AMCStages
                                   join li in context.VW_ListItems on s.Stage equals li.ListTypeItemId.ToString()
                                   where s.AMCId == amcId
                                   select new AmcStagesResponse
                                   {
                                       AMCId = s.AMCId,
                                       Comments = s.Comments,
                                       Id = s.Id,
                                       Index = s.Index,
                                       IsActive = s.IsActive,
                                       IsCompleted = s.IsCompleted,
                                       IsDeleted = s.IsDeleted,
                                       PayAmt = s.PayAmt,
                                       CreatedBy = s.CreatedBy,
                                       CreatedOn = s.CreatedOn,
                                       PayAmtCurrencyId = s.PayAmtCurrencyId,
                                       PaymentTypeId = s.PaymentTypeId,
                                       Stage = s.Stage,
                                       StageName = li.ItemName,
                                       StageIndex = s.StageIndex,
                                       UpdatedBy = s.UpdatedBy,
                                       UpdatedOn = s.UpdatedOn,
                                       UserId = s.UserId
                                   }).ToListAsync();
            return amcStages;
        }
        public async Task<AMCStages> GetByIdAsync(Guid requestId)
           => await context.AMCStages.FirstOrDefaultAsync(x => x.Id == requestId);

        public async Task<Guid> UpdateAsync(AMCStages amcStages)
        {
            amcStages.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            amcStages.UpdatedOn = DateTime.Now;

            context.Entry(amcStages).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return amcStages.Id;
        }
    }
}