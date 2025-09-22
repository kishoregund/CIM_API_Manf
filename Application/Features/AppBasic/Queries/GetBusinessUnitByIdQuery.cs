using Application.Features.AppBasic.Responses;

namespace Application.Features.AppBasic.Queries
{
    public class GetBusinessUnitByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid BusinessUnitId { get; set; } 
    }

    public class GetBusinessUnitByIdQueryHandler(IBusinessUnitService businessUnitService) : IRequestHandler<GetBusinessUnitByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetBusinessUnitByIdQuery request, CancellationToken cancellationToken)
        {
            var businessUnitInDb = (await businessUnitService.GetBusinessUnitByIdAsync(request.BusinessUnitId)).Adapt<BusinessUnitResponse>();

            if (businessUnitInDb is not null)
            {
                return await ResponseWrapper<BusinessUnitResponse>.SuccessAsync(data: businessUnitInDb);
            }
            return await ResponseWrapper<BusinessUnitResponse>.SuccessAsync(message: "Business Unit does not exists.");
        }
    }
}