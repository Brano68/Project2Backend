using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeJson
{
    public class JsonWithToken
    {
        public string Token { get; set; }
        public int StatusCode { get; set; }
        public JsonWithToken(string token, int statusCode)
        {
            Token = token;
            StatusCode = statusCode;
        }
    }
}
