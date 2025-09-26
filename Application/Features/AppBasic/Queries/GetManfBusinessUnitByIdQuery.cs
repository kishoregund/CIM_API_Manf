using Application.Features.AppBasic.Responses;

namespace Application.Features.AppBasic.Queries
{
    public class GetManfBusinessUnitByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ManfBusinessUnitId { get; set; } 
    }

    public class GetManfBusinessUnitByIdQueryHandler(IManfBusinessUnitService ManfBusinessUnitService) : IRequestHandler<GetManfBusinessUnitByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetManfBusinessUnitByIdQuery request, CancellationToken cancellationToken)
        {
            var ManfBusinessUnitInDb = (await ManfBusinessUnitService.GetManfBusinessUnitByIdAsync(request.ManfBusinessUnitId)).Adapt<ManfBusinessUnitResponse>();

            if (ManfBusinessUnitInDb is not null)
            {
                return await ResponseWrapper<ManfBusinessUnitResponse>.SuccessAsync(data: ManfBusinessUnitInDb);
            }
            return await ResponseWrapper<ManfBusinessUnitResponse>.SuccessAsync(message: "Business Unit does not exists.");
        }
    }
}