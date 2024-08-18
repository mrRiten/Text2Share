using System.ComponentModel.DataAnnotations;

namespace AuthorizeMicroService.Core.Attributes
{
    public class UniqueUserNameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string userName)
            {
                return new ValidationResult("Value must be string");
            }

            if (validationContext.GetService(typeof(AuthorizeMicroServiceContext)) is not AuthorizeMicroServiceContext dbContext)
            {
                throw new InvalidOperationException("Can`t get dbContext");
            }

            bool userExists = dbContext.Users.Any(u => u.UserName == userName);

            return userExists
                ? new ValidationResult("A user with that name already exists.")
                : ValidationResult.Success;
        }
    }
}
