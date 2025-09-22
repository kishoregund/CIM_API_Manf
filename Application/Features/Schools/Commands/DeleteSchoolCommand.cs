namespace Application.Features.Schools.Commands
{
    public class DeleteSchoolCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public int SchoolId { get; set; }
    }

    public class DeleteSchoolCommandHandler(ISchoolService schoolService) : IRequestHandler<DeleteSchoolCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolInDb = await schoolService.GetSchoolByIdAsync(request.SchoolId);
            var deletedSchoolId = await schoolService.DeleteSchoolAsync(schoolInDb);

            return await ResponseWrapper<int>.SuccessAsync(data: deletedSchoolId, message: "School deleted successfully.");
        }
    }
}
