using Application.Features.ServiceReports.Requests;
using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Commands
{
    public class CreateSRPEngWorkDoneCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SRPEngWorkDoneRequest SRPEngWorkDoneRequest { get; set; }
    }

    public class CreateSRPEngWorkDoneCommandHandler(ISRPEngWorkDoneService SRPEngWorkDoneService) : IRequestHandler<CreateSRPEngWorkDoneCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSRPEngWorkDoneCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSRPEngWorkDone = request.SRPEngWorkDoneRequest.Adapt<SRPEngWorkDone>();

            var SRPEngWorkDoneId = await SRPEngWorkDoneService.CreateSRPEngWorkDoneAsync(newSRPEngWorkDone);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SRPEngWorkDoneId, message: "Record saved successfully.");
        }
    }
}
