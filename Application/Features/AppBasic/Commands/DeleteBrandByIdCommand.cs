namespace Application.Features.AppBasic.Commands
{
    public class DeleteBrandByIdCommand : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; }
    }
    public class DeleteBrandByIdCommandHandler(IBrandService brandService)
        : IRequestHandler<DeleteBrandByIdCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteBrandByIdCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await brandService.DeleteBrandAsync(request.Id);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted,
                message: "Brand deleted successfully.");
        }
    }
}