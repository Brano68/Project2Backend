using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.DTO
{
    public class UserLogoutDTO
    {
        [JsonRequired]
        public string Login { get; set; }

        [JsonRequired]
        public string Token { get; set; }
    }
}
