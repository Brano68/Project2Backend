using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Security.Claims;


namespace WebAppCarRental.Controllers
{
    [ApiController]
    [Route("userrr")]
    public class ControllerCard : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("pay")]
        public async Task<ActionResult<DTO.CardDTO>> PostCard([FromBody] DTO.CardDTO cardDTO)
        {
            string login = cardDTO.Login;
            string from = cardDTO.From;
            string to = cardDTO.To;
            string cardNumber = cardDTO.CardNumber;
            string cardName = cardDTO.CardName;
            string expirationDate = cardDTO.ExpirationDate;
            int cvc = cardDTO.Cvc;
            int price = cardDTO.Price;

            if (cardNumber == null || cardName == null || expirationDate == null || cvc == 0 || price == 0)
            {
                MakeJson.Message messageError = new MakeJson.Message("No input data", 400);
                return BadRequest(messageError);
            }

            CreditCard.Check check = new CreditCard.Check();
            int n = check.Checking(cardNumber, cardName, expirationDate, cvc, price);
            if (n == -1)
            {
                MakeJson.Message message1 = new MakeJson.Message("The card is invalid", 400);
                return BadRequest(message1);
            }
            if (n == 0)
            {
                MakeJson.Message message2 = new MakeJson.Message("Not enough money in the account", 400);
                return BadRequest(message2);
            }
            if (n == 1)
            {
                MakeJson.Message message3 = new MakeJson.Message("Payment was successful", 200);
                return Ok(message3);
            }
            MakeJson.Message message = new MakeJson.Message("The card is invalid", 400);
            return BadRequest(message);
        }
    }
}
