using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace Moq
{
    public static class ValidatorExtensions
    {
        public static async Task SetValidatorSuccess<T>(this Mock<IValidator<T>> validator)
        {
            validator.Setup(v => v.ValidateAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((new ValidationResult()));
        }

        public static async Task SetValidatorFailure<T>(this Mock<IValidator<T>> validator,
            string errorMessage)
        {
            validator.Setup(v => v.ValidateAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>()
                {
                    new ValidationFailure(string.Empty,
                        errorMessage)
                }));
        }
    }
}