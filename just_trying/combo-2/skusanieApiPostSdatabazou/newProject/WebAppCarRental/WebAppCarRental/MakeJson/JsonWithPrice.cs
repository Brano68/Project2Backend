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

        public int CarId { get; set; }

        public int UserId { get; set; }
        public string Login { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        


        public JsonWithPrice(string info, double price, int statusCode, int carId, int userId, string login, string from, string to) {
            Info = info;
            Price = price;
            StatusCode = statusCode;
            CarId = carId;
            UserId = userId;
            Login = login;
            From = from;
            To = to;
        }
    }
}
