using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.DTO
{
    public class ReceiverDTO
    {
        [JsonRequired]
        public string Login { get; set; }
        [JsonRequired]
        public int CarID { get; set; }

        [JsonRequired]
        public int UserID { get; set; }

        [JsonRequired]
        public string From { get; set; }

        [JsonRequired]
        public string To { get; set; }

        [JsonRequired]
        public double PricePerDays { get; set; }

    }
}
