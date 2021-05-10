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
        public int CardNumber { get; set; }

        [JsonRequired]
        public int ExpirationDate { get; set; }

        [JsonRequired]
        public int Cvc { get; set; }

        [JsonRequired]
        public int Sum { get; set; }

    }
}
