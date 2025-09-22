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
    public class SRPEngWorkDoneService(ApplicationDbContext Context, ICurrentUserService currentUserService) : ISRPEngWorkDoneService
    {

        public async Task<SRPEngWorkDone> GetSRPEngWorkDoneAsync(Guid id)
             => await Context.SRPEngWorkDone.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<SRPEngWorkDone>> GetSRPEngWorkDoneBySRPIdAsync(Guid serviceReportId)
          => await Context.SRPEngWorkDone.Where(p => p.ServiceReportId == serviceReportId).ToListAsync();

        public async Task<Guid> CreateSRPEngWorkDoneAsync(SRPEngWorkDone SRPEngWorkDone)
        {
            SRPEngWorkDone.CreatedOn = DateTime.Now;
            SRPEngWorkDone.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            SRPEngWorkDone.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SRPEngWorkDone.UpdatedOn = DateTime.Now;

            await Context.SRPEngWorkDone.AddAsync(SRPEngWorkDone);
            await Context.SaveChangesAsync();
            return SRPEngWorkDone.Id;
        }

        public async Task<bool> DeleteSRPEngWorkDoneAsync(Guid id)
        {

            var deletedEngAction = await Context
                .SRPEngWorkDone.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            Context.Entry(deletedEngAction).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateSRPEngWorkDoneAsync(SRPEngWorkDone SRPEngWorkDone)
        {
            SRPEngWorkDone.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SRPEngWorkDone.UpdatedOn = DateTime.Now;

            Context.Entry(SRPEngWorkDone).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return SRPEngWorkDone.Id;
        }


    }
}
