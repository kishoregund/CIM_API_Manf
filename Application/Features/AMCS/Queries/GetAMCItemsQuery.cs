using Application.Exceptions;
using Application.Features.AMCS.Responses;

namespace Application.Features.AMCS.Queries
{
    public class GetAmcItemsQuery : IRequest<IResponseWrapper>
    {
        public Guid AmcId { get; set; } 
    }

    public class GetAmcItemsQueryHandler(IAmcItemsService amcItemsService)
        : IRequestHandler<GetAmcItemsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAmcItemsQuery request, CancellationToken cancellationToken)
        {
            var amcItemsInDb = await amcItemsService.GetByAmcIdAsync(request.AmcId);

            if (amcItemsInDb is not null)
            {
                return await ResponseWrapper<List<AmcItemsResponse>>.SuccessAsync(data: amcItemsInDb);
            }
            return await ResponseWrapper<AmcItemsResponse>.SuccessAsync(message: "AMC Items does not exists.");
        }
    }
}