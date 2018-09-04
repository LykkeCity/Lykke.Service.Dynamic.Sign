using Common.Log;
using Lykke.Service.Dynamic.Sign.Services;
using NBitcoin;
using NBitcoin.Dynamic;
using NBitcoin.Policy;
using NBitcoin.JsonConverters;
using Newtonsoft.Json;
using System.Linq;
using Xunit;
using Lykke.Service.Dynamic.Sign.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Moq;
using Lykke.Service.Dynamic.Sign.Core.Services;

namespace Lykke.Service.Dynamic.Sign.Tests
{
    public class DynamicServiceTests
    {
        private ILog _log;
        public Network network = DynamicNetworks.Testnet;
        public string from = "DQ5H7o7WcP9gT3VLZZQW3qeq2r1mmy6SLB";
        public string fromPrivateKey = "Mj9qy3cFENVjikdvGz8TGi9Xrq7ypNRwSTL19JeZhmyJsswN53zL";
        public BitcoinAddress fromAddress;
        public Key fromKey;
        public string to = "D9JitGHNd99zzxhos5qx7ox1kZFikGTyFc";
        public BitcoinAddress toAddress;
        public Transaction prevTx = Transaction.Parse("0100000001e2f23b2d1580d765bd969d1889d4700076688d3219f5686ac21573757f55fff50000000000ffffffff02b0ebffea0f0000001976a914daa46815060c0372118e52ccf970c4c54031055b88ac00e1f505000000001976a9146a4c6d1473b8a5bb8cfea0319d8dac1bc24e147088ac00000000");
        public TransactionBuilder txBuilder = new TransactionBuilder();
        public Transaction tx;
        public ICoin[] spentCoins;
        public DynamicService service;
        public Mock<IServiceProvider> serviceProvider;

        public DynamicServiceTests()
        {
            _log = new LogToMemory();

            fromAddress = new BitcoinPubKeyAddress(from);
            fromKey = Key.Parse(fromPrivateKey);
            toAddress = new BitcoinPubKeyAddress(to);
            tx = txBuilder
                .AddCoins(prevTx.Outputs.AsCoins().Where(c => c.ScriptPubKey.GetDestinationAddress(network).ToString() == from).ToArray())
                .Send(toAddress, Money.Coins(1))
                .SetChange(fromAddress)
                .SubtractFees()
                .SendFees(txBuilder.EstimateFees(new FeeRate(Money.Satoshis(1024))))
                .BuildTransaction(false);
            spentCoins = txBuilder.FindSpentCoins(tx);
            service = new DynamicService(_log, "dynamic-testnet");

            serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(provider => provider.GetService(typeof(IDynamicService)))
                .Returns(service);
        }

        [Fact]
        public void GetPrivateKeyShouldReturnData()
        {
            // Act
            var key = service.GetPrivateKey();

            // Assert
            Assert.Equal(52, key.Length);
        }

        [Fact]
        public void GetPublicAddressShouldReturnData()
        {
            // Arrange
            var key = service.GetPrivateKey();

            // Act
            var address = service.GetPublicAddress(key);

            // Assert
            Assert.Equal(34, address.Length);
        }

        [Fact]
        public void ShouldSignTransaction()
        {
            // Act
            var signedTransactionHex = service.SignTransaction(tx, spentCoins, new[] { fromKey });
            var signedTx = Transaction.Parse(signedTransactionHex);

            // Assert
            Assert.True(new TransactionBuilder()
                .AddCoins(spentCoins)
                .SetTransactionPolicy(new StandardTransactionPolicy { CheckFee = false })
                .Verify(signedTx, out var errors));
        }

        [Fact]
        public void ShouldSerializeDeserializeData()
        {
            // Arrange
            var body = JsonConvert.SerializeObject(new
            {
                PrivateKeys = new[] { this.fromPrivateKey },
                TransactionContext = Serializer.ToString((this.tx, this.spentCoins))
            });

            // Act
            var request = JsonConvert.DeserializeObject<SignTransactionRequest>(body);
            var validationResult = request.Validate(new ValidationContext(request, serviceProvider.Object, null));

            // Assert;
            Assert.Empty(validationResult);
        }

        [Fact]
        public void ShouldNotValidate_IfTxIsNull()
        {
            // Arrange
            var body = JsonConvert.SerializeObject(new
            {
                PrivateKeys = new[] { this.fromPrivateKey },
                TransactionContext = Serializer.ToString(((Transaction)null, this.spentCoins))
            });

            // Act
            var request = JsonConvert.DeserializeObject<SignTransactionRequest>(body);
            var validationResult = request.Validate(new ValidationContext(request, serviceProvider.Object, null));

            // Assert
            Assert.NotEmpty(validationResult);
            Assert.Contains(nameof(SignTransactionRequest.TransactionContext), validationResult.First().MemberNames);
        }

        [Fact]
        public void ShouldNotValidate_IfPrivateKeysArrayIsNull()
        {
            // Arrange
            var body = JsonConvert.SerializeObject(new
            {
                PrivateKeys = Array.Empty<string>(),
                TransactionHex = Serializer.ToString((this.tx, (ICoin[])null))
            });

            // Act
            var request = JsonConvert.DeserializeObject<SignTransactionRequest>(body);
            var validationResult = request.Validate(new ValidationContext(request, serviceProvider.Object, null));

            // Assert
            Assert.NotEmpty(validationResult);
            Assert.Contains(nameof(SignTransactionRequest.PrivateKeys), validationResult.First().MemberNames);
        }

        [Fact]
        public void ShouldNotValidate_IfKeyIsInvalid()
        {
            // Arrange
            var body = JsonConvert.SerializeObject(new
            {
                PrivateKeys = new[] { "invalid" },
                TransactionContext = Serializer.ToString((this.tx, this.spentCoins))
            });

            // Act
            var request = JsonConvert.DeserializeObject<SignTransactionRequest>(body);
            var validationResult = request.Validate(new ValidationContext(request, serviceProvider.Object, null));

            // Assert
            Assert.NotEmpty(validationResult);
            Assert.Contains(nameof(SignTransactionRequest.PrivateKeys), validationResult.First().MemberNames);
        }
    }
}
