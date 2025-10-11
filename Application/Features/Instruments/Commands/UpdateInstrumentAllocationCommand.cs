using Application.Features.Instruments.Commands;
using Application.Features.Instruments;
using Application.Features.Instruments.Requests;
using Domain.Entities;
using System.Runtime.InteropServices.JavaScript;
using System.Diagnostics.Metrics;

namespace Application.Features.InstrumentAllocations.Commands
{
    public class UpdateInstrumentAllocationCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public InstrumentAllocationRequest InstrumentAllocationRequest { get; set; }
    }

    public class UpdateInstrumentAllocationCommandHandler(IInstrumentAllocationService InstrumentAllocationService) : IRequestHandler<UpdateInstrumentAllocationCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateInstrumentAllocationCommand request, CancellationToken cancellationToken)
        {
            var InstrumentAllocationInDb = await InstrumentAllocationService.GetInstrumentAllocationEntityAsync(request.InstrumentAllocationRequest.Id);

            InstrumentAllocationInDb.Id = request.InstrumentAllocationRequest.Id;
            InstrumentAllocationInDb.BrandId = request.InstrumentAllocationRequest.BrandId;
            InstrumentAllocationInDb.BusinessUnitId = request.InstrumentAllocationRequest.BusinessUnitId;
            InstrumentAllocationInDb.InstrumentId = request.InstrumentAllocationRequest.InstrumentId;
            InstrumentAllocationInDb.DistributorId = request.InstrumentAllocationRequest.DistributorId;
            InstrumentAllocationInDb.UpdatedBy = request.InstrumentAllocationRequest.UpdatedBy;

            var updateInstrumentAllocationId = await InstrumentAllocationService.UpdateInstrumentAllocationAsync(InstrumentAllocationInDb);
            return await ResponseWrapper<Guid>.SuccessAsync(data: updateInstrumentAllocationId, message: "Record updated successfully.");
        }
    }
}
