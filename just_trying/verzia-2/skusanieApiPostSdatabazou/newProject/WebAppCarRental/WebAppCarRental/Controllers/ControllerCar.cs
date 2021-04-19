using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCarRental.Data;

namespace WebAppCarRental.Controllers
{
    [ApiController]
    [Route("car")]
    public class ControllerCar : ControllerBase
    {
        //https://localhost:44353/car/model?BrandOfCar=Toyota
        [HttpGet]
        [Route("model")]
        public IActionResult makeFilter([FromQuery(Name = "BrandOfCar")] string brandOfCar)
        {
            List<string> list = new List<string>();
            ContosoCarReservationContext contosoCarReservationContext = new ContosoCarReservationContext();
            foreach(var row in contosoCarReservationContext.Cars)
            {
                if(row.BrandOfCar == brandOfCar)
                {
                    list.Add(row.Model);
                }
            }
            if (list.Count >= 1)
            {
                return Ok(list);
            }
            else
            {
                return BadRequest("Our Car rental does not have that car");
            }

        }
    }
}
