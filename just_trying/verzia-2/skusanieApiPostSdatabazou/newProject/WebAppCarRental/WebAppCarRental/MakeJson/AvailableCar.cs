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

        public AvailableCar(string brandOfCar, string model, string plate)
        {
            BrandOfCar = brandOfCar;
            Model = model;
            Plate = plate;
        }
    }
}
