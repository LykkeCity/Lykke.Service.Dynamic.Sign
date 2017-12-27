using Common.Log;
using Lykke.Service.Dash.Sign.Core.Services;
using NBitcoin;
using NBitcoin.Dash;

namespace Lykke.Service.Dash.Sign.Services
{
    public class DashService : IDashService
    {
        private readonly ILog _log;
        private readonly Network _network;

        public DashService(ILog log,
            string network)
        {
            DashNetworks.Register();

            _log = log;
            _network = Network.GetNetwork(network);
        }

        public bool IsValidTransactionHex(string transactionHex)
        {
            try
            {
                Transaction.Parse(transactionHex);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool IsValidPrivateKey(string privateKey)
        {
            try
            {
                Key.Parse(privateKey);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public string GetPrivateKey()
        {
            var key = new Key();

            return key.GetWif(_network).ToString();
        }

        public string GetPublicAddress(string privateKey)
        {
            var wallet = new BitcoinSecret(privateKey);

            return wallet.GetAddress().ToString();
        }
    }
}
