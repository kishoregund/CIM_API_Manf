using Application.Features.AppBasic.Responses;

namespace Application.Features.AppBasic.Queries
{
    public class GetBusinessUnitsQuery : IRequest<IResponseWrapper>
    {
        
    }

    public class GetBusinessUnitByCompanyIdQueryHandler(IBusinessUnitService businessUnitService) : IRequestHandler<GetBusinessUnitsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetBusinessUnitsQuery request, CancellationToken cancellationToken)
        {
            var businessUnitsInDb = await businessUnitService.GetBusinessUnitsAsync();

            if (businessUnitsInDb.Count > 0)
            {
                return await ResponseWrapper<List<BusinessUnitResponse>>.SuccessAsync(data: businessUnitsInDb.Adapt<List<BusinessUnitResponse>>());
            }
            return await ResponseWrapper<List<BusinessUnitResponse>>.SuccessAsync(message: "No BusinessUnits were found.");
        }
    }
}