using Application.Features.AMCS;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using Application.Features.AMCS.Responses;
using Mapster;

namespace Infrastructure.Services
{
    public class AmcItemsService(ApplicationDbContext context, ICurrentUserService currentUserService) : IAmcItemsService
    {
        public async Task<Guid> CreateAmcItems(AMCItems items)
        {
            items.IsActive = true;
            items.IsDeleted = false;

            items.UpdatedOn = DateTime.Now;
            items.CreatedOn = DateTime.Now;

            items.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            items.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await context.AMCItems.AddAsync(items);
            await context.SaveChangesAsync();

            return items.Id;
        }

        public async Task<bool> DeleteAmcItems(Guid id)
        {
            var amcItems = await context.AMCItems.FirstOrDefaultAsync(x => x.Id == id);
            if (amcItems is null)
                return false;

            //amcItems.IsDeleted = true;

            context.Entry(amcItems).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return false;
        }

        public async Task<List<AmcItemsResponse>> GetByAmcIdAsync(Guid requestAmcId)
        {
            var amcItems = (await context.AMCItems.Where(x => x.AMCId == requestAmcId).ToListAsync()).Adapt<List<AmcItemsResponse>>();
            foreach (AmcItemsResponse item in amcItems)
            {
                if (!string.IsNullOrEmpty(item.ServiceRequestId))
                    item.SerReqNo = context.ServiceRequest.FirstOrDefault(x => x.Id.ToString() == item.ServiceRequestId).SerReqNo;

                if (!string.IsNullOrEmpty(item.Status))
                    item.StatusName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == item.Status).ItemName;
            }
            return amcItems;
        }

        public async Task<AMCItems> GetByIdAsync(Guid requestId)
           => await context.AMCItems.FirstOrDefaultAsync(x => x.Id == requestId);


        public async Task<Guid> UpdateAsync(AMCItems amcItems)
        {

            amcItems.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            amcItems.UpdatedOn = DateTime.Now;

            context.Entry(amcItems).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return amcItems.Id;
        }
    }
}