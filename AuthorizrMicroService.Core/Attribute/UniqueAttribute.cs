using System.ComponentModel.DataAnnotations;

namespace UserMicroService.Core.Attribute
{
    public class UniqueAttribute : ValidationAttribute
    {
        private readonly string _propertyName;

        public UniqueAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        // Some Magic)))
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (UserMicroServiceContext)validationContext.GetService(typeof(UserMicroServiceContext));
            if (dbContext == null)
            {
                throw new InvalidOperationException("DbContext not found in validation context");
            }

            var entityType = validationContext.ObjectType;
            var property = entityType.GetProperty(_propertyName);

            if (property == null)
            {
                return new ValidationResult($"Property '{_propertyName}' not found on type '{entityType.Name}'");
            }

            var entitySet = dbContext.GetType().GetMethod("Set", new Type[] { }).MakeGenericMethod(entityType).Invoke(dbContext, null);

            var existingEntity = ((IQueryable<object>)entitySet)
                .AsQueryable()
                .FirstOrDefault(e => property.GetValue(e).Equals(value));

            if (existingEntity != null)
            {
                return new ValidationResult($"{_propertyName} must be unique.");
            }

            return ValidationResult.Success;
        }
    }
}
