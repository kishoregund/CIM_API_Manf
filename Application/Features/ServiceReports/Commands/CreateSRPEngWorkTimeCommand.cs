using Application.Features.ServiceReports.Requests;
using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Commands
{
    public class CreateSRPEngWorkTimeCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SRPEngWorkTimeRequest SRPEngWorkTimeRequest { get; set; }
    }

    public class CreateSRPEngWorkTimeCommandHandler(ISRPEngWorkTimeService SRPEngWorkTimeService) : IRequestHandler<CreateSRPEngWorkTimeCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSRPEngWorkTimeCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSRPEngWorkTime = request.SRPEngWorkTimeRequest.Adapt<SRPEngWorkTime>();

            var SRPEngWorkTimeId = await SRPEngWorkTimeService.CreateSRPEngWorkTimeAsync(newSRPEngWorkTime);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SRPEngWorkTimeId, message: "Record saved successfully.");
        }
    }
}
