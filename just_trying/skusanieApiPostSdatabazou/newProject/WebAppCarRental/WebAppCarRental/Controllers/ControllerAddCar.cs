using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCarRental.DTO;
using WebAppCarRental.Models;

namespace WebAppCarRental.Controllers
{
    [ApiController]
    [Route("car")]
    public class ControllerAddCar : ControllerBase
    {

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<CarDTO>> PostCar([FromBody] CarDTO carDTO)
        {
            //tu sa este osetri ze iba Admin moze pridat auto
            string brandOfCar = carDTO.BrandOfCar;
            string plate = carDTO.Plate;
            string model = carDTO.Model;
            if(brandOfCar != null && plate != null && model != null 
                && brandOfCar != "" && plate != "" && model != "")
            {
                using Data.ContosoCarContext contosoCarContext = new Data.ContosoCarContext();
                Car car = new Car()
                {
                    BrandOfCar = brandOfCar,
                    Model = model,
                    Plate = plate,
                };
                contosoCarContext.Add(car);
                contosoCarContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}
