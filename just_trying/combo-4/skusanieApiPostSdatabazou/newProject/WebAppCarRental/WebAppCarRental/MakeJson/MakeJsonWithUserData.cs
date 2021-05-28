using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeJson
{
    public class MakeJsonWithUserData
    {
        public string Token { get; set; }
        public Message Message { get; set; }
        
        public string Role { get; set; }

        public string UserFname { get; set; }

        public string UserLname { get; set; }

        public string PhoneNumber { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Adress { get; set; }

        public string PostCode { get; set; }

        public string DriverLicenceNumber { get; set; }

        public int IdNumber { get; set; }

        public string Email { get; set; }

       public MakeJsonWithUserData(string token, Message message, string role, string userFname, string userLname, string phoneNumber,
           string state, string city, string adress, string postCode, string driverLicenceNumber, int idNumber,string email)
        {
            Token = token;
            Message = message;
            Role = role;

            UserFname = userFname;
            UserLname = userLname;
            PhoneNumber = phoneNumber;
            State = state;
            City = city;
            Adress = adress;
            PostCode = postCode;
            DriverLicenceNumber = driverLicenceNumber;
            IdNumber = idNumber;
            Email = email;
        } 
    }
}
