using Lykke.Service.Dash.Sign.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Dash.Sign.Models
{
    public class SignRequest
    {
        [Required]
        [TransactionHexValidation]
        public string TransactionHex { get; set; }

        [Required]
        [PrivateKeysValidation]
        public IEnumerable<string> PrivateKeys { get; set; }
    }

    public class SignResponse
    {
        public string SignedTransaction { get; set; }
    }
}
