using Application.Features.AppBasic.Requests;
using Application.Features.AppBasic.Responses;
using Application.Features.Identity.Users;
using Application.Features.Travels;
using Domain.Entities;

namespace Application.Features.Travels.Commands
{
    public class CreateTravelExpenseCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public TravelExpenseRequest Request { get; set; }
    }
    public class CreateTravelExpenseCommandHandler(ITravelExpenseService TravelExpenseService)
        : IRequestHandler<CreateTravelExpenseCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateTravelExpenseCommand request, CancellationToken cancellationToken)
        {
            var TravelExpense = request.Request.Adapt<TravelExpense>();
            var result = await TravelExpenseService.CreateTravelExpenseAsync(TravelExpense);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record saved successfully.");
        }
    }
}