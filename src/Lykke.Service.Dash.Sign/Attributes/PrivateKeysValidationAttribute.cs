using Lykke.Service.Dash.Sign.Core.Services;
using Lykke.Service.Dash.Sign.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Lykke.Service.Dash.Sign.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PrivateKeysValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var keys = value as IEnumerable<string>;
            var dashService = (IDashService)validationContext.GetService(typeof(IDashService));

            if (!keys.Any())
            {
                return new ValidationResult($"{validationContext.DisplayNameInCamelCase()} array can not be empty");
            }

            var num = 0;
            foreach (var key in keys)
            {
                if (!dashService.IsValidPrivateKey(key))
                {
                    return new ValidationResult($"{validationContext.DisplayNameInCamelCase()}[{num}] is not a valid");
                }

                num++;
            }

            return null;
        } 
    }
}
