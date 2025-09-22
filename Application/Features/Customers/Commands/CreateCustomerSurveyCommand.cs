using Application.Features.Customers.Requests;
using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Commands
{
    public class CreateCustomerSurveyCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CustomerSurveyRequest CustomerSurveyRequest { get; set; }
    }

    public class CreateCustomerSurveyCommandHandler(ICustomerSurveyService CustomerService) : IRequestHandler<CreateCustomerSurveyCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateCustomerSurveyCommand request, CancellationToken cancellationToken)
        {

            var CustomerId = await CustomerService.CreateCustomerSurveyAsync(request.CustomerSurveyRequest.Adapt<CustomerSatisfactionSurvey>());

            return await ResponseWrapper<Guid>.SuccessAsync(data: CustomerId, message: "Record saved successfully.");
        }
    }
}

