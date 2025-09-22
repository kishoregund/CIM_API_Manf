using Application.Features.Masters.Responses;
using Domain.Views;

namespace Application.Features.Masters.Queries
{
    public class GetVWListTypeItemsByListIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ListId { get; set; }
    }

    public class GetVWListTypeItemsByListIdQueryHandler(IListTypeItemsService ListTypeItemsService) : IRequestHandler<GetVWListTypeItemsByListIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetVWListTypeItemsByListIdQuery request, CancellationToken cancellationToken)
        {
            var ListTypeItemsInDb = (await ListTypeItemsService.GetVWListTypeItemByListIdAsync(request.ListId));//.Adapt<VW_ListTypeItemsResponse>();

            if (ListTypeItemsInDb is not null)
            {
                return await ResponseWrapper<List<VW_ListItems>>.SuccessAsync(data: ListTypeItemsInDb);
            }
            return await ResponseWrapper<List<VW_ListItems>>.SuccessAsync(message: "ListType Items does not exists.");
        }
    }
}
