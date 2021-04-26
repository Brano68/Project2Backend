using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCarRental.Models;

namespace WebAppCarRental.Data
{
    public class ContosoCarReservationContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        public DbSet<Reservation> Reservations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AllCarsAndReservations");
        }
    }
}
