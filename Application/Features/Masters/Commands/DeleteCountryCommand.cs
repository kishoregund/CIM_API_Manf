namespace Application.Features.Masters.Commands
{
    public class DeleteCountryCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid CountryId { get; set; }
    }

    public class DeleteCountryCommandHandler(ICountryService CountryService) : IRequestHandler<DeleteCountryCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var deletedCountry = await CountryService.DeleteCountryAsync(request.CountryId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedCountry, message: "Country deleted successfully.");
        }
    }
}
