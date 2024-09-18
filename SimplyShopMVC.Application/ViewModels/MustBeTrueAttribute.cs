using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.ViewModels;

public class MustBeTrueAttribute: ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is bool boolValue && boolValue)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Pole musi być prawdą.");
    }
}
