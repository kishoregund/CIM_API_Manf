namespace Application.Features.Distributors.Commands
{
    public class DeleteRegionCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid RegionId { get; set; }
    }


    public class DeleteRegionCommandHandler(IRegionService regionService, IRegionContactService regionContactService) : IRequestHandler<DeleteRegionCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
        {
            var regionContacts = await regionContactService.GetRegionContactsAsync(request.RegionId);
            if (regionContacts.Count == 0)
            {
                var isDeleted = await regionService.DeleteRegionAsync(request.RegionId);

                return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted, message: "Region deleted successfully.");
            }
            return await ResponseWrapper<bool>.SuccessAsync(data: false, message: "Contacts exists for this Region and cannot be deleted.");
        }
    }
}
