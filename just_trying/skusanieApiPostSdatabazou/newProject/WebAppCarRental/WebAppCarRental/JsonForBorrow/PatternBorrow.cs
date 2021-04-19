using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.JsonForBorrow
{
    public class PatternBorrow
    {
        public List<Customer> Borrows = new List<Customer>();

        public PatternBorrow(List<Customer> borrows)
        {
            Borrows = borrows;
        }
    }
}
