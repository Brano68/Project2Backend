using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRequests.CreateJSON
{
    public class Message
    {
        public string InfoMessage { get; set; }
        public int StatusCode { get; set; }

        public Message(string yourMessage)
        {
            InfoMessage = yourMessage;
        }

        public Message(string yourMessage, int statusCode)
        {
            InfoMessage = yourMessage;
            StatusCode = statusCode;
        }
    }
}
