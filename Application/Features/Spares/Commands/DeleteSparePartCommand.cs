namespace Application.Features.Spares.Commands
{
    public class DeleteSparePartCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SparepartId { get; set; }
    }

    public class DeleteSparePartCommandHandler(ISparepartService sparepartService)
        : IRequestHandler<DeleteSparePartCommand, IResponseWrapper>
    {

        public async Task<IResponseWrapper> Handle(DeleteSparePartCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await sparepartService.DeleteSparepartAsync(request.SparepartId);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted,
                message: "Sparepart deleted successfully.");
        }
    }
}