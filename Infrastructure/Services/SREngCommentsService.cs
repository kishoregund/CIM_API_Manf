using Application.Features.ServiceRequests;
using Application.Features.Identity.Users;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.ServiceRequests.Responses;
using System.Security.Cryptography.Xml;

namespace Infrastructure.Services
{
    public class SREngCommentsService(ApplicationDbContext context, ICurrentUserService currentUserService) : ISREngCommentsService
    {
        public async Task<SREngComments> GetSREngCommentAsync(Guid id) 
            => await context.SREngComments.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<SREngComments>> GetSREngCommentEntityBySRIdAsync(Guid serviceRequestId)
            => await context.SREngComments.Where(p => p.ServiceRequestId == serviceRequestId).ToListAsync();

        public async Task<List<SREngCommentsResponse>> GetSREngCommentBySRIdAsync(Guid serviceRequestId)
        {
         var engComments =  await context.SREngComments.Where(p => p.ServiceRequestId == serviceRequestId).ToListAsync();
            List<SREngCommentsResponse> srEngCommentsResponses = new(); 
            foreach (SREngComments engCom in engComments)
            {
                var eng = context.RegionContact.FirstOrDefault(x => x.Id == engCom.EngineerId);
                var mSREngComment = new SREngCommentsResponse();
                mSREngComment.Id = engCom.Id;
                mSREngComment.Comments = engCom.Comments;
                mSREngComment.IsActive = engCom.IsActive;
                mSREngComment.EngineerId = engCom.EngineerId;
                mSREngComment.EngineerName = eng.FirstName + " " + eng.LastName;
                mSREngComment.NextDate = engCom.Nextdate.Value.Date;
                mSREngComment.ServiceRequestId = engCom.ServiceRequestId;

                srEngCommentsResponses.Add(mSREngComment);
            }
            return srEngCommentsResponses;
        }

        public async Task<Guid> CreateSREngCommentAsync(SREngComments SREngComment)
        {
            SREngComment.CreatedOn = DateTime.Now;
            SREngComment.UpdatedOn = DateTime.Now;
            SREngComment.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            SREngComment.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await context.SREngComments.AddAsync(SREngComment);
            await context.SaveChangesAsync();
            return SREngComment.Id;
        }

        public async Task<bool> DeleteSREngCommentAsync(Guid id)
        {

            var deletedEngAction = await context
                .SREngComments.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            context.Entry(deletedEngAction).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateSREngCommentAsync(SREngComments SREngComment)
        {
            SREngComment.UpdatedOn = DateTime.Now;
            SREngComment.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(SREngComment).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return SREngComment.Id;
        }


    }
}