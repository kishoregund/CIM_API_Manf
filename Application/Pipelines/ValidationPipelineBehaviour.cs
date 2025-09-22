using FluentValidation;

namespace Application.Pipelines
{
    public class ValidationPipelineBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, IValidateMe
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!validators.Any()) return await next();
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task
                .WhenAll(validators.Select(vr => vr.ValidateAsync(context, cancellationToken)));

            if (validationResults.Any(vr => vr.IsValid))
                return await next();

            List<string> errors = [];

            var failures = validationResults.SelectMany(vr => vr.Errors)
                .Where(f => f != null)
                .ToList();

            foreach (var failure in failures)
            {
                errors.Add(failure.ErrorMessage);
            }

            return (TResponse)await ResponseWrapper.FailAsync(errors);

        }
    }
}