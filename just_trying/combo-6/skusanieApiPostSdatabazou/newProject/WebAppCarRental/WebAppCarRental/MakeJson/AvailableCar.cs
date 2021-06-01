using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeJson
{
    public class AvailableCar
    {
        public string BrandOfCar { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        public double Price { get; set; }

        //po update databazy
        
        public string Fuel { get; set; }

        
        public double Consumption { get; set; }

        
        public int NumberOfPassenger { get; set; }

        
        public string Gear { get; set; }

        
        public bool AirConditioner { get; set; }

        public AvailableCar(string brandOfCar, string model, string plate, double price,
            string fuel, double consumption, int numberOfPassenger, string gear, bool airConditioner)
        {
            BrandOfCar = brandOfCar;
            Model = model;
            Plate = plate;
            Price = price;
            Fuel = fuel;
            Consumption = consumption;
            NumberOfPassenger = numberOfPassenger;
            Gear = gear;
            AirConditioner = airConditioner;
        }
    }
}
