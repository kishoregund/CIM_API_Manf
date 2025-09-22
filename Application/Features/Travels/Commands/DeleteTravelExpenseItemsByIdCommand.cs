namespace Application.Features.Travels.Commands
{
    public class DeleteTravelExpenseItemsByIdCommand : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; } 
    }

    public class DeleteTravelExpenseItemsByIdCommandHandler(ITravelExpenseItemsService TravelExpenseItemsService)
        : IRequestHandler<DeleteTravelExpenseItemsByIdCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteTravelExpenseItemsByIdCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await TravelExpenseItemsService.DeleteTravelExpenseItemsAsync(request.Id);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted,
                message: "Travel Expense Item deleted successfully.");
        }
    }
}