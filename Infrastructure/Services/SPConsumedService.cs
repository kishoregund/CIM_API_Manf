using Application.Features.ServiceReports;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SPConsumedService(ApplicationDbContext Context, ICurrentUserService currentUserService) : ISPConsumedService
    {

        public async Task<SPConsumed> GetSPConsumedAsync(Guid id)
            => await Context.SPConsumed.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<SPConsumed>> GetSPConsumedBySRPIdAsync(Guid serviceReportId)
       => await Context.SPConsumed.Where(x=>x.ServiceReportId == serviceReportId).ToListAsync();

        public async Task<Guid> CreateSPConsumedAsync(SPConsumed SPConsumed)
        {
            SPConsumed.CreatedOn = DateTime.Now;
            SPConsumed.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            SPConsumed.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SPConsumed.UpdatedOn = DateTime.Now;

            await Context.SPConsumed.AddAsync(SPConsumed);
            await Context.SaveChangesAsync();
            return SPConsumed.Id;
        }

        public async Task<bool> DeleteSPConsumedAsync(Guid id)
        {

            var deletedEngAction = await Context
                .SPConsumed.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            Context.Entry(deletedEngAction).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateSPConsumedAsync(SPConsumed SPConsumed)
        {
            SPConsumed.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SPConsumed.UpdatedOn = DateTime.Now;

            Context.Entry(SPConsumed).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return SPConsumed.Id;
        }

    }
}
