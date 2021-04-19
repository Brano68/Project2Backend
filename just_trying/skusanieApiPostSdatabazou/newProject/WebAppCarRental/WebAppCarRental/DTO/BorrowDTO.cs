using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.DTO
{
    public class BorrowDTO
    {
        [JsonRequired]
        public string Login { get; set; }

        [JsonRequired]
        public string Token { get; set; }

        [JsonRequired]
        public string Plate { get; set; }

        [JsonRequired]
        public string From { get; set; }

        [JsonRequired]
        public string To { get; set; }
    }
}
