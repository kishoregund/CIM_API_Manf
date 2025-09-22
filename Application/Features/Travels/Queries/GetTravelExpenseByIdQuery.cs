using Application.Features.AppBasic.Responses;
using Application.Features.Travels;
using Application.Features.Travels.Responses;

namespace Application.Features.Travels.Queries
{
    public class GetTravelExpenseByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid TravelExpenseId { get; set; }
    }

    public class GetTravelExpenseByIdQueryHandler(ITravelExpenseService TravelExpenseService) : IRequestHandler<GetTravelExpenseByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetTravelExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            var TravelExpenseInDb = (await TravelExpenseService.GetTravelExpenseByIdAsync(request.TravelExpenseId)).Adapt<TravelExpenseResponse>();

            if (TravelExpenseInDb is not null)
            {
                return await ResponseWrapper<TravelExpenseResponse>.SuccessAsync(data: TravelExpenseInDb);
            }
            return await ResponseWrapper<TravelExpenseResponse>.SuccessAsync(message: "TravelExpense does not exists.");
        }
    }
}