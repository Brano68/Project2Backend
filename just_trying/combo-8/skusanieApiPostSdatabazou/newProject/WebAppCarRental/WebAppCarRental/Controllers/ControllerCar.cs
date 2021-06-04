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
            foreach (var row in contosoCarReservationContext.Cars)
            {
                if (row.BrandOfCar == brandOfCar)
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

        //https://localhost:44353/car/models?From=18.10.2022&To=19.10.2022
        [HttpGet]
        [Route("models")]
        public IActionResult makeAvailable([FromQuery(Name = "From")] string from, [FromQuery(Name = "To")] string to)
        {
            /*
            DateTime dateFrom = DateTime.Parse(from);
            DateTime dateTo = DateTime.Parse(to);
            double days = (dateTo - dateFrom).TotalDays;
            */
            //prejst databazu Reservation
            //overit datumy
            List<int> list = selectCarsWhichAreTaken(from, to);
            List<AvailableCar> listOfAvailableCar = new List<AvailableCar>();

            //ak bude list prazdny znamena to ze vsetke auta su volne
            //cize vratime vsetke auta v jsone
            ContosoCarReservationContext contosoCarReservationContext = new ContosoCarReservationContext();
            if(list.Count == 0)
            {
                foreach (var row in contosoCarReservationContext.Cars)
                {
                    //po update v database nastala zmena v konstruktore
                    AvailableCar availableCar = new AvailableCar(row.BrandOfCar, row.Model, row.Plate, row.Price, row.Fuel, row.Consumption, row.NumberOfPassenger, row.Gear, row.AirConditioner, row.Path);
                    listOfAvailableCar.Add(availableCar);
                }
                DescribeCar describeCar = new DescribeCar(listOfAvailableCar);
                return Ok(describeCar);
            }
            else
            {
                //return Ok(list);
                
                foreach (var row in contosoCarReservationContext.Cars)
                {
                    //teraz overime idecka
                    if (list.Contains(row.Id))
                    {
                        continue;
                    }
                    else
                    {
                        AvailableCar availableCar = new AvailableCar(row.BrandOfCar, row.Model, row.Plate, row.Price, row.Fuel, row.Consumption, row.NumberOfPassenger, row.Gear, row.AirConditioner, row.Path);
                        listOfAvailableCar.Add(availableCar);
                    }
                }
                DescribeCar describeCar = new DescribeCar(listOfAvailableCar);
                return Ok(describeCar);
            }
        }

        //pomocna metoda
        private List<int> selectCarsWhichAreTaken(string dateFrom, string dateTo)
        {
            List<int> list = new List<int>();

            DateTime dateTimeFrom = DateTime.Parse(dateFrom);
            DateTime dateTimeTo = DateTime.Parse(dateTo);
            //ak datum vratenia je mensi ako pozicania vrati false
            if (dateTimeTo < dateTimeFrom)
            {
                return list;
            }

            //teraz prejde tabulku Reservations
            ContosoCarReservationContext contosoCarReservationContext = new ContosoCarReservationContext();
            foreach (var row in contosoCarReservationContext.Reservations)
            {
                DateTime dtFromDB = DateTime.Parse(row.From);
                DateTime dtToDB = DateTime.Parse(row.To);
                //ak zadany datum je vacsi ako dtToDb nech pokracuje
                if (dateTimeFrom > dtToDB)
                {
                    continue;
                }
                else if (dateTimeTo < dtFromDB)
                {
                    continue;
                }
                else
                {
                    list.Add(row.CarId);
                }
            }
            return list;
        }






        ////citanie zo subora
        //https://localhost:44353/car/subor
        [HttpGet]
        [Route("subor")]
        public IActionResult makeAvailablee()
        {
            string text = System.IO.File.ReadAllText(@"AdminPassword/PasswordForAdmin.txt");
            return Ok(text);
        }

        ////
    }


}