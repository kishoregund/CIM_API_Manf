using Application.Features.AMCS.Requests;
using Domain.Entities;

namespace Application.Features.AMCS.Commands
{
    public class CreateAmcStagesCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public AmcStagesRequest Request { get; set; }
    }

    public class CreateAmcStagesCommandHandler(IAmcStagesService amcStagesService)
        : IRequestHandler<CreateAmcStagesCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateAmcStagesCommand request, CancellationToken cancellationToken)
        {
            var amcStagesEntity = request.Request.Adapt<AMCStages>();
            var newAmcStages = await amcStagesService.CreateAmcStages(amcStagesEntity);
            return await ResponseWrapper<Guid>.SuccessAsync(data: newAmcStages,
                message: "Record saved successfully.");
        }
    }
}
