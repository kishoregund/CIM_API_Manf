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

            try
            {
                // Validation: Check for required fields
                if (EngScheduler.EngId == Guid.Empty)
                    throw new ArgumentException("Engineer ID (EngId) is required.", nameof(EngScheduler.EngId));

                if (string.IsNullOrWhiteSpace(EngScheduler.StartTime))
                    throw new ArgumentException("Start time is required.", nameof(EngScheduler.StartTime));

                if (string.IsNullOrWhiteSpace(EngScheduler.EndTime))
                    throw new ArgumentException("End time is required.", nameof(EngScheduler.EndTime));

                // Validation: Check for duplicate schedule (same engineer, same start time, same end time)
                var existingSchedule = await context.EngScheduler
                    .Where(x => !x.IsDeleted &&
                               x.EngId == EngScheduler.EngId &&
                               x.StartTime == EngScheduler.StartTime &&
                               x.EndTime == EngScheduler.EndTime)
                    .FirstOrDefaultAsync();

                if (existingSchedule != null)
                    throw new InvalidOperationException(
                        $"A schedule already exists for engineer {EngScheduler.EngId} with the same start time ({EngScheduler.StartTime}) and end time ({EngScheduler.EndTime}).");

                // Validation: Check for overlapping schedules (same engineer, overlapping time slots)
                var overlappingSchedule = await context.EngScheduler
                    .Where(x => !x.IsDeleted &&
                               x.EngId == EngScheduler.EngId &&
                               x.StartTime.CompareTo(EngScheduler.EndTime) < 0 &&
                               x.EndTime.CompareTo(EngScheduler.StartTime) > 0)
                    .FirstOrDefaultAsync();

                if (overlappingSchedule != null)
                    throw new InvalidOperationException(
                        $"An overlapping schedule exists for engineer {EngScheduler.EngId}. " +
                        $"Existing schedule: {overlappingSchedule.StartTime} to {overlappingSchedule.EndTime}. " +
                        $"New schedule: {EngScheduler.StartTime} to {EngScheduler.EndTime}.");

                EngScheduler.CreatedOn = DateTime.Now;
                EngScheduler.UpdatedOn = DateTime.Now;
                EngScheduler.CreatedBy = Guid.Parse(currentUserService.GetUserId());
                EngScheduler.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

                await context.EngScheduler.AddAsync(EngScheduler);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
            }
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