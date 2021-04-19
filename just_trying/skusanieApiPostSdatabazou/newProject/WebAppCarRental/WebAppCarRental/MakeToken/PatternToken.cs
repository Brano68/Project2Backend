using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeToken
{
    public class PatternToken
    {
        public string Token { get; private set; }
        public string CreatedDateTime { get; private set; }

        public PatternToken(string token, string createdDateTime)
        {
            Token = token;
            CreatedDateTime = createdDateTime;
        }

       
        
    }
}
