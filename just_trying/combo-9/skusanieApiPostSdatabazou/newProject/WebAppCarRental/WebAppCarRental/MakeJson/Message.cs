using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeJson
{
    public class Message
    {
        public string YourMessage { get; set; }
        public int StatusCode { get; set; }

        public Message(string yourMessage)
        {
            YourMessage = yourMessage;
        }

        public Message(string yourMessage, int statusCode)
        {
            YourMessage = yourMessage;
            StatusCode = statusCode;
        }
    }
}
