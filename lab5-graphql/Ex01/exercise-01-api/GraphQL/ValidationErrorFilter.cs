using HotChocolate;
using System.Linq;

namespace exercise_01_api.GraphQL
{
    public class ValidationErrorFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            if (error.Exception is not FluentValidation.ValidationException validationException)
            {
                return error;
            }

            var errors = validationException.Errors
                .Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                })
                .ToList();

            return error.WithMessage("Validation failed.")
                .WithCode("VALIDATION_FAILED")
                .SetExtension("validationErrors", errors)
                .WithException(null);
        }
    }
}