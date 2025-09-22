
namespace Application.Features.Travels.Commands
{
    public class DeleteAdvanceRequestByIdCommand : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; }
    }
    public class DeleteAdvanceRequestByIdCommandHandler(IAdvanceRequestService AdvanceRequestService)
        : IRequestHandler<DeleteAdvanceRequestByIdCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteAdvanceRequestByIdCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await AdvanceRequestService.DeleteAdvanceRequestAsync(request.Id);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted, message: "Advance Request deleted successfully.");
        }
    }
}