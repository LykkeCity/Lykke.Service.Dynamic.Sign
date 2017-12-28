using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Dash.Sign.Core.Services
{
    public interface IDashService
    {
        bool IsValidPublicAddress(string address);
        bool IsValidPrivateKey(string privateKey);
        bool IsValidTransactionHex(string transactionHex);
        string GetPrivateKey();
        string GetPublicAddress(string privateKey);
        Task<string> GetSignedTransactionHex(string from, string to, 
            ulong amount, IEnumerable<string> privateKeys);
    }
}
