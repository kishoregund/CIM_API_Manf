using Application.Features.AppBasic.Requests;

namespace Application.Features.AppBasic.Commands
{
    public class UpdateManfBusinessUnitCommand : IRequest<IResponseWrapper>
    {
        public UpdateManfBusinessUnitRequest Request { get; set; }
    }

    public class UpdateManfBusinessUnitCommandHandler(IManfBusinessUnitService manfBusinessUnitService)
        : IRequestHandler<UpdateManfBusinessUnitCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateManfBusinessUnitCommand request,
            CancellationToken cancellationToken)
        {
            var ManfBusinessUnit = await manfBusinessUnitService.GetManfBusinessUnitByIdAsync(request.Request.Id);
            ManfBusinessUnit.BusinessUnitName = request.Request.BusinessUnitName;

            var result = await manfBusinessUnitService.UpdateManfBusinessUnitAsync(ManfBusinessUnit);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record updated successfully.");
        }
    }
}