namespace Application.Features.AppBasic.Commands
{
    public class DeleteManfBusinessUnitByIdCommand : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; } 
    }

    public class DeleteManfBusinessUnitByIdCommandHandler(IManfBusinessUnitService ManfBusinessUnitService)
        : IRequestHandler<DeleteManfBusinessUnitByIdCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteManfBusinessUnitByIdCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await ManfBusinessUnitService.DeleteManfBusinessUnitAsync(request.Id);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted,
                message: "Business unit deleted successfully.");
        }
    }
}