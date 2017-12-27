namespace Lykke.Service.Dash.Sign.Core.Services
{
    public interface IDashService
    {
        bool IsValidPrivateKey(string privateKey);
        bool IsValidTransactionHex(string transactionHex);
        string GetPrivateKey();
        string GetPublicAddress(string privateKey);
    }
}
