

using Application.Features.Masters.Requests;

namespace Application.Features.Masters.Commands
{
    public class CreateConfigTypeValuesCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public ConfigTypeValuesRequest ConfigTypeValuesRequest { get; set; }
    }

    public class CreateConfigTypeValuesCommandHandler(IConfigTypeValuesService ConfigTypeValuesService) : IRequestHandler<CreateConfigTypeValuesCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateConfigTypeValuesCommand request, CancellationToken cancellationToken)
        {
            // map

            var newConfigTypeValues = request.ConfigTypeValuesRequest.Adapt<ConfigTypeValues>();

            var ConfigTypeValuesId = await ConfigTypeValuesService.CreateConfigTypeValuesAsync(newConfigTypeValues);

            return await ResponseWrapper<Guid>.SuccessAsync(data: ConfigTypeValuesId, message: "Record saved successfully.");
        }
    }
}
