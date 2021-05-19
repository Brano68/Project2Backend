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

            CheckingCard.Check.Check check = new CheckingCard.Check.Check();
            int n = check.Checking(cardNumber, expirationDate, cvc, price);
            if(n == -1)
            {
                Message message1 = new Message("The card is invalid", 400);
                return BadRequest(message1);
            }
            if (n == 0)
            {
                Message message2 = new Message("Not enough money in the account", 400);
                return BadRequest(message2);
            }
            if (n == 1)
            {
                PayRequest payRequest = new PayRequest("Payment was successful", price, 200, carId, userId, login, from, to);
                return Ok(payRequest);
            }
            Message message = new Message("The card is invalid", 400);
            return BadRequest(message);
        }
        
        [HttpGet]
        [Route("skuska")]
        public IActionResult PostTest([FromQuery(Name = "ILYA")] string ilya)
        {
            
            
            
        
            return Ok("Ahoj" + ilya);
        }

    }
}
