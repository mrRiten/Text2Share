using System.ComponentModel.DataAnnotations;

namespace AuthorizeMicroService.Core.Attributes
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string email)
            {
                return new ValidationResult("Value must be string");
            }

            if (validationContext.GetService(typeof(AuthorizeMicroServiceContext)) is not AuthorizeMicroServiceContext dbContext)
            {
                throw new InvalidOperationException("Can`t get dbContext");
            }

            bool emailExists = dbContext.Users.Any(u => u.UserEmail == email);

            return emailExists
                ? new ValidationResult("A user with that email already exists.")
                : ValidationResult.Success;
        }
    }
}
