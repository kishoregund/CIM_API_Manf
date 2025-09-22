using Application.Exceptions;
using Application.Features.AMCS.Requests;
using Domain.Entities;
using System.Globalization;

namespace Application.Features.AMCS.Commands
{
    public class UpdateAmcCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public AmcRequest Request { get; set; }
    }

    public class UpdateAmcCommandHandler(IAmcService amcService) : IRequestHandler<UpdateAmcCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateAmcCommand request, CancellationToken cancellationToken)
        {
            var amc = await amcService.GetByIdEntityAsync(request.Request.Id);
          
            amc.BaseCurrencyAmt = request.Request.BaseCurrencyAmt;
            amc.BillTo = request.Request.BillTo;
            amc.BrandId = request.Request.BrandId;
            amc.ConversionAmount = request.Request.ConversionAmount;
            amc.CreatedBy = request.Request.CreatedBy;
            amc.CreatedOn = request.Request.CreatedOn;
            amc.CurrencyId = request.Request.CurrencyId;
            amc.EDate = request.Request.EDate;
            amc.FirstVisitDate = request.Request.FirstVisitDate;
            if ((DateTime.Now.Date < DateTime.ParseExact(request.Request.EDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date))
            {
                amc.IsActive = true;
            }
            else { amc.IsActive = false; }

            //amc.IsActive = request.Request.IsActive;
            amc.IsDeleted = request.Request.IsDeleted;

            amc.IsMultipleBreakdown = request.Request.IsMultipleBreakdown;
            amc.PaymentTerms = request.Request.PaymentTerms;
            amc.Project = request.Request.Project;
            amc.SecondVisitDate = request.Request.SecondVisitDate;
            amc.ServiceQuote = request.Request.ServiceQuote;
            amc.ServiceType = request.Request.ServiceType;
            amc.SDate = request.Request.SDate;
            amc.SqDate = request.Request.SqDate;
            amc.TnC = request.Request.TnC;
            amc.UpdatedBy = request.Request.UpdatedBy;
            amc.UpdatedOn = DateTime.Now;
            amc.Zerorate = request.Request.Zerorate;

            var response = await amcService.UpdateAmcAsync(amc);

            return await ResponseWrapper<Guid>.SuccessAsync(response, "AMC updated successfully.");
        }
    }
}
