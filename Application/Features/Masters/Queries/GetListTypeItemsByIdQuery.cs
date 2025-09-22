using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Queries
{
    public class GetListTypeItemsByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ListTypeItemsId { get; set; }
    }

    public class GetListTypeItemsByIdQueryHandler(IListTypeItemsService ListTypeItemsService) : IRequestHandler<GetListTypeItemsByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetListTypeItemsByIdQuery request, CancellationToken cancellationToken)
        {
            var ListTypeItemsInDb = (await ListTypeItemsService.GetListTypeItemAsync(request.ListTypeItemsId)).Adapt<ListTypeItemsResponse>();

            if (ListTypeItemsInDb is not null)
            {
                return await ResponseWrapper<ListTypeItemsResponse>.SuccessAsync(data: ListTypeItemsInDb);
            }
            return await ResponseWrapper<ListTypeItemsResponse>.SuccessAsync(message: "ListTypeItems does not exists.");
        }
    }
}
