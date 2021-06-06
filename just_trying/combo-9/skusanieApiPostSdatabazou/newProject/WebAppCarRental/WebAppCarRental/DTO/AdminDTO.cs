using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.DTO
{
    public class AdminDTO
    {
        [JsonRequired]
        public string LoginWhoIsCreating { get; set; }

        [JsonRequired]
        public string Login { get; set; }

        [JsonRequired]
        public string Password { get; set; }
        [JsonRequired]
        public string Email { get; set; }
        [JsonRequired]
        public string GlobalPassword { get; set; }
    }
}
