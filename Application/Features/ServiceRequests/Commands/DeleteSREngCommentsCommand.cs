namespace Application.Features.ServiceRequests.Commands
{
    public class DeleteSREngCommentsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SREngCommentId { get; set; }
    }

    public class DeleteSREngCommentCommandHandler(ISREngCommentsService SREngCommentService) : IRequestHandler<DeleteSREngCommentsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSREngCommentsCommand request, CancellationToken cancellationToken)
        {
            var deletedSREngComment = await SREngCommentService.DeleteSREngCommentAsync(request.SREngCommentId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedSREngComment, message: "SREngComment deleted successfully.");
        }
    }
}
