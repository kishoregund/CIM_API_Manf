using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Queries
{
    public class GetListTypesQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetListTypesQueryHandler(IListTypeItemsService listTypeItemsService) : IRequestHandler<GetListTypesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetListTypesQuery request, CancellationToken cancellationToken)
        {
            var ListTypesInDb = await listTypeItemsService.GetListTypesAsync();

            if (ListTypesInDb.Count > 0)
            {
                return await ResponseWrapper<List<ListType>>.SuccessAsync(data: ListTypesInDb.Adapt<List<ListType>>());
            }
            return await ResponseWrapper<List<ListType>>.SuccessAsync(message: "No List Types were found.");
        }
    }
}
