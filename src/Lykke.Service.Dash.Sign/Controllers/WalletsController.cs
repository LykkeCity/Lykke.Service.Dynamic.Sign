using Lykke.Service.Dash.Sign.Core.Services;
using Lykke.Service.Dash.Sign.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Lykke.Service.Dash.Sign.Controllers
{
    [Route("api/wallets")]
    public class WalletsController : Controller
    {
        private readonly IDashService _dashService;

        public WalletsController(IDashService dashService)
        {
            _dashService = dashService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(WalletResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Post()
        {
            var privateKey = _dashService.GetPrivateKey();
            var publicAddress = _dashService.GetPublicAddress(privateKey);

            return Ok(new WalletResponse()
            {
                PrivateKey = privateKey,
                PublicAddress = publicAddress
            });
        }
    }
}
