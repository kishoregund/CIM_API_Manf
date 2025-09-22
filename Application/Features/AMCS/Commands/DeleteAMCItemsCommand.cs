namespace Application.Features.AMCS.Commands
{
    public class DeleteAmcItemsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid Id { get; set; } 
    }

    public class DeleteAmcItemsCommandHandler(IAmcItemsService amcItemsService)
        : IRequestHandler<DeleteAmcItemsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteAmcItemsCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await amcItemsService.DeleteAmcItems(request.Id);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted, message: "AMC Items deleted successfully.");
        }
    }
}