using System.ComponentModel.DataAnnotations;

namespace UserMicroService.Core.Attributes
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var dbContext = (UserMicroServiceContext)validationContext.GetService(typeof(UserMicroServiceContext))
                ?? throw new InvalidOperationException("DbContext не может быть определен.");

            var email = (string)value;

            var user = dbContext.Users.FirstOrDefault(u => u.UserEmail == email);

            if (user != null)
            {
                return new ValidationResult("Пользователь с этой почтой уже существует.");
            }

            return ValidationResult.Success;
        }
    }
}
