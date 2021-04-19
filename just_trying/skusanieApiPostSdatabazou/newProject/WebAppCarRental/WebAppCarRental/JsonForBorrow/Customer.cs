using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.JsonForBorrow
{
    public class Customer
    {
        public string Login { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public Customer()
        {
            //Login = login;
            //From = from;
            //To = to;
        }
    }
}
