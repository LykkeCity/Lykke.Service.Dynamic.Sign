using Common.Log;
using Flurl.Http;
using Lykke.Service.Dash.Sign.Core.Domain;
using Lykke.Service.Dash.Sign.Core.Services;
using Lykke.Service.Dash.Sign.Services.Helpers;
using NBitcoin;
using NBitcoin.Dash;
using NBitcoin.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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

        public bool IsValidPublicAddress(string address)
        {
            try
            {
                BitcoinAddress.Create(address, _network);
            }
            catch
            {
                return false;
            }

            return true;
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
                Key.Parse(privateKey, _network);
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

            return wallet.PubKey.GetAddress(_network).ToString();
        }

        public async Task<string> GetSignedTransactionHex(string from, string to, ulong amount, 
            IEnumerable<string> privateKeys)
        {
            var sendFrom = BitcoinAddress.Create(from);
            var sendTo = BitcoinAddress.Create(to);
            var sendAmount = new Money(amount, MoneyUnit.Satoshi);
            var fee = new Money(0.001m, MoneyUnit.BTC);
            var secrets = privateKeys.Select((f => new BitcoinSecret(f).PrivateKey)).ToArray();

            var secret = secrets.FirstOrDefault(f => f.PubKey.GetAddress(_network).ToString() == from);
            if (secret == null)
            {
                throw new Exception("Private key was not found for provided from address");
            }

            var coins = await GetUnspentCoins(from);

            var txBuilder = new TransactionBuilder();
            var tx = txBuilder
                .AddCoins(coins)
                .AddKeys(secret)
                .SendFees(fee)
                .Send(sendTo, sendAmount)
                .SetChange(sendFrom)
                .BuildTransaction(true);

            if (!txBuilder.Verify(tx, out TransactionPolicyError[] errors))
            {
                var error = String.Join(";", errors.Select(f => f.ToString()));

                throw new Exception($"Failed to build signed tx: {error}");
            }

            return tx.ToHex();
        }

        private async Task<Coin[]> GetUnspentCoins(string address)
        {
            var coins = new List<Coin>();

            var txsUnspent = await Retry.Try(() => GetTxsUnspent(address),
                ex => ex is FlurlHttpException,
                tryCount: 10,
                logger: _log,
                delayAfterException: 3);

            if (txsUnspent == null)
            {
                throw new Exception("There are no unspent coins (utxo) in from address");
            }

            foreach (var txUnspent in txsUnspent)
            {
                var txHex = await Retry.Try(() => GetTxHex(txUnspent.Txid),
                    ex => ex is FlurlHttpException,
                    tryCount: 10,
                    logger: _log,
                    delayAfterException: 3);

                var tx = Transaction.Parse(txHex.Rawtx);
                var coin = tx.Outputs.AsIndexedOutputs().ToArray()[txUnspent.Vout].ToCoin();

                coins.Add(coin);
            }

            return coins.ToArray();
        }

        private async Task<TxUnspent[]> GetTxsUnspent(string address)
        {
            try
            {
                return await $"http://52.178.39.182:3001/insight-api-dash/addr/{address}/utxo"
                    .GetJsonAsync<TxUnspent[]>();
            }
            catch (FlurlHttpException e) when (e.Call.Response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        private async Task<TxHex> GetTxHex(string txId)
        {
            try
            {
                return await $"http://52.178.39.182:3001/insight-api-dash/rawtx/{txId}"
                    .GetJsonAsync<TxHex>();
            }
            catch (FlurlHttpException e) when (e.Call.Response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}
