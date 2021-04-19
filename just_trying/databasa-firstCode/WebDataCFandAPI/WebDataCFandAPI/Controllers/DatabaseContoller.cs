using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDataCFandAPI.Controllers
{
    [ApiController]
    [Route("database")]
    public class DatabaseContoller : ControllerBase
    {
        //database/addCar
        [Route("addCar")]
        public IActionResult Get()
        {
            ///zavolanim tejto metody pridam do databazy auto
            ///
            
            using Data.CarRentalContext carRentalContext = new Data.CarRentalContext();
            Models.Car car = new Models.Car()
            {
                name = "Toyota",
                Plate = "KK333ZZ"
            };
            carRentalContext.Add(car);
            carRentalContext.SaveChanges();
            
            /*
            Models.Car car = new Models.Car()
            {
                name = "Toyo",
                Plate = "KK111MM"
            };
            */

            return Ok(car);
        }

        //metoda na vratenie vsetkych
        //database/allCar
        [Route("allCar")]
        public IActionResult GetAll()
        {

            using Data.CarRentalContext carRentalContext = new Data.CarRentalContext();
            List<Models.Car> list = new List<Models.Car>();
            foreach (var row in carRentalContext.Cars)
            {
                list.Add(row);
            }
            
            //return Ok(carRentalContext);
            return Ok(list);
        }
    }
}
