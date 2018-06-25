using Lykke.Service.Dynamic.Sign.Core.Services;
using Lykke.Service.Dynamic.Sign.Models;
using Lykke.Service.Dynamic.Sign.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Lykke.Service.Dynamic.Sign.Controllers
{
    [Route("api/sign")]
    public class SignController : Controller
    {
        private readonly IDynamicService _dynamicService;

        public SignController(IDynamicService dynamicService)
        {
            _dynamicService = dynamicService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SignResponse), (int)HttpStatusCode.OK)]
        public IActionResult SignTransaction([FromBody]SignTransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToErrorResponse());
            }

            var hex = _dynamicService.SignTransaction(request.Tx, request.Coins, request.Keys);

            return Ok(new SignResponse()
            {
                SignedTransaction = hex
            });
        }        
    }
}
