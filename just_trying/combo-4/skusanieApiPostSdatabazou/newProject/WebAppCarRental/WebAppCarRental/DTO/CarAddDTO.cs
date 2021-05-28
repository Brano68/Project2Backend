using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.DTO
{
    public class CarAddDTO
    {
        [JsonRequired]
        public string BrandOfCar { get; set; }

        [JsonRequired]
        public string Model { get; set; }

        [JsonRequired]
        public string Plate { get; set; }

        [JsonRequired]
        public string Login { get; set; }

        [JsonRequired]
        public double Price { get; set; }
    }
}
