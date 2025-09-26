using Application.Features.AppBasic.Requests;
using Application.Features.AppBasic.Responses;
using Application.Features.Identity.Users;

namespace Application.Features.AppBasic.Commands
{
    public class CreateManfBusinessUnitCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public ManfBusinessUnitRequest Request { get; set; }
    }

    public class CreateManfBusinessUnitCommandHandler(IManfBusinessUnitService manfBusinessUnitService)
        : IRequestHandler<CreateManfBusinessUnitCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateManfBusinessUnitCommand request,
            CancellationToken cancellationToken)
        {
            var ManfBusinessUnit = request.Request.Adapt<ManfBusinessUnit>();            
            var result = await manfBusinessUnitService.CreateManfBusinessUnitAsync(ManfBusinessUnit);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result,
                message: "Record saved successfully.");
        }
    }
}