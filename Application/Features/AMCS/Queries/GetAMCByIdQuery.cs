using Application.Exceptions;
using Application.Features.AMCS.Responses;
using Application.Features.Schools;


namespace Application.Features.AMCS.Queries
{
    public class GetAmcByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; }
    }

    public class GetAmcByIdQueryHandler(IAmcService amcService) : IRequestHandler<GetAmcByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAmcByIdQuery request, CancellationToken cancellationToken)
        {
            var amcInDb = (await amcService.GetByIdAsync(request.Id)).Adapt<AmcResponse>();

            if (amcInDb is not null)
            {
                return await ResponseWrapper<AmcResponse>.SuccessAsync(data: amcInDb);
            }
            return await ResponseWrapper<AmcResponse>.SuccessAsync(message: "AMC does not exists.");
        }


    }
}