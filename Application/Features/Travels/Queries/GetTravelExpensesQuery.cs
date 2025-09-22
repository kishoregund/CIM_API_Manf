using Application.Features.Travels.Responses;

namespace Application.Features.Travels.Queries
{
    public class GetTravelExpensesQuery : IRequest<IResponseWrapper>
    {
        public string BusinessUnitId { get; set; }
        public string BrandId { get; set; }
    }

    public class GetTravelExpensesQueryHandler(ITravelExpenseService TravelExpenseservice) : IRequestHandler<GetTravelExpensesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetTravelExpensesQuery request, CancellationToken cancellationToken)
        {
            var TravelExpensesInDb = await TravelExpenseservice.GetTravelExpensesAsync(request.BusinessUnitId, request.BrandId);

            if (TravelExpensesInDb.Count > 0)
            {
                return await ResponseWrapper<List<TravelExpenseResponse>>.SuccessAsync(data: TravelExpensesInDb.Adapt<List<TravelExpenseResponse>>());
            }
            return await ResponseWrapper<List<TravelExpenseResponse>>.SuccessAsync(message: "No TravelExpenses were found.");
        }
    }
}