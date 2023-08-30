using FluentValidation;
using MediatR;
using TvJahnOrchesterApp.Application.Common.Errors;

namespace TvJahnOrchesterApp.Application.Common.Behaviors
{
    internal class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest>? _validator;

        // Auf diese Art kann optionale DI machen, wenn ein Service im DI Container gefunden wird der dem Interface IValidator<TRequest> entspricht, wird er hier eingebunden, ansonsten wird validator auf null gesetzt, aber es gibt keinen Fehler:
        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Code der vor dem Handler aufgerufen wird:
            if (_validator is null)
            {
                return await next();
            }
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid)
            {
                return await next(); // Hier wird der Handler des Commands aufgerufen.
            }

            var errorMessage = string.Join(", ", validationResult.Errors.Select(validationFailure => $"{validationFailure.PropertyName}: {validationFailure.ErrorMessage}"));

            throw new ServiceValidationException(errorMessage);
            
        }
    }
}
