
namespace Application.Features.Travels.Commands
{
    public class DeleteTravelExpenseByIdCommand : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; }
    }
    public class DeleteTravelExpenseByIdCommandHandler(ITravelExpenseService TravelExpenseService)
        : IRequestHandler<DeleteTravelExpenseByIdCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteTravelExpenseByIdCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await TravelExpenseService.DeleteTravelExpenseAsync(request.Id);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted,
                message: "TravelExpense deleted successfully.");
        }
    }
}