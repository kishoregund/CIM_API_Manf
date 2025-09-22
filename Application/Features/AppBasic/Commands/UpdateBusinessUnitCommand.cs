using Application.Features.AppBasic.Requests;

namespace Application.Features.AppBasic.Commands
{
    public class UpdateBusinessUnitCommand : IRequest<IResponseWrapper>
    {
        public UpdateBusinessUnitRequest Request { get; set; }
    }

    public class UpdateBusinessUnitCommandHandler(IBusinessUnitService businessUnitService)
        : IRequestHandler<UpdateBusinessUnitCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateBusinessUnitCommand request,
            CancellationToken cancellationToken)
        {
            var businessUnit = await businessUnitService.GetBusinessUnitByIdAsync(request.Request.Id);
            businessUnit.BusinessUnitName = request.Request.BusinessUnitName;

            var result = await businessUnitService.UpdateBusinessUnitAsync(businessUnit);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record updated successfully.");
        }
    }
}