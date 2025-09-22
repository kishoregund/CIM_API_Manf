using Application.Features.AppBasic.Requests;
using Domain.Entities;

namespace Application.Features.Travels.Commands
{
    public class UpdateTravelExpenseCommand : IRequest<IResponseWrapper>
    {
        public UpdateTravelExpenseRequest Request { get; set; }
    }

    public class UpdateTravelExpenseCommandHandler(ITravelExpenseService TravelExpenseService)
        : IRequestHandler<UpdateTravelExpenseCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateTravelExpenseCommand request, CancellationToken cancellationToken)
        {
            var TravelExpense = await TravelExpenseService.GetTravelExpenseEntityByIdAsync(request.Request.Id);

            TravelExpense.Id = request.Request.Id;
            TravelExpense.IsActive = request.Request.IsActive;

            TravelExpense.EndDate = request.Request.EndDate;
            TravelExpense.EngineerId = request.Request.EngineerId;
            TravelExpense.DistributorId = request.Request.DistributorId;
            TravelExpense.CustomerId = request.Request.CustomerId;
            TravelExpense.ServiceRequestId = request.Request.ServiceRequestId;
            TravelExpense.StartDate = request.Request.StartDate;
            TravelExpense.GrandCompanyTotal = request.Request.GrandCompanyTotal;
            TravelExpense.GrandEngineerTotal = request.Request.GrandEngineerTotal;
            TravelExpense.Designation = request.Request.Designation;
            TravelExpense.TotalDays = request.Request.TotalDays;

            var result = await TravelExpenseService.UpdateTravelExpenseAsync(TravelExpense);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record updated successfully.");
        }
    }
}