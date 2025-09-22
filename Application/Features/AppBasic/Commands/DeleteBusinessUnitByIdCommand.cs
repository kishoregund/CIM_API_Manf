namespace Application.Features.AppBasic.Commands
{
    public class DeleteBusinessUnitByIdCommand : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; } 
    }

    public class DeleteBusinessUnitByIdCommandHandler(IBusinessUnitService businessUnitService)
        : IRequestHandler<DeleteBusinessUnitByIdCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteBusinessUnitByIdCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await businessUnitService.DeleteBusinessUnitAsync(request.Id);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted,
                message: "Business unit deleted successfully.");
        }
    }
}