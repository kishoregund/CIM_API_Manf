using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Queries
{
    public class GetConfigTypeValuesByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ConfigTypeValuesId { get; set; }
    }

    public class GetConfigTypeValuesByIdQueryHandler(IConfigTypeValuesService ConfigTypeValuesService) : IRequestHandler<GetConfigTypeValuesByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetConfigTypeValuesByIdQuery request, CancellationToken cancellationToken)
        {
            var schooolInDb = (await ConfigTypeValuesService.GetConfigTypeValueAsync(request.ConfigTypeValuesId)).Adapt<ConfigTypeValuesResponse>();

            if (schooolInDb is not null)
            {
                return await ResponseWrapper<ConfigTypeValuesResponse>.SuccessAsync(data: schooolInDb);
            }
            return await ResponseWrapper<ConfigTypeValuesResponse>.SuccessAsync(message: "ConfigTypeValues does not exists.");
        }
    }
}
