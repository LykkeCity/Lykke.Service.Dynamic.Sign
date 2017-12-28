using Common.Log;
using Lykke.Service.Dash.Sign.Services;
using NBitcoin;
using NBitcoin.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lykke.Service.Dash.Sign.Tests
{
    public class DashServiceTests
    {
        private ILog _log;

        private DashService Init()
        {
            _log = new LogToMemory();

            return new DashService(_log, "dash-testnet");
        }

        [Fact]
        public void GetPrivateKeyShouldReturnData()
        {
            // Arrange
            var svc = Init();

            // Act
            var key = svc.GetPrivateKey();

            // Assert
            Assert.Equal(52, key.Length);
        }

        [Fact]
        public void GetPublicAddressShouldReturnData()
        {
            // Arrange
            var svc = Init();
            var key = svc.GetPrivateKey();

            // Act
            var address = svc.GetPublicAddress(key);

            // Assert
            Assert.Equal(34, address.Length);
        }

        //[Fact]
        //public void SignTransactionShouldReturnData()
        //{
            // Arrange
            //var svc = Init();
            //var destinationkey = svc.GetPrivateKey();
            //var destinationAddress = BitcoinAddress.Create(svc.GetPublicAddress(destinationkey));
            //var sourceKey = "cV9nTtEJwgLe7pmSALvzrtQbjtP7zg8phhzDqgEURvWTEmSAVGjH";
            //var sourceAddress = BitcoinAddress.Create("ygFX7C2QGD5YQG6EE9wGFddTxqMdUwELuB");
            //var sourceWallet = new BitcoinSecret(sourceKey);

            //var txu = Transaction.Parse("0100000001af85294571caa1078ed31c022355b48b0679f6e6fbd824b0d3152ecd22edb8e8010000006b4830450221009979cba5db1538ca87a8a207494003e87eaeb5b29ebcfdc9c7cdccd108a89ecd02202bd9e4234abd698a2ba394ddf147029227195911fc971b5fcca78ed7715dd97a012103ddfa734b57b3b33b0e09de7426d2bed45b37093d52741c937e0f5193dc7aa1a1feffffff025053f7f00f0000001976a914daa46815060c0372118e52ccf970c4c54031055b88ac8a3a2497c20000001976a914db2a5c35c77d858150185661525190d451459b6388ac73bc0000");

            //var coin = new Coin(
            //    fromTxHash: new uint256("f5ff557f757315c26a68f519328d68760070d489189d96bd65d780152d3bf2e2"),
            //    fromOutputIndex: 0,
            //    amount: new Money(68467250000, MoneyUnit.Satoshi),
            //    scriptPubKey: new Script("76a914daa46815060c0372118e52ccf970c4c54031055b88ac"));

            //var coins = new List<Coin> {
            //    coin
            //};

            //var txBuilder = new TransactionBuilder();

            //var tx = txBuilder
            //    .AddCoins(txu)
            //    .AddKeys(sourceWallet.PrivateKey)
            //    .SendFees(new Money(0.001m, MoneyUnit.BTC))
            //    .Send(destinationAddress, new Money(1m, MoneyUnit.BTC))
            //    .SetChange(sourceAddress)
            //    .BuildTransaction(false);

            //if (!txBuilder.Verify(tx, out TransactionPolicyError[] errorsTmp))
            //{
            //    var msg = String.Join(";", errorsTmp.Select(f => f.ToString()));
            //}

            //var transactionHex = tx.ToHex();

            //// Act
            //var signedTransationHex = svc.SignTransaction(transactionHex, new string[] { sourceKey });
            ////var verificationResult = svc.VerifyTransaction(signedTransationHex);

            //var signedTx = Transaction.Parse(signedTransationHex);
            //if (!txBuilder.Verify(signedTx, out TransactionPolicyError[] errors))
            //{
            //    var msg = String.Join(";", errors.Select(f => f.ToString()));
            //}

            //// Assert
            //Assert.NotEmpty(signedTransationHex);
        //}
    }
}
