using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeJson
{
    public class DescribeCar
    {
        public List<AvailableCar> YourCars { get; set; }

        public DescribeCar(List<AvailableCar> yourCars)
        {
            YourCars = yourCars;
        }
    }
}
