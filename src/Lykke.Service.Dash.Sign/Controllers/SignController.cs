using Lykke.Service.Dash.Sign.Core.Services;
using Lykke.Service.Dash.Sign.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.Service.Dash.Sign.Controllers
{
    [Route("api/sign")]
    public class SignController : Controller
    {
        private readonly IDashService _dashService;

        public SignController(IDashService dashService)
        {
            _dashService = dashService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SignResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody]SignRequest signRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create("ValidationError", ModelState));
            }

            SignContextModel context;
            try
            {
                context = JsonConvert.DeserializeObject<SignContextModel>(signRequest.TransactionHex);
            }
            catch(Exception ex)
            {
                return BadRequest(ErrorResponse.Create($"Failed to deserialize transactionHex: {ex.Message}"));
            }
            if (!_dashService.IsValidPublicAddress(context.From))
            {
                return BadRequest(ErrorResponse.Create("context.From is not a valid"));
            }
            if (!_dashService.IsValidPublicAddress(context.To))
            {
                return BadRequest(ErrorResponse.Create("context.To is not a valid"));
            }
            if (context.Amount <= 0)
            {
                return BadRequest(ErrorResponse.Create("context.Amount can not be 0 or less"));
            }

            var txHex = await _dashService.GetSignedTransactionHex(
                from: context.From,
                to: context.To,
                amount: context.Amount,
                privateKeys: signRequest.PrivateKeys);

            return Ok(new SignResponse()
            {
                SignedTransaction = txHex
            });
        }
    }
}
