using Application.Features.ServiceRequests;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.ServiceRequests.Responses;

namespace Infrastructure.Services
{
    public class SRAssignedHistoryService(ApplicationDbContext context, ICurrentUserService currentUserService) : ISRAssignedHistoryService
    {

        public Task<SRAssignedHistory> GetSRAssignedHistoryAsync(Guid id)
            => context.SRAssignedHistory.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<SRAssignedHistory>> GetSRAssignedHistoryEntityBySRIdAsync(Guid serviceRequestId)
            => await context.SRAssignedHistory.Where(x => x.ServiceRequestId == serviceRequestId).ToListAsync();

        public async Task<List<SRAssignedHistoryResponse>> GetSRAssignedHistoryBySRIdAsync(Guid serviceRequestId)
        {
            List<SRAssignedHistoryResponse> srAssignedHistoryResponses = new();
            var assignedHistorys = await context.SRAssignedHistory.Where(x => x.ServiceRequestId == serviceRequestId).OrderByDescending(x => x.AssignedDate).ToListAsync();
            foreach (SRAssignedHistory assignHistory in assignedHistorys)
            {
                var mSRAssignHistory = new SRAssignedHistoryResponse();
                mSRAssignHistory.Id = assignHistory.Id;
                mSRAssignHistory.Comments = assignHistory.Comments;
                mSRAssignHistory.IsActive = assignHistory.IsActive;
                mSRAssignHistory.AssignedDate = assignHistory.AssignedDate;
                mSRAssignHistory.EngineerId = assignHistory.EngineerId;

                if (assignHistory.EngineerId != Guid.Empty)
                {
                    var engineerConatct = context.RegionContact.FirstOrDefault(x => x.Id == assignHistory.EngineerId);
                    mSRAssignHistory.EngineerName = engineerConatct.FirstName + "" + engineerConatct.LastName;
                }

                if (assignHistory.TicketStatus != null && assignHistory.TicketStatus != "")
                {
                    mSRAssignHistory.TicketStatus = context.VW_ListItems.FirstOrDefault(x => x.ItemCode == assignHistory.TicketStatus)?.ItemName;
                }

                mSRAssignHistory.ServiceRequestId = assignHistory.ServiceRequestId;

                srAssignedHistoryResponses.Add(mSRAssignHistory);
            }
            return srAssignedHistoryResponses;
        }

        public async Task<Guid> CreateSRAssignedHistoryAsync(SRAssignedHistory SRAssignedHistory)
        {
            SRAssignedHistory.CreatedOn = DateTime.Now;
            SRAssignedHistory.UpdatedOn = DateTime.Now;
            SRAssignedHistory.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            SRAssignedHistory.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await context.SRAssignedHistory.AddAsync(SRAssignedHistory);
            await context.SaveChangesAsync();
            return SRAssignedHistory.Id;
        }

        public async Task<bool> DeleteSRAssignedHistoryAsync(Guid id)
        {
            var deletedEngAction = await context
                .SRAssignedHistory.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            context.Entry(deletedEngAction).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateSRAssignedHistoryAsync(SRAssignedHistory SRAssignedHistory)
        {
            SRAssignedHistory.UpdatedOn = DateTime.Now;
            SRAssignedHistory.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(SRAssignedHistory).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return SRAssignedHistory.Id;
        }
    }
}