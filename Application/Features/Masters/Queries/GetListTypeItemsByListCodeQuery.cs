using Application.Features.Masters.Responses;
using Domain.Views;

namespace Application.Features.Masters.Queries
{
    public class GetListTypeItemsByListCodeQuery : IRequest<IResponseWrapper>
    {
        public string ListCode { get; set; }
    }

    public class GetListTypeItemsByListCodeQueryHandler(IListTypeItemsService ListTypeItemsService) : IRequestHandler<GetListTypeItemsByListCodeQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetListTypeItemsByListCodeQuery request, CancellationToken cancellationToken)
        {
            var ListTypeItemsInDb = (await ListTypeItemsService.GetListTypeItemByListCodeAsync(request.ListCode));//.Adapt<VW_ListTypeItemsResponse>();

            if (ListTypeItemsInDb is not null)
            {
                return await ResponseWrapper<List<VW_ListItems>>.SuccessAsync(data: ListTypeItemsInDb);
            }
            return await ResponseWrapper<List<VW_ListItems>>.SuccessAsync(message: "ListType Items does not exists.");
        }
    }
}
