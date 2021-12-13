using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AspStore.Models.CustomValidators
{
    public class SlugValidator: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var data = (string)value;
            if (string.IsNullOrEmpty(data))
            {
                return new ValidationResult("Invalid slug");
            }
            var regularExpression = "^[a-z0-9]+(?:-[a-z0-9]+)*$";
            var res = Regex.Match(data, regularExpression);
            if (res.Success)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Invalid Slug");
            }
        }
    }
}
