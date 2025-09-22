
using Application.Features.Distributors.Requests;
using Domain.Entities;

namespace Application.Features.Distributors.Commands
{
    public class CreateDistributorCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public DistributorRequest DistributorRequest { get; set; } 
    }

    public class CreateDistributorCommandHandler(IDistributorService distributorService)
       : IRequestHandler<CreateDistributorCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateDistributorCommand request, CancellationToken cancellationToken)
        {

            var distributor = request.DistributorRequest.Adapt<Distributor>();

            var distributorId = await distributorService.CreateDistributorAsync(distributor);

            return await ResponseWrapper<Guid>.SuccessAsync(data: distributorId, message: "Record saved successfully.");

        }
    }
}