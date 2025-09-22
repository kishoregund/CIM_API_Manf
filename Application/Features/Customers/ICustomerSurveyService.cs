
using Application.Features.Customers.Responses;
using MediatR;

namespace Application.Features.Customers
{
    public interface ICustomerSurveyService
    {
        Task<CustomerSatisfactionSurvey> GetCustomerSurveyAsync(Guid id);
        Task<List<CustomerSurveyResponse>> GetCustomerSurveysAsync();
        Task<Guid> CreateCustomerSurveyAsync(CustomerSatisfactionSurvey CustomerSurvey);
        Task<Guid> UpdateCustomerSurveyAsync(CustomerSatisfactionSurvey CustomerSurvey);
        Task<bool> DeleteCustomerSurveyAsync(Guid id);
    }
}
