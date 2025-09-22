namespace Application.Features.Schools.Queries
{

    // Asignment solution
    public class GetSchoolsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetSchoolsQueryHandler(ISchoolService schoolService) : IRequestHandler<GetSchoolsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSchoolsQuery request, CancellationToken cancellationToken)
        {
            var schoolsInDb = await schoolService.GetSchoolsAsync();

            if (schoolsInDb.Count > 0)
            {
                return await ResponseWrapper<List<SchoolResponse>>.SuccessAsync(data: schoolsInDb.Adapt<List<SchoolResponse>>());
            }
            return await ResponseWrapper<List<SchoolResponse>>.FailAsync(message: "No Schools were found.");
        }
    }
}
