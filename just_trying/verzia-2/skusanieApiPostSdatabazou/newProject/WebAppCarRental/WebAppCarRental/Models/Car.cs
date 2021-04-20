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
    }
}
