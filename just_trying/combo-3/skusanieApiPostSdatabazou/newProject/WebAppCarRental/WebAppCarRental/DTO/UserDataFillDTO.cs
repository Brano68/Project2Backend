using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.DTO
{
    public class UserDataFillDTO
    {
        [JsonRequired]
        public string Login { get; set; }

        [JsonRequired]
        public string UserFname { get; set; }

        [JsonRequired]
        public string UserLname { get; set; }

        [JsonRequired]
        public string PhoneNumber { get; set; }

        [JsonRequired]
        public string State { get; set; }

        [JsonRequired]
        public string City { get; set; }

        [JsonRequired]
        public string Adress { get; set; }

        [JsonRequired]
        public string PostCode { get; set; }

        [JsonRequired]
        public string DriverLicenceNumber { get; set; }

        [JsonRequired]
        public int IdNumber { get; set; }
    }
}
