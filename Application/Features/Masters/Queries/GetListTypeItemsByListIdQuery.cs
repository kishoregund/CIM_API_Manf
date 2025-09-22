using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Queries
{
    public class GetListTypeItemsByListIdQuery : IRequest<IResponseWrapper>
    {
        public Guid LisTypeId { get; set; }
    }

    public class GetListTypeItemsByListIdQueryHandler(IListTypeItemsService ListTypeItemService) : IRequestHandler<GetListTypeItemsByListIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetListTypeItemsByListIdQuery request, CancellationToken cancellationToken)
        {
            var ListTypeItemsInDb = await ListTypeItemService.GetListTypeItemsByListIdAsync(request.LisTypeId);

            if (ListTypeItemsInDb.Count > 0)
            {
                return await ResponseWrapper<List<ListTypeItemsResponse>>.SuccessAsync(data: ListTypeItemsInDb.Adapt<List<ListTypeItemsResponse>>());
            }
            return await ResponseWrapper<List<ListTypeItemsResponse>>.SuccessAsync(message: "No ListTypeItems were found.");
        }
    }
}
