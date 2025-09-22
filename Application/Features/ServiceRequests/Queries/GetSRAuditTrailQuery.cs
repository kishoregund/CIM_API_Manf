

using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetSRAuditTrailQuery : IRequest<IResponseWrapper>
    {
        public Guid SRAuditTrailId { get; set; }
    }

    public class GetSRAuditTrailQueryHandler(ISRAuditTrailService SRAuditTrailService) : IRequestHandler<GetSRAuditTrailQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSRAuditTrailQuery request, CancellationToken cancellationToken)
        {
            var SRAuditTrailInDb = (await SRAuditTrailService.GetSRAuditTrailAsync(request.SRAuditTrailId)).Adapt<SRAuditTrailResponse>();

            if (SRAuditTrailInDb is not null)
            {
                return await ResponseWrapper<SRAuditTrailResponse>.SuccessAsync(data: SRAuditTrailInDb);
            }
            return await ResponseWrapper<SRAuditTrailResponse>.SuccessAsync(message: "SRAuditTrail does not exists.");
        }
    }
}