using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRequests.CreateJSON
{
    public class PayRequest
    {
        public string Info { get; set; }
        public int Price { get; set; }
        public int StatusCode { get; set; }

        public int CarId { get; set; }

        public int UserId { get; set; }
        public string Login { get; set; }

        public string From { get; set; }
        public string To { get; set; }

        public PayRequest(string info, int price, int statusCode, int carId, int userId, string login, string from, string to)
        {
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
