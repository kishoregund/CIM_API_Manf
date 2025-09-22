using Application.Features.Identity.Users;
using Application.Features.ServiceReports;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SRPEngWorkTimeService(ApplicationDbContext Context, ICurrentUserService currentUserService) : ISRPEngWorkTimeService
    {

        public async Task<SRPEngWorkTime> GetSRPEngWorkTimeAsync(Guid id)
            => await Context.SRPEngWorkTime.FirstOrDefaultAsync(p => p.Id == id);



        public async Task<List<SRPEngWorkTime>> GetSRPEngWorkTimeBySRPIdAsync(Guid serviceReportId)
       => await Context.SRPEngWorkTime.Where(p => p.ServiceReportId == serviceReportId).ToListAsync();

        public async Task<Guid> CreateSRPEngWorkTimeAsync(SRPEngWorkTime SRPEngWorkTime)
        {
            SRPEngWorkTime.CreatedOn = DateTime.Now;
            SRPEngWorkTime.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            SRPEngWorkTime.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SRPEngWorkTime.UpdatedOn = DateTime.Now;

            await Context.SRPEngWorkTime.AddAsync(SRPEngWorkTime);
            await Context.SaveChangesAsync();
            return SRPEngWorkTime.Id;
        }

        public async Task<bool> DeleteSRPEngWorkTimeAsync(Guid id)
        {

            var deletedEngAction = await Context
                .SRPEngWorkTime.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            Context.Entry(deletedEngAction).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateSRPEngWorkTimeAsync(SRPEngWorkTime SRPEngWorkTime)
        {
            SRPEngWorkTime.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SRPEngWorkTime.UpdatedOn = DateTime.Now;

            Context.Entry(SRPEngWorkTime).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return SRPEngWorkTime.Id;
        }

    }
}
