using Application.Features.Masters.Requests;
using Domain.Entities;

namespace Application.Features.Masters.Commands
{
    public class UpdateListTypeItemsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UpdateListTypeItemsRequest ListTypeItemsRequest { get; set; }
    }

    public class UpdateListTypeItemsCommandHandler(IListTypeItemsService ListTypeItemsService) : IRequestHandler<UpdateListTypeItemsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateListTypeItemsCommand request, CancellationToken cancellationToken)
        {
            var ListTypeItemsInDb = await ListTypeItemsService.GetListTypeItemAsync(request.ListTypeItemsRequest.Id);

            ListTypeItemsInDb.Id = request.ListTypeItemsRequest.Id;
            ListTypeItemsInDb.UpdatedBy = request.ListTypeItemsRequest.UpdatedBy;
            ListTypeItemsInDb.Code = request.ListTypeItemsRequest.Code;
            ListTypeItemsInDb.IsEscalationSupervisor = request.ListTypeItemsRequest.IsEscalationSupervisor;
            ListTypeItemsInDb.ItemName = request.ListTypeItemsRequest.ItemName;
            ListTypeItemsInDb.ListTypeId = request.ListTypeItemsRequest.ListTypeId;


            var updateListTypeItemsId = await ListTypeItemsService.UpdateListTypeItemsAsync(ListTypeItemsInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateListTypeItemsId, message: "Record updated successfully.");
        }
    }
}