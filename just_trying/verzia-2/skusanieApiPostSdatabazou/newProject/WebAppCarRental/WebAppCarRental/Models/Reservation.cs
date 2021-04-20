using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Car")]
        public int CarId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public string UserLogin { get; set; }

        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }

        public double PricePerDays { get; set; }
    }
}
