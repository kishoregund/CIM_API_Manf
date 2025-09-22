using Application.Features.AppBasic.Requests;
using Application.Features.AppBasic.Responses;
using Application.Features.Identity.Users;

namespace Application.Features.AppBasic.Commands
{
    public class CreateBusinessUnitCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public BusinessUnitRequest Request { get; set; }
    }

    public class CreateBusinessUnitCommandHandler(IBusinessUnitService businessUnitService)
        : IRequestHandler<CreateBusinessUnitCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateBusinessUnitCommand request,
            CancellationToken cancellationToken)
        {
            var businessUnit = request.Request.Adapt<BusinessUnit>();            
            var result = await businessUnitService.CreateBusinessUnitAsync(businessUnit);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result,
                message: "Record saved successfully.");
        }
    }
}