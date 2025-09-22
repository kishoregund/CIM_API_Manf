using Application.Features.Customers.Responses;
using Application.Features.Customers.Queries;
using Application.Features.Customers;

namespace Application.Features.Customers.Queries
{
    public class GetCustomerSurveyByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid SurveyId { get; set; }
    }

    public class GetCustomerSurveyByIdQueryHandler(ICustomerSurveyService CustomerService) : IRequestHandler<GetCustomerSurveyByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustomerSurveyByIdQuery request, CancellationToken cancellationToken)
        {
            var customerInDb = await CustomerService.GetCustomerSurveyAsync(request.SurveyId);

            if (customerInDb is not null)
            {
                return await ResponseWrapper<CustomerSatisfactionSurvey>.SuccessAsync(data: customerInDb);
            }
            return await ResponseWrapper<CustomerSatisfactionSurvey>.SuccessAsync(message: "Customer Survey does not exists.");
        }
    }
}
