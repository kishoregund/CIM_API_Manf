
namespace Application.Features.ServiceRequests.Commands
{
    public class DeleteSRAuditTrailCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SRAuditTrailId { get; set; }
    }

    public class DeleteSRAuditTrailCommandHandler(ISRAuditTrailService SRAuditTrailService) : IRequestHandler<DeleteSRAuditTrailCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSRAuditTrailCommand request, CancellationToken cancellationToken)
        {
            var deletedSRAuditTrail = await SRAuditTrailService.DeleteSRAuditTrailAsync(request.SRAuditTrailId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedSRAuditTrail, message: "SRAuditTrail deleted successfully.");
        }
    }
}
