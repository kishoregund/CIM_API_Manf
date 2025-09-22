using Domain.Entities;

namespace Application.Features.Schools.Commands
{
    public class CreateSchoolCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CreateSchoolRequest SchoolRequest { get; set; }
    }

    public class CreateSchoolCommandHandler(ISchoolService schoolService) : IRequestHandler<CreateSchoolCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSchool = request.SchoolRequest.Adapt<School>();

            var schoolId = await schoolService.CreateSchoolAsync(newSchool);

            return await ResponseWrapper<int>.SuccessAsync(data: schoolId, message: "School created successfully.");
        }
    }
}
