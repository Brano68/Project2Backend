using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeJson
{
    public class Order
    {
        public string BrandOfCar { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
        public double PricePerReservation { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public Order(string branOfCar, string model, string plate, double pricePerReservation, string from, string to)
        {
            BrandOfCar = branOfCar;
            Model = model;
            Plate = plate;
            PricePerReservation = pricePerReservation;
            From = from;
            To = to;
        }
    }
}
