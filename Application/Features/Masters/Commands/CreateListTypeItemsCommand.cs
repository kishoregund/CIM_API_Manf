using Application.Features.Masters.Requests;
using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Commands
{
    public class CreateListTypeItemsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public ListTypeItemsRequest ListTypeItemsRequest { get; set; }
    }

    public class CreateListTypeItemsCommandHandler(IListTypeItemsService ListTypeItemsService) : IRequestHandler<CreateListTypeItemsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateListTypeItemsCommand request, CancellationToken cancellationToken)
        {
            // map

            var newListTypeItems = request.ListTypeItemsRequest.Adapt<ListTypeItems>();

            var ListTypeItemsId = await ListTypeItemsService.CreateListTypeItemsAsync(newListTypeItems);

            return await ResponseWrapper<Guid>.SuccessAsync(data: ListTypeItemsId, message: "Record saved successfully.");
        }
    }
}
