using Application.Features.Masters;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using Domain.Views;
using Application.Features.Masters.Responses;

namespace Infrastructure.Services
{
    public class ListTypeItemsService(ApplicationDbContext Context, ICurrentUserService currentUserService) : IListTypeItemsService
    {

        public async Task<ListTypeItems> GetListTypeItemAsync(Guid id)
             => await Context.ListTypeItems.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<ListTypeItems>> GetListTypeItemsByListIdAsync(Guid listTypeId)
         => await Context.ListTypeItems.Where(p => p.ListTypeId == listTypeId).OrderBy(x=>x.ItemName).ToListAsync();

        public async Task<List<VW_ListItems>> GetListTypeItemByListCodeAsync(string listCode)
            => await Context.VW_ListItems.Where(p => p.ListCode == listCode).OrderBy(x => x.ItemName).ToListAsync();

        public async Task<List<VW_ListItems>> GetVWListTypeItemByListIdAsync(Guid listId)
            => await Context.VW_ListItems.Where(p => p.ListTypeId == listId).OrderBy(x => x.ItemName).ToListAsync();

        public async Task<Guid> CreateListTypeItemsAsync(ListTypeItems listTypeItems)
        {
            if (!Context.VW_ListItems.Any(x => x.ItemName == listTypeItems.ItemName && x.ListTypeId == listTypeItems.ListTypeId))
            {
                listTypeItems.CreatedBy = Guid.Parse(currentUserService.GetUserId());
                listTypeItems.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

                await Context.ListTypeItems.AddAsync(listTypeItems);
                await Context.SaveChangesAsync();
            }
            return listTypeItems.Id;
        }

        public async Task<bool> DeleteListTypeItemsAsync(Guid id)
        {
            var cVal = await Context.ConfigTypeValues.Where(x => x.ListTypeItemId == id).ToListAsync();

            foreach (var li in cVal)
            {
                Context.Entry(li).State = EntityState.Deleted;
            }

            var listtypeitem = await Context.ListTypeItems.FirstOrDefaultAsync(x => x.Id == id);

            Context.Entry(listtypeitem).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateListTypeItemsAsync(ListTypeItems listTypeItems)
        {
            listTypeItems.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            Context.Entry(listTypeItems).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return listTypeItems.Id;
        }

        public async Task<List<ListType>> GetListTypesAsync()
        {
            var listTypes = await Context.ListTypes.ToListAsync();
            return listTypes.OrderBy(x => x.ListName).ToList();
        }

        public async Task<ListType> GetListTypeByIdAsync(Guid listTypeId)
            => await Context.ListTypes.Where(x => x.Id == listTypeId).FirstOrDefaultAsync();

        public async Task<List<ScreensForRolePermissionsResponse>> GetScreensForRolePermissions()
        {
            var lstProfile = new List<ScreensForRolePermissionsResponse>();
            var lstType = await Context.VW_ListItems.Where(x => x.ListCode == "SCRNS").OrderBy(x => x.ItemName).ToListAsync();

            foreach (var profile in lstType)
            {
                var allScreens = new ScreensForRolePermissionsResponse
                {
                    ItemName = profile.ItemName,
                    ListName = profile.ListName,
                    ListCode = profile.ListCode,
                    ListTypeId = profile.ListTypeId,
                    ListTypeItemId = profile.ListTypeItemId,
                    //Category = getCategory(profile.ItemCode),
                    IsDeleted = profile.IsDeleted,
                    ItemCode = profile.ItemCode,
                };

                lstProfile.Add(getCategory(allScreens));
            }

            return lstProfile;
        }

        private ScreensForRolePermissionsResponse getCategory(ScreensForRolePermissionsResponse screen)
        {
            VW_ListItems lItem = new();
            if (screen.ItemCode == "PROF" || screen.ItemCode == "URPRF")
            {
                lItem = Context.VW_ListItems.FirstOrDefault(x => x.ListCode == "PRGRP" && x.ItemCode == "ADMIN");
                screen.Category = lItem.ListTypeItemId;
                screen.CategoryName = lItem.ItemName;

                //Context.VW_ListItems.FirstOrDefault(x => x.ListCode == "PRGRP" && x.ItemCode == "ADMIN").ListTypeItemId;
            }
            else if (screen.ItemCode == "SCURR" || screen.ItemCode == "SCOUN" || screen.ItemCode == "SCUST" || screen.ItemCode == "SDIST" || screen.ItemCode == "SINST" || screen.ItemCode == "SSPAR")
            {
                lItem = Context.VW_ListItems.FirstOrDefault(x => x.ListCode == "PRGRP" && x.ItemCode == "MSTRS");
                screen.Category = lItem.ListTypeItemId;
                screen.CategoryName = lItem.ItemName;
            }

            else if (screen.ItemCode == "AUDIT" || screen.ItemCode == "SIMXP" || screen.ItemCode == "SSRCH" || screen.ItemCode == "PSRRP" || screen.ItemCode == "CUSDH" || screen.ItemCode == "DHSET" || screen.ItemCode == "DISDH")
            {
                lItem = Context.VW_ListItems.FirstOrDefault(x => x.ListCode == "PRGRP" && x.ItemCode == "UTILS");
                screen.Category = lItem.ListTypeItemId;
                screen.CategoryName = lItem.ItemName;
            }

            else if (screen.ItemCode == "SAMC" || screen.ItemCode == "CTSPI" || screen.ItemCode == "OFREQ" || screen.ItemCode == "SCDLE" || screen.ItemCode == "SRREQ" || screen.ItemCode == "SPRCM" || screen.ItemCode == "SRREP" || screen.ItemCode == "TREXP" || screen.ItemCode == "TRINV" || screen.ItemCode == "ADREQ" || screen.ItemCode == "CTSS")
            {
                lItem = Context.VW_ListItems.FirstOrDefault(x => x.ListCode == "PRGRP" && x.ItemCode == "TRANS");
                screen.Category = lItem.ListTypeItemId;
                screen.CategoryName = lItem.ItemName;
            }

            else if (screen.ItemCode == "SRQRP" || screen.ItemCode == "SRCMR" || screen.ItemCode == "PDQRQ" || screen.ItemCode == "SRCRR")
            {
                lItem = Context.VW_ListItems.FirstOrDefault(x => x.ListCode == "PRGRP" && x.ItemCode == "REPTS");
                screen.Category = lItem.ListTypeItemId;
                screen.CategoryName = lItem.ItemName;
            }

            return screen;
        }


    }
}