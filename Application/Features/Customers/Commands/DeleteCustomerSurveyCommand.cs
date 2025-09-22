namespace Application.Features.Customers.Commands
{
    public class DeleteCustomerSurveyCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SurveyId { get; set; }
    }

    public class DeleteCustomerSurveyCommandHandler(ICustomerSurveyService CustomerService) : IRequestHandler<DeleteCustomerSurveyCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteCustomerSurveyCommand request, CancellationToken cancellationToken)
        {
           
                var deletedCustomer = await CustomerService.DeleteCustomerSurveyAsync(request.SurveyId);

                return await ResponseWrapper<bool>.SuccessAsync(data: deletedCustomer, message: "Customer Survey deleted successfully.");
           
        }
    }
}
