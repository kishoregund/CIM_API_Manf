using Application.Features.AppBasic.Requests;

namespace Application.Features.AppBasic.Commands
{
    public class UpdateBrandCommand : IRequest<IResponseWrapper>
    {
        public UpdateBrandRequest Request { get; set; }
    }

    public class UpdateBrandCommandHandler(IBrandService brandService)
        : IRequestHandler<UpdateBrandCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await brandService.GetBrandByIdAsync(request.Request.Id);

            brand.BrandName = request.Request.BrandName;
            brand.BusinessUnitId = request.Request.BusinessUnitId;

            var result = await brandService.UpdateBrandAsync(brand);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record updated successfully.");
        }
    }
}