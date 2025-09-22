using Application.Features.AppBasic.Requests;
using Application.Features.AppBasic.Responses;
using Application.Features.Identity.Users;
using Domain.Entities;

namespace Application.Features.AppBasic.Commands
{
    public class CreateBrandCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public BrandRequest Request { get; set; }
    }
    public class CreateBrandCommandHandler(IBrandService brandService)
        : IRequestHandler<CreateBrandCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = request.Request.Adapt<Brand>();
            //brand.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            //brand.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            var result = await brandService.CreateBrandAsync(brand);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record saved successfully.");
        }
    }
}