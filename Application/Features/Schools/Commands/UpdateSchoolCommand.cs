namespace Application.Features.Schools.Commands
{
    public class UpdateSchoolCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UpdateSchoolRequest SchoolRequest { get; set; }
    }

    public class UpdateSchoolCommandHandler(ISchoolService schoolService) : IRequestHandler<UpdateSchoolCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolInDb = await schoolService.GetSchoolByIdAsync(request.SchoolRequest.Id);

            schoolInDb.Name = request.SchoolRequest.Name;
            schoolInDb.EstablishedOn = request.SchoolRequest.EstablishedOn;

            var updateSchoolId = await schoolService.UpdateSchoolAsync(schoolInDb);

            return await ResponseWrapper<int>.SuccessAsync(data: updateSchoolId, message: "School updated successfully.");
        }
    }
}
