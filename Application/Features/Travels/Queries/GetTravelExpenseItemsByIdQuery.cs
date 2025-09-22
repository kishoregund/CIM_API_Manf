using Application.Features.AppBasic.Responses;

namespace Application.Features.Travels.Queries
{
    public class GetTravelExpenseItemsByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid TravelExpenseItemsId { get; set; } 
    }

    public class GetTravelExpenseItemsByIdQueryHandler(ITravelExpenseItemsService TravelExpenseItemsService) : IRequestHandler<GetTravelExpenseItemsByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetTravelExpenseItemsByIdQuery request, CancellationToken cancellationToken)
        {
            var TravelExpenseItemsInDb = (await TravelExpenseItemsService.GetTravelExpenseItemsByIdAsync(request.TravelExpenseItemsId)).Adapt<TravelExpenseItemsResponse>();

            if (TravelExpenseItemsInDb is not null)
            {
                return await ResponseWrapper<TravelExpenseItemsResponse>.SuccessAsync(data: TravelExpenseItemsInDb);
            }
            return await ResponseWrapper<TravelExpenseItemsResponse>.SuccessAsync(message: "Business Unit does not exists.");
        }
    }
}