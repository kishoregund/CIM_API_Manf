using Application.Features.AppBasic.Requests;
using Application.Features.AppBasic.Responses;
using Application.Features.Identity.Users;
using Application.Features.Travels;
using Domain.Entities;

namespace Application.Features.Travels.Commands
{
    public class CreateBankDetailsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public BankDetails Request { get; set; }
    }
    public class CreateBankDetailsCommandHandler(IBankDetailsService BankDetailsService)
        : IRequestHandler<CreateBankDetailsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateBankDetailsCommand request, CancellationToken cancellationToken)
        {
            var BankDetails = request.Request.Adapt<BankDetails>();
            var result = await BankDetailsService.CreateBankDetailsAsync(BankDetails);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record saved successfully.");
        }
    }
}