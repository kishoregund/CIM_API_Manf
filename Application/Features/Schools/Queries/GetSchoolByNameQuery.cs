namespace Application.Features.Schools.Queries
{
    public class GetSchoolByNameQuery : IRequest<IResponseWrapper>
    {
        public string Name { get; set; }
    }

    public class GetSchoolByNameQueryHandler(ISchoolService schoolService) : IRequestHandler<GetSchoolByNameQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSchoolByNameQuery request, CancellationToken cancellationToken)
        {
            var schooolInDb = (await schoolService.GetSchoolByNameAsync(request.Name)).Adapt<SchoolResponse>();

            if (schooolInDb is not null)
            {
                return await ResponseWrapper<SchoolResponse>.SuccessAsync(data: schooolInDb);
            }
            return await ResponseWrapper<SchoolResponse>.FailAsync(message: "School does not exists.");
        }
    }
}
