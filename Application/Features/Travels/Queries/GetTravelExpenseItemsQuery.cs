using Application.Features.AppBasic.Responses;
using Domain.Entities;

namespace Application.Features.Travels.Queries
{
    public class GetTravelExpenseItemsQuery : IRequest<IResponseWrapper>
    {
        public Guid TravelExpenseId { get; set; }
    }

    public class GetTravelExpenseItemsByCompanyIdQueryHandler(ITravelExpenseItemsService TravelExpenseItemservice) : IRequestHandler<GetTravelExpenseItemsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetTravelExpenseItemsQuery request, CancellationToken cancellationToken)
        {
            var TravelExpenseItemsInDb = await TravelExpenseItemservice.GetTravelExpenseItemsAsync(request.TravelExpenseId);

            if (TravelExpenseItemsInDb.Count > 0)
            {
                return await ResponseWrapper<List<TravelExpenseItemsResponse>>.SuccessAsync(data: TravelExpenseItemsInDb.Adapt<List<TravelExpenseItemsResponse>>());
            }
            return await ResponseWrapper<List<TravelExpenseItemsResponse>>.SuccessAsync(message: "No TravelExpenseItems were found.");
        }
    }
}