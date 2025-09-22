using Application.Features.ServiceReports;

namespace Application.Features.SPConsumeds.Commands
{
    public class DeleteSPConsumedCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SPConsumedId { get; set; }
    }

    public class DeleteSPConsumedCommandHandler(ISPConsumedService SPConsumedService) : IRequestHandler<DeleteSPConsumedCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSPConsumedCommand request, CancellationToken cancellationToken)
        {
            var deletedSPConsumed = await SPConsumedService.DeleteSPConsumedAsync(request.SPConsumedId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedSPConsumed, message: "SPConsumed deleted successfully.");
        }
    }
}
