
namespace Application.Features.ServiceReports.Commands
{
    public class DeleteSPRecommendedCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SPRecommendedId { get; set; }
    }

    public class DeleteSPRecommendedCommandHandler(ISPRecommendedService SPRecommendedService) : IRequestHandler<DeleteSPRecommendedCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSPRecommendedCommand request, CancellationToken cancellationToken)
        {
            var deletedSPRecommended = await SPRecommendedService.DeleteSPRecommendedAsync(request.SPRecommendedId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedSPRecommended, message: "SPRecommended deleted successfully.");
        }
    }
}
