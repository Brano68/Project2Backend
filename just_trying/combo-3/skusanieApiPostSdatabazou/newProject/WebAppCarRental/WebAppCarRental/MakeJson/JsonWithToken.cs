using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeJson
{
    public class JsonWithToken
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public int StatusCode { get; set; }
        public string Role { get; set; }
        public JsonWithToken(string token, int statusCode, string role, string message)
        {
            Message = message;
            Token = token;
            StatusCode = statusCode;
            Role = role;
        }

        public JsonWithToken(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
