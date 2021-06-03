using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRequests.DTO
{
    public class CardDTO
    {
        [JsonRequired]
        public string Login { get; set; }


        [JsonRequired]
        public string From { get; set; }

        [JsonRequired]
        public string To { get; set; }

        [JsonRequired]
        public string CardNumber { get; set; }

        [JsonRequired]
        public string CardName { get; set; }

        [JsonRequired]
        public string ExpirationDate { get; set; }

        [JsonRequired]
        public int Cvc { get; set; }

        [JsonRequired]
        public int Price { get; set; }

    }
}
