namespace Application.Features.Masters.Commands
{
    public class DeleteListTypeItemsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid ListTypeItemsId { get; set; }
    }

    public class DeleteListTypeItemsCommandHandler(IListTypeItemsService ListTypeItemsService) : IRequestHandler<DeleteListTypeItemsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteListTypeItemsCommand request, CancellationToken cancellationToken)
        {
            var deletedListTypeItems = await ListTypeItemsService.DeleteListTypeItemsAsync(request.ListTypeItemsId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedListTypeItems, message: "ListTypeItems deleted successfully.");
        }
    }
}
