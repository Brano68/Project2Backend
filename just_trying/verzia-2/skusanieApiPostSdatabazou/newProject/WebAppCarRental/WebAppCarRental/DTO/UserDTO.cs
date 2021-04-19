using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebAppCarRental.DTO
{
    public class UserDTO
    {
        [JsonRequired]
        public string Login { get; set; }

        [JsonRequired]
        public string Password { get; set; }
        [JsonRequired]
        public string Email { get; set; }
    }
}
