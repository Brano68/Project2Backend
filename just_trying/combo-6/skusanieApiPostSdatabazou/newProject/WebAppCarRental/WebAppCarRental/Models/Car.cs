using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string BrandOfCar { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Plate { get; set; }
        //public string Reservations { get; set; }
        [Required]
        public double Price { get; set; }

        //upgrade databazy
        [Required]
        public string Fuel { get; set; }

        [Required]
        public double Consumption { get; set; }

        [Required]
        public int NumberOfPassenger { get; set; }

        [Required]
        public string Gear { get; set; }

        [Required]
        public bool AirConditioner { get; set; }

    }
}
