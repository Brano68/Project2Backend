using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.DTO
{
    public class UserLoginDTO
    {
        [JsonRequired]
        public string Login { get; set; }

        [JsonRequired]
        public string Password { get; set; }
    }
}
