using Application.Features.ServiceRequests;
using Domain.Entities;
using Application.Features.Identity.Users;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.ServiceRequests.Responses;

namespace Infrastructure.Services
{
    public class SREngActionService(ApplicationDbContext context, ICurrentUserService currentUserService) : ISREngActionService
    {

        public async Task<SREngAction> GetSREngActionAsync(Guid id)
            => await context.SREngAction.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        public async Task<List<SREngAction>> GetSREngActionEntityBySRIdAsync(Guid serviceRequestId)
        => await context.SREngAction.Where(p => p.ServiceRequestId == serviceRequestId && !p.IsDeleted).ToListAsync();

        public async Task<List<SREngActionResponse>> GetSREngActionBySRIdAsync(Guid serviceRequestId)
        { 
           var actions = await context.SREngAction.Where(p => p.ServiceRequestId == serviceRequestId).ToListAsync();
            List<SREngActionResponse> srEngActionResponses = new();
            foreach (SREngAction engAct in actions)
            {
                var mSREngineerAction = new SREngActionResponse();
                var eng = context.RegionContact.FirstOrDefault(x => x.Id == engAct.EngineerId);           
                mSREngineerAction.Id = engAct.Id;
                mSREngineerAction.Comments = engAct.Comments;
                mSREngineerAction.IsActive = engAct.IsActive;
                mSREngineerAction.ActionDate = engAct.ActionDate;
                mSREngineerAction.Actiontaken = engAct.Actiontaken;
                mSREngineerAction.ActiontakenName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == engAct.Actiontaken)?.ItemName;
                mSREngineerAction.EngineerId = engAct.EngineerId;
                mSREngineerAction.EngineerName = eng.FirstName + " " + eng.LastName;
                mSREngineerAction.TeamviewRecording = engAct.TeamviewRecording;
                mSREngineerAction.ServiceRequestId = engAct.ServiceRequestId;

                srEngActionResponses.Add(mSREngineerAction);
            }
            return srEngActionResponses.ToList();
        }

        public async Task<Guid> CreateSREngActionAsync(SREngAction EngAction)
        {
            EngAction.CreatedOn = DateTime.Now;
            EngAction.UpdatedOn = DateTime.Now;
            EngAction.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            EngAction.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await context.SREngAction.AddAsync(EngAction);
            await context.SaveChangesAsync();
            return EngAction.Id;
        }

        public async Task<bool> DeleteSREngActionAsync(Guid id)
        {

            var deletedEngAction = await context
                .SREngAction.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            context.Entry(deletedEngAction).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateSREngActionAsync(SREngAction EngAction)
        {
            EngAction.UpdatedOn = DateTime.Now;
            EngAction.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(EngAction).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return EngAction.Id;
        }
    }
}