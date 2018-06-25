using NBitcoin;

namespace Lykke.Service.Dynamic.Sign.Core.Services
{
    public interface IDynamicService
    {
        bool IsValidPrivateKey(string privateKey);
        string GetPrivateKey();
        string GetPublicAddress(string privateKey);
        string SignTransaction(Transaction tx, ICoin[] coins, Key[] keys);
    }
}
