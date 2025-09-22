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
    public class EngSchedulerService(ApplicationDbContext context, ICurrentUserService currentUserService) : IEngSchedulerService
    {

        public Task<EngScheduler> GetEngSchedulerAsync(Guid id)
            => context.EngScheduler.FirstOrDefaultAsync(p => p.Id ==  id);

        public async Task<List<EngScheduler>> GetEngSchedulerEntityBySRIdAsync(Guid ServiceRequestId)
             => await context
               .EngScheduler
               .Where(s => s.SerReqId == ServiceRequestId)
               .ToListAsync();

        public async Task<List<EngSchedulerResponse>> GetEngSchedulerBySRIdAsync(Guid ServiceRequestId)
        {
            var engSchedules = await (from a in context.EngScheduler.Where(x => !x.IsDeleted)
                                      join b in context.ServiceRequest.Where(x => !x.IsDeleted) on a.SerReqId equals b.Id
                                      where b.Id == ServiceRequestId
                                      select a).ToListAsync();

            List<EngSchedulerResponse> engSchedulerResponses = new();
            foreach (EngScheduler eScheduler in engSchedules)
            {
                var eng = context.RegionContact.FirstOrDefault(x => x.Id == eScheduler.EngId);
                var mEngScheduler = new EngSchedulerResponse
                {
                    Id = eScheduler.Id,
                    CreatedOn = eScheduler.CreatedOn,
                    Subject = eScheduler.Subject,
                    DisplayName = eScheduler.Subject,
                    StartTime = eScheduler.StartTime,
                    EndTime = eScheduler.EndTime,
                    IsAllDay = eScheduler.IsAllDay,
                    IsBlock = eScheduler.IsBlock,
                    IsReadOnly = eScheduler.IsReadOnly,
                    RoomId = eScheduler.RoomId,
                    ResourceId = eScheduler.ResourceId,
                    SerReqId = eScheduler.SerReqId,
                    ActionId = eScheduler.ActionId,
                    Location = eScheduler.Location,
                    EngId = eScheduler.EngId,
                    EngineerName = eng?.FirstName + " " + eng?.LastName,
                    Description = eScheduler.Desc,
                    RecurrenceException = eScheduler.RecurrenceException,
                    RecurrenceRule = eScheduler.RecurrenceRule,
                    StartTimezone = eScheduler.StartTimezone,
                    EndTimezone = eScheduler.EndTimezone
                };
                engSchedulerResponses.Add(mEngScheduler);
            }
            return engSchedulerResponses;
        }

        public async Task<List<EngSchedulerResponse>> GetEngSchedulerByEngineerAsync(Guid engineerId)
        {
            var engSchedules = await (from a in context.EngScheduler.Where(x => !x.IsDeleted)
                                      join b in context.ServiceRequest.Where(x => !x.IsDeleted) on a.SerReqId equals b.Id
                                      where a.EngId == engineerId
                                      select a).ToListAsync();

            List<EngSchedulerResponse> engSchedulerResponses = new();
            foreach (EngScheduler eScheduler in engSchedules)
            {
                var eng = context.RegionContact.FirstOrDefault(x => x.Id == eScheduler.EngId);
                var mEngScheduler = new EngSchedulerResponse
                {
                    Id = eScheduler.Id,
                    CreatedOn = eScheduler.CreatedOn,
                    Subject = eScheduler.Subject,
                    DisplayName = eScheduler.Subject,
                    StartTime = eScheduler.StartTime,
                    EndTime = eScheduler.EndTime,
                    IsAllDay = eScheduler.IsAllDay,
                    IsBlock = eScheduler.IsBlock,
                    IsReadOnly = eScheduler.IsReadOnly,
                    RoomId = eScheduler.RoomId,
                    ResourceId = eScheduler.ResourceId,
                    SerReqId = eScheduler.SerReqId,
                    ActionId = eScheduler.ActionId,
                    Location = eScheduler.Location,
                    EngId = eScheduler.EngId,
                    EngineerName = eng?.FirstName + " " + eng?.LastName,
                    Description = eScheduler.Desc,
                    RecurrenceException = eScheduler.RecurrenceException,
                    RecurrenceRule = eScheduler.RecurrenceRule,
                    StartTimezone = eScheduler.StartTimezone,
                    EndTimezone = eScheduler.EndTimezone
                };
                engSchedulerResponses.Add(mEngScheduler);
            }
            return engSchedulerResponses;
        }

        public async Task<Guid> CreateEngSchedulerAsync(EngScheduler EngScheduler)
        {
            EngScheduler.CreatedOn = DateTime.Now;
            EngScheduler.UpdatedOn = DateTime.Now;
            EngScheduler.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            EngScheduler.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await context.EngScheduler.AddAsync(EngScheduler);
            await context.SaveChangesAsync();
            return EngScheduler.Id;
        }

        public async Task<bool> DeleteEngSchedulerAsync(Guid id)
        {
            var deletedEngAction = await context
                .EngScheduler.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            context.Entry(deletedEngAction).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateEngSchedulerAsync(EngScheduler EngScheduler)
        {
            EngScheduler.UpdatedOn = DateTime.Now;
            EngScheduler.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(EngScheduler).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return EngScheduler.Id;
        }
    }
}