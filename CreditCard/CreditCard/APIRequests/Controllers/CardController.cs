using APIRequests.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Security.Claims;


namespace APIRequests.Controllers
{
    [ApiController]
    [Route("card")]
    public class CardController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("pay")]
        public async Task<ActionResult<CardDTO>> PostCard([FromBody] CardDTO cardDTO)
        {
            int cardNumber = cardDTO.CardNumber;
            int expirationDate = cardDTO.ExpirationDate;
            int cvc = cardDTO.Cvc;
            int sum = cardDTO.Sum;
            Console.WriteLine(cardNumber);

            return null;

        }
        
        [HttpGet]
        [Route("skuska")]
        public IActionResult PostTest([FromQuery(Name = "ILYA")] string ilya)
        {
            
            
            
        
            return Ok("Ahoj" + ilya);
        }

    }
}
