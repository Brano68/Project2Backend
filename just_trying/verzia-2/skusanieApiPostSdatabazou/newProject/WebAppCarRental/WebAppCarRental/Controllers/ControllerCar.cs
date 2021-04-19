using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCarRental.Data;
using WebAppCarRental.MakeJson;

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
                ModelCars modelCars = new ModelCars(list);
                return Ok(modelCars.getJson());
                //return Ok(list);
            }
            else
            {
                //
                String message = "Our Car rental does not have that car";
                ModelErrorForFilter modelErrorForFilter = new ModelErrorForFilter(message);
                return BadRequest(modelErrorForFilter.getJson());
            }

        }
    }
}
