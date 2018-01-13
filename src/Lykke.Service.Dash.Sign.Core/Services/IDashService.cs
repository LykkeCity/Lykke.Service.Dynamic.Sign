using NBitcoin;

namespace Lykke.Service.Dash.Sign.Core.Services
{
    public interface IDashService
    {
        bool IsValidPrivateKey(string privateKey);
        string GetPrivateKey();
        string GetPublicAddress(string privateKey);
        string SignTransaction(Transaction tx, ICoin[] coins, Key[] keys);
    }
}
