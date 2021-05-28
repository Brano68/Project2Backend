using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.Models
{
    public class InformationAboutCustomer
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public string UserFname { get; set; }

        [Required]
        public string UserLname { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Adress { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string DriverLicenceNumber { get; set; }

        public int IdNumber { get; set; }

    }
}
