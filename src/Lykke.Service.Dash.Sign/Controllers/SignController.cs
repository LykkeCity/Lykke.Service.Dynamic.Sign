using Lykke.Service.Dash.Sign.Core.Services;
using Lykke.Service.Dash.Sign.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public IActionResult Post([FromBody]SignRequest signRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create("ValidationError", ModelState));
            }

            return Ok(new SignResponse()
            {
                SignedTransaction = ""
            });
        }
    }
}
