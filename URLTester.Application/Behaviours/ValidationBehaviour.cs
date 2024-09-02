using FluentValidation;
using FluentValidation.Results;
using MediatR;
using URLTester.Application.Exceptions;
using URLTester.Resources;

namespace URLTester.Application.Behaviours;

//Validation Behaviour It uses a list of validators to validate the incoming request before passing it to the handler. 
public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
    {
        if (validators.Any())
        {
			//used to provide the current request to the validators
			var context = new ValidationContext<TRequest>(request);

            var validationResult = await validators.First().ValidateAsync(context, cancellationToken);

            if (!validationResult.IsValid)
            {
                var failures = Serialize(validationResult.Errors);
                throw new BadRequestException(Messages.BadRequest, failures);
            }
        }
        return await next();
    }

    private static Dictionary<string, string[]> Serialize(IEnumerable<ValidationFailure> failures)
    {
        var camelCaseFailures = failures
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).ToArray()
            );

        return camelCaseFailures;
    }
}
