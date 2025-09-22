using Application.Features.AMCS.Requests;
using Domain.Entities;

namespace Application.Features.AMCS.Commands
{
    public class CreateAmcCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public AmcRequest CreateAmcRequest { get; set; }
    }


    public class CreateAmcCommandHandler(IAmcService amcService)
        : IRequestHandler<CreateAmcCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateAmcCommand request, CancellationToken cancellationToken)
        {
            var amcEntity = request.CreateAmcRequest.Adapt<AMC>();
            var newAmc = await amcService.CreateAmc(amcEntity);
            return await ResponseWrapper<Guid>.SuccessAsync(data: newAmc, message: "Record created successfully.");

        }
    }
}