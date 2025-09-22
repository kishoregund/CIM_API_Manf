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

namespace Infrastructure.Services
{
#pragma warning disable CS9113 // Parameter is unread.
    public class SRAuditTrailService(ApplicationDbContext context, ICurrentUserService currentUserService) : ISRAuditTrailService
#pragma warning restore CS9113 // Parameter is unread.
    {
        public async Task<SRAuditTrail> GetSRAuditTrailAsync(Guid id)
            => await context.SRAuditTrail.FirstOrDefaultAsync(p => p.Id == id);



        public async Task<List<SRAuditTrail>> GetSRAuditTrailBySRIdAsync(Guid serviceRequestId)
        => await context.SRAuditTrail.Where(p => p.ServiceRequestId == serviceRequestId).ToListAsync();

        public async Task<Guid> CreateSRAuditTrailAsync(SRAuditTrail SRAuditTrail)
        {
            await context.SRAuditTrail.AddAsync(SRAuditTrail);
            await context.SaveChangesAsync();
            return SRAuditTrail.Id;
        }

        public async Task<bool> DeleteSRAuditTrailAsync(Guid id)
        {

            var deletedEngAction = await context
                .SRAuditTrail.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            context.Entry(deletedEngAction).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateSRAuditTrailAsync(SRAuditTrail SRAuditTrail)
        {
            context.Entry(SRAuditTrail).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return SRAuditTrail.Id;
        }
     
    }
}