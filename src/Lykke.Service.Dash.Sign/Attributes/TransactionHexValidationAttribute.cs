using Lykke.Service.Dash.Sign.Core.Services;
using Lykke.Service.Dash.Sign.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Dash.Sign.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TransactionHexValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var transactionHex = value as string;
            var dashService = (IDashService)validationContext.GetService(typeof(IDashService));

            if (!dashService.IsValidTransactionHex(transactionHex))
            {
                return new ValidationResult($"{validationContext.DisplayNameInCamelCase()} is not a valid");
            }

            return null;
        }
    }
}
