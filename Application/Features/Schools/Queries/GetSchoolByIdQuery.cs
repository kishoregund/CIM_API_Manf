namespace Application.Features.Schools.Queries
{
    public class GetSchoolByIdQuery : IRequest<IResponseWrapper>
    {
        public int SchoolId { get; set; }
    }

    public class GetSchoolByIdQueryHandler(ISchoolService schoolService) : IRequestHandler<GetSchoolByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSchoolByIdQuery request, CancellationToken cancellationToken)
        {
            var schooolInDb = (await schoolService.GetSchoolByIdAsync(request.SchoolId)).Adapt<SchoolResponse>();

            if (schooolInDb is not null)
            {
                return await ResponseWrapper<SchoolResponse>.SuccessAsync(data: schooolInDb);
            }
            return await ResponseWrapper<SchoolResponse>.FailAsync(message: "School does not exists.");
        }
    }
}
