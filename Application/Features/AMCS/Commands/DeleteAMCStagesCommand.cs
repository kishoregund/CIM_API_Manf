namespace Application.Features.AMCS.Commands
{
    public class DeleteAmcStagesCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid Id { get; set; }
    }

    public class DeleteAmcStagesHandler(IAmcStagesService amcService) : IRequestHandler<DeleteAmcCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteAmcCommand request, CancellationToken cancellationToken)
        {
            var response = await amcService.DeleteAmcStages(request.Id);
            return await ResponseWrapper<bool>.SuccessAsync(data: response, message: "AMC Stages deleted successfully.");

        }
    }
}
