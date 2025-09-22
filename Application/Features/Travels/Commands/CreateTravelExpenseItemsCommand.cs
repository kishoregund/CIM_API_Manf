using Application.Features.AppBasic.Requests;
using Application.Features.AppBasic.Responses;
using Application.Features.Identity.Users;
using Application.Features.Travels.Requests;

namespace Application.Features.Travels.Commands
{
    public class CreateTravelExpenseItemsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public TravelExpenseItemsRequest Request { get; set; }
    }

    public class CreateTravelExpenseItemsCommandHandler(ITravelExpenseItemsService TravelExpenseItemsService)
        : IRequestHandler<CreateTravelExpenseItemsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateTravelExpenseItemsCommand request,
            CancellationToken cancellationToken)
        {
            var TravelExpenseItems = request.Request.Adapt<TravelExpenseItems>();            
            var result = await TravelExpenseItemsService.CreateTravelExpenseItemsAsync(TravelExpenseItems);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result,
                message: "Record saved successfully.");
        }
    }
}