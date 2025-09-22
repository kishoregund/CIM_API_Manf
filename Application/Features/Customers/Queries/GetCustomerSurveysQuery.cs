using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetCustomerSurveysQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetCustomerSurveysQueryHandler(ICustomerSurveyService CustomerService) : IRequestHandler<GetCustomerSurveysQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustomerSurveysQuery request, CancellationToken cancellationToken)
        {
            var CustomersInDb = (await CustomerService.GetCustomerSurveysAsync());

            if (CustomersInDb.Count > 0)
            {
                return await ResponseWrapper<List<CustomerSurveyResponse>>.SuccessAsync(data: CustomersInDb);
            }
            return await ResponseWrapper<CustomerSurveyResponse>.SuccessAsync(message: "Customer Surveys were found.");
        }
    }
}

