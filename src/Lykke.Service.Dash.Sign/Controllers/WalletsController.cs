using Lykke.Service.Dynamic.Sign.Core.Services;
using Lykke.Service.Dynamic.Sign.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Lykke.Service.Dynamic.Sign.Controllers
{
    [Route("api/wallets")]
    public class WalletsController : Controller
    {
        private readonly IDynamicService _dynamicService;

        public WalletsController(IDynamicService dynamicService)
        {
            _dynamicService = dynamicService;
        }

        [HttpPost]
        public WalletResponse Post()
        {
            var privateKey = _dynamicService.GetPrivateKey();
            var publicAddress = _dynamicService.GetPublicAddress(privateKey);

            return new WalletResponse()
            {
                PrivateKey = privateKey,
                PublicAddress = publicAddress
            };
        }
    }
}
