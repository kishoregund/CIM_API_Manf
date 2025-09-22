using Application.Features.Travels.Requests;
using Domain.Entities;

namespace Application.Features.Travels.Commands
{
    public class UpdateTravelExpenseItemsCommand : IRequest<IResponseWrapper>
    {
        public UpdateTravelExpenseItemsRequest Request { get; set; }
    }

    public class UpdateTravelExpenseItemsCommandHandler(ITravelExpenseItemsService TravelExpenseItemsService)
        : IRequestHandler<UpdateTravelExpenseItemsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateTravelExpenseItemsCommand request,
            CancellationToken cancellationToken)
        {
            var TravelExpensesItems = await TravelExpenseItemsService.GetTravelExpenseItemsEntityAsync(request.Request.Id);

            TravelExpensesItems.Id = request.Request.Id;
            TravelExpensesItems.IsActive = request.Request.IsActive;
            TravelExpensesItems.TravelExpenseId = request.Request.TravelExpenseId;
            TravelExpensesItems.ExpDate = request.Request.ExpDate;
            TravelExpensesItems.ExpDetails = request.Request.ExpDetails;
            TravelExpensesItems.BcyAmt = request.Request.BcyAmt;
            TravelExpensesItems.UsdAmt = request.Request.UsdAmt;
            TravelExpensesItems.Currency = request.Request.Currency;
            TravelExpensesItems.IsBillsAttached = request.Request.IsBillsAttached;
            TravelExpensesItems.Remarks = request.Request.Remarks;
            TravelExpensesItems.ExpNature = request.Request.ExpNature;
            TravelExpensesItems.ExpenseBy = request.Request.ExpenseBy;

            var result = await TravelExpenseItemsService.UpdateTravelExpenseItemsAsync(TravelExpensesItems);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record updated successfully.");
        }
    }
}