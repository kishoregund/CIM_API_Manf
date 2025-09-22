using Application.Features.AMCS.Requests;
using Domain.Entities;

namespace Application.Features.AMCS.Commands
{
    public class CreateAmcItemsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public AmcItemsRequest Request{ get; set; }
    }

    public class CreateAmcItemsCommandHandler(IAmcItemsService amcItemsService)
        : IRequestHandler<CreateAmcItemsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateAmcItemsCommand request, CancellationToken cancellationToken)
        {
            var amcItemsEntity = request.Request.Adapt<AMCItems>();
            var newAmcItems = await amcItemsService.CreateAmcItems(amcItemsEntity);
            return await ResponseWrapper<Guid>.SuccessAsync(data: newAmcItems, message: "Record saved successfully.");
        }
    }
}
