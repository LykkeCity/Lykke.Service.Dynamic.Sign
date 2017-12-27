using System;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Dash.Sign.Utils
{
    public static class Extensions
    {
        public static string DisplayNameInCamelCase(this ValidationContext context)
        {
            return $"{context.DisplayName.Substring(0, 1).ToLower()}{context.DisplayName.Substring(1)}";
        }
    }
}
