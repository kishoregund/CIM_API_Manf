
using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Queries
{
    public class GetConfigTypeValuesByTypeIdQuery : IRequest<IResponseWrapper>
    {
        public Guid LisTypeItemId { get; set; }
    }

    public class GetConfigTypeValuesByTypeIdQueryHandler(IConfigTypeValuesService ConfigTypeValueService) : IRequestHandler<GetConfigTypeValuesByTypeIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetConfigTypeValuesByTypeIdQuery request, CancellationToken cancellationToken)
        {
            var ConfigTypeValuesInDb = await ConfigTypeValueService.GetConfigTypeValuesByTypeIdAsync(request.LisTypeItemId);

            if (ConfigTypeValuesInDb.Count > 0)
            {
                return await ResponseWrapper<List<ConfigTypeValuesResponse>>.SuccessAsync(data: ConfigTypeValuesInDb.Adapt<List<ConfigTypeValuesResponse>>());
            }
            return await ResponseWrapper<List<ConfigTypeValuesResponse>>.SuccessAsync(message: "No ConfigTypeValues were found.");
        }
    }
}
