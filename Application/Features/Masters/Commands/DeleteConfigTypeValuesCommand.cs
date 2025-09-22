
namespace Application.Features.Masters.Commands
{
    public class DeleteConfigTypeValuesCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid ConfigTypeValuesId { get; set; }
    }

    public class DeleteConfigTypeValuesCommandHandler(IConfigTypeValuesService ConfigTypeValuesService) : IRequestHandler<DeleteConfigTypeValuesCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteConfigTypeValuesCommand request, CancellationToken cancellationToken)
        {
            var deletedConfigTypeValues = await ConfigTypeValuesService.DeleteConfigTypeValuesAsync(request.ConfigTypeValuesId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedConfigTypeValues, message: "ConfigTypeValues deleted successfully.");
        }
    }
}
