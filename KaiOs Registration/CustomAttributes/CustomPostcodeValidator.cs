using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using KaiOs_Registration.Helpers;
namespace KaiOs_Registration.CustomAttributes
{
    public class CustomPostcodeValidator : ValidationAttribute
    {
        public string Country { get; set; }

        public CustomPostcodeValidator(string country)
        {
            Country = country;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string postcode = value.ToString();
                var country = validationContext.ObjectType.GetProperty(Country).GetValue(validationContext.ObjectInstance, null);

                if (country == null)
                {
                    return new ValidationResult("Country is required");
                }

                if (ApiValidator.IsPostCodeValid(postcode.TrimEnd(), (country.ToString() == "United States" ? "US" : "UK")   ))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Postcode is Invalid");
                }
            }
            else
            {
                return new ValidationResult("" + validationContext.DisplayName + " is required");
            }
        }
    }
}