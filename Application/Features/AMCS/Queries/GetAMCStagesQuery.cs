using Application.Exceptions;
using Application.Features.AMCS.Responses;

namespace Application.Features.AMCS.Queries
{
    public class GetAmcStagesQuery : IRequest<IResponseWrapper>
    {
        public Guid AmcId { get; set; } 
    }

    public class GetAmcStagesQueryHandler(IAmcStagesService amcStagesService)
        : IRequestHandler<GetAmcStagesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAmcStagesQuery request, CancellationToken cancellationToken)
        {
            var amcStagesInDb = (await amcStagesService.GetAllByAmcIdAsync(request.AmcId));// .Adapt<List<AmcStagesResponse>>();

            if (amcStagesInDb is not null)
            {
                return await ResponseWrapper<List<AmcStagesResponse>>.SuccessAsync(data: amcStagesInDb);
            }
            return await ResponseWrapper<AmcStagesResponse>.SuccessAsync(message: "AMC Stages does not exists.");
        }
    }
}