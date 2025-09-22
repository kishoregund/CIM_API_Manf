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
using Application.Features.ServiceReports.Responses;

namespace Infrastructure.Services
{
#pragma warning disable CS9113 // Parameter is unread.
    public class PastServiceReportService(ApplicationDbContext Context, ICurrentUserService currentUserService) : IPastServiceReportService
#pragma warning restore CS9113 // Parameter is unread.
    {

        public async Task<PastServiceReport> GetPastServiceReportAsync(Guid id)
            => await Context.PastServiceReport.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<PastServiceReportResponse>> GetPastServiceReportsAsync()
        {
            var reports =await  (from p in Context.PastServiceReport
                           join c in Context.Customer on p.CustomerId equals c.Id
                           join s in Context.Site on p.SiteId equals s.Id
                           join i in Context.Instrument on p.InstrumentId equals i.Id
                           join b in Context.Brand on p.BrandId equals b.Id
                           select new PastServiceReportResponse
                           {
                               BrandId = p.BrandId,
                               SiteId = p.SiteId,
                               Id = p.Id,
                               CreatedBy = p.CreatedBy,
                               CreatedOn = p.CreatedOn,
                               CustomerId = p.CustomerId,
                               InstrumentId = p.InstrumentId,
                               Of = p.Of,
                               IsActive = p.IsActive,
                               CustomerName = c.CustName,
                               Instrument = (Context.VW_ListItems.Where(x => x.ListTypeItemId.ToString() == i.InsType).FirstOrDefault().ItemName) + " - " + i.SerialNos,
                               SiteName = s.CustRegName,
                               BrandName = b.BrandName,
                               FileId = Context.FileShare.Where(x=>x.ParentId == p.Id && x.FileFor == "PSTSRP").FirstOrDefault().Id
                           }).ToListAsync();

            return reports;
        }

        public async Task<Guid> CreatePastServiceReportAsync(PastServiceReport PastServiceReport)
        {
            PastServiceReport.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            PastServiceReport.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await Context.PastServiceReport.AddAsync(PastServiceReport);
            await Context.SaveChangesAsync();
            return PastServiceReport.Id;
        }

        public async Task<bool> DeletePastServiceReportAsync(Guid id)
        {

            var deletedEngAction = await Context.PastServiceReport.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            //deletedEngAction.IsDeleted = true;
            //deletedEngAction.IsActive = false;

            Context.Entry(deletedEngAction).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdatePastServiceReportAsync(PastServiceReport PastServiceReport)
        {
            PastServiceReport.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            PastServiceReport.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            Context.Entry(PastServiceReport).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return PastServiceReport.Id;
        }
    }
}
