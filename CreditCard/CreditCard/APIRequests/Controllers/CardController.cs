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
using APIRequests.CreateJSON;

namespace APIRequests.Controllers
{
    [ApiController]
    [Route("userrr")]
    public class CardController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("pay")]
        public async Task<ActionResult<CardDTO>> PostCard([FromBody] CardDTO cardDTO)
        {
            string login = cardDTO.Login;
            string email = cardDTO.Email;
            int carId = cardDTO.CarId;
            int userId = cardDTO.UserId;
            string from = cardDTO.From;
            string to = cardDTO.To;
            string cardNumber = cardDTO.CardNumber;
            string expirationDate = cardDTO.ExpirationDate;
            int cvc = cardDTO.Cvc;
            int price = cardDTO.Price;

            Message message = new Message("Test", 400);
            return BadRequest(message);

            //return Ok("Údaje: " + login + " " + email + " " + carId + " " + userId + " " + from + " " + to + " " + cardNumber + " " + expirationDate + " " + cvc + " " + price);

        }
        
        [HttpGet]
        [Route("skuska")]
        public IActionResult PostTest([FromQuery(Name = "ILYA")] string ilya)
        {
            
            
            
        
            return Ok("Ahoj" + ilya);
        }

    }
}
