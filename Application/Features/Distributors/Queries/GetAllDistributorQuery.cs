using Application.Features.Distributors.Responses;

namespace Application.Features.Distributors.Queries
{
    public class GetDistributorsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetDistributorsQueryHandler(IDistributorService distributorService) : IRequestHandler<GetDistributorsQuery, IResponseWrapper>
    {

        public async Task<IResponseWrapper> Handle(GetDistributorsQuery request, CancellationToken cancellationToken)
        {
            var distributorsInDb = await distributorService.GetDistributorsAsync();

            if (distributorsInDb.Count > 0)
            {
                return await ResponseWrapper<List<DistributorResponse>>.SuccessAsync(data: distributorsInDb.Adapt<List<DistributorResponse>>());
            }
            return await ResponseWrapper<List<DistributorResponse>>.SuccessAsync(message: "No Distributors were found.");
        }
    }
}
