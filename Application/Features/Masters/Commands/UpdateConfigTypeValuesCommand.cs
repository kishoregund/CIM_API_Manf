
using Application.Features.Masters.Requests;
using Domain.Entities;

namespace Application.Features.Masters.Commands
{
    public class UpdateConfigTypeValuesCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UpdateConfigTypeValuesRequest ConfigTypeValuesRequest { get; set; }
    }

    public class UpdateConfigTypeValuesCommandHandler(IConfigTypeValuesService ConfigTypeValuesService) : IRequestHandler<UpdateConfigTypeValuesCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateConfigTypeValuesCommand request, CancellationToken cancellationToken)
        {
            var ConfigTypeValuesInDb = await ConfigTypeValuesService.GetConfigTypeValueAsync(request.ConfigTypeValuesRequest.Id);

            ConfigTypeValuesInDb.Id = request.ConfigTypeValuesRequest.Id;
            ConfigTypeValuesInDb.ConfigValue = request.ConfigTypeValuesRequest.ConfigValue;
            ConfigTypeValuesInDb.ListTypeItemId = request.ConfigTypeValuesRequest.ListTypeItemId;
            ConfigTypeValuesInDb.UpdatedBy = request.ConfigTypeValuesRequest.UpdatedBy;


            var updateConfigTypeValuesId = await ConfigTypeValuesService.UpdateConfigTypeValuesAsync(ConfigTypeValuesInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateConfigTypeValuesId, message: "Record updated successfully.");
        }
    }
}