using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeJson
{
    public class JsonWithPrice
    {
        public string Info { get; set; }
        public double Price { get; set; }
        public int StatusCode { get; set; }

        public JsonWithPrice(string info, double price, int statusCode) {
            Info = info;
            Price = price;
            StatusCode = statusCode;
        }
    }
}
