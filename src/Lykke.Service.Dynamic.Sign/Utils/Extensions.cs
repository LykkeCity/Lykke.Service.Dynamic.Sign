using Lykke.Common.Api.Contract.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace Lykke.Service.Dynamic.Sign.Utils
{
    public static class Extensions
    {
        public static ErrorResponse ToErrorResponse(this ModelStateDictionary modelState)
        {
            return new ErrorResponse()
            {
                ModelErrors = modelState.ToDictionary(
                    state => state.Key,
                    state => state.Value.Errors.Select(e => e.Exception?.Message ?? e.ErrorMessage).ToList()
                )
            };
        }
    }
}
