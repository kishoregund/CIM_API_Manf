using Application.Features.AppBasic.Responses;

namespace Application.Features.AppBasic.Queries
{
    public class GetManfBusinessUnitsQuery : IRequest<IResponseWrapper>
    {
        
    }

    public class GetManfBusinessUnitByCompanyIdQueryHandler(IManfBusinessUnitService ManfBusinessUnitService) : IRequestHandler<GetManfBusinessUnitsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetManfBusinessUnitsQuery request, CancellationToken cancellationToken)
        {
            var ManfBusinessUnitsInDb = await ManfBusinessUnitService.GetManfBusinessUnitsAsync();

            if (ManfBusinessUnitsInDb.Count > 0)
            {
                return await ResponseWrapper<List<ManfBusinessUnitResponse>>.SuccessAsync(data: ManfBusinessUnitsInDb.Adapt<List<ManfBusinessUnitResponse>>());
            }
            return await ResponseWrapper<List<ManfBusinessUnitResponse>>.SuccessAsync(message: "No Manufacturer BusinessUnits were found.");
        }
    }
}