using Application.Features.AppBasic.Responses;

namespace Application.Features.AppBasic.Queries
{
    public class GetBUsByDistributorQuery : IRequest<IResponseWrapper>
    {
        public Guid DistributorId { get; set; }
    }
    public class GetBUsByDistributorQueryHandler(IBusinessUnitService businessUnit) : IRequestHandler<GetBUsByDistributorQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetBUsByDistributorQuery request, CancellationToken cancellationToken)
        {
            var brandInDb = (await businessUnit.GetBusinessUnitsByDistributorAsync(request.DistributorId)).Adapt<List<BusinessUnitResponse>>();

            if (brandInDb is not null)
            {
                return await ResponseWrapper<List<BusinessUnitResponse>>.SuccessAsync(data: brandInDb);
            }
            return await ResponseWrapper<BusinessUnitResponse>.SuccessAsync(message: "Brand does not exists.");
        }
    }
}