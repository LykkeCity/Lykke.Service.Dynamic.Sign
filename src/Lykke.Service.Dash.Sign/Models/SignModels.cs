using Lykke.Service.Dash.Sign.Attributes;
using Lykke.Service.Dash.Sign.Core.Services;
using Lykke.Service.Dash.Sign.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Lykke.Service.Dash.Sign.Models
{
    public class SignRequest
    {
        [Required]
        public string TransactionHex { get; set; }

        [Required]
        [PrivateKeysValidation]
        public IEnumerable<string> PrivateKeys { get; set; }
    }

    public class SignContextModel
    {
        public string From { get; set; }

        public string To { get; set; }

        public ulong Amount { get; set; }
    }

    public class SignResponse
    {
        public string SignedTransaction { get; set; }
    }
}
