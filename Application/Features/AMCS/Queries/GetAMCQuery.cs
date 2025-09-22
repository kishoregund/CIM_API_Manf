using Application.Exceptions;
using Application.Features.AMCS.Responses;

namespace Application.Features.AMCS.Queries
{
    public class GetAmcQuery : IRequest<IResponseWrapper>;

    public class GetAmcQueryHandler(IAmcService amcService) : IRequestHandler<GetAmcQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAmcQuery request, CancellationToken cancellationToken)
        {
            var amcInDb = (await amcService.GetAllAsync()).Adapt<List<AmcResponse>>();

            if (amcInDb is not null)
            {
                return await ResponseWrapper<List<AmcResponse>>.SuccessAsync(data: amcInDb);
            }
            return await ResponseWrapper<AmcResponse>.SuccessAsync(message: "AMC  does not exists.");
        }
    }
}