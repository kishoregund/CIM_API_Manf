using Application.Features.Masters.Responses;
using Domain.Views;

namespace Application.Features.Masters
{
    public interface IListTypeItemsService
    {
        Task<ListTypeItems> GetListTypeItemAsync(Guid id);
        Task<ListType> GetListTypeByIdAsync(Guid id);
        Task<List<ListTypeItems>> GetListTypeItemsByListIdAsync(Guid listId);
        Task<List<VW_ListItems>> GetListTypeItemByListCodeAsync(string listCode);
        Task<List<VW_ListItems>> GetVWListTypeItemByListIdAsync(Guid listId);        
        Task<Guid> CreateListTypeItemsAsync(ListTypeItems listTypeItems);
        Task<Guid> UpdateListTypeItemsAsync(ListTypeItems listTypeItems);
        Task<bool> DeleteListTypeItemsAsync(Guid id);
        Task<List<ListType>> GetListTypesAsync();
        Task<List<ScreensForRolePermissionsResponse>> GetScreensForRolePermissions();
    }
}
