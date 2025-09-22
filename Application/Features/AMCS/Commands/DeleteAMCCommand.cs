namespace Application.Features.AMCS.Commands
{
    public class DeleteAmcCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid Id { get; set; }
    }

    public class DeleteAmcCommandHandler(IAmcService amcService)
        : IRequestHandler<DeleteAmcCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteAmcCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await amcService.DeleteAmc(request.Id);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted, message: "AMC deleted successfully.");
        }
    }
}