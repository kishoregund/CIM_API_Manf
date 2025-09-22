namespace Application.Features.ServiceRequests.Commands
{
    public class DeleteSREngActionCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SREngActionId { get; set; }
    }

    public class DeleteSREngActionCommandHandler(ISREngActionService SREngActionService) : IRequestHandler<DeleteSREngActionCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSREngActionCommand request, CancellationToken cancellationToken)
        {
            var deletedSREngAction = await SREngActionService.DeleteSREngActionAsync(request.SREngActionId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedSREngAction, message: "SREngAction deleted successfully.");
        }
    }
}
