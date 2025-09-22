using Application.Exceptions;
using Application.Features.AMCS.Requests;
using Application.Features.Schools;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Features.AMCS.Commands
{
    public class UpdateAmcItemsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public AmcItemsRequest Request { get; set; }
    }

    public class UpdateAmcItemsHandler(IAmcItemsService amcItemsService)
        : IRequestHandler<UpdateAmcItemsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateAmcItemsCommand request, CancellationToken cancellationToken)
        {
            var amcItems = await amcItemsService.GetByIdAsync(request.Request.Id);

            amcItems.SqNo = request.Request.SqNo;
            amcItems.AMCId = request.Request.AMCId;
            amcItems.ServiceType = request.Request.ServiceType;
            amcItems.Date = request.Request.Date;
            amcItems.ServiceRequestId = request.Request.ServiceRequestId;
            amcItems.EstStartDate = request.Request.EstStartDate;
            amcItems.EstEndDate = request.Request.EstEndDate;
            amcItems.Status = request.Request.Status;

            var response = await amcItemsService.UpdateAsync(amcItems);
            return await ResponseWrapper<Guid>.SuccessAsync(data: response, message: "Record updated successfully.");
        }       
            
    }
}