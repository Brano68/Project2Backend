using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeToken
{
    public class Token
    { 

        private string createToken()
        {
            string TokenString = "";
            Random random = new Random();
            char sign;
            for (int i = 0; i < 25; i++) {
                int randomNumber = random.Next(3);
                switch (randomNumber)
                {
                    case (0):
                        sign = (char)(random.Next(57-48+1)+48);
                        TokenString += sign;
                        break;
                    case (1):
                        sign = (char)(random.Next(90 - 65 + 1) + 65);
                        TokenString += sign;
                        break;
                    case (2):
                        sign = (char)(random.Next(122 - 97 + 1) + 97);
                        TokenString += sign;
                        break;
                }  
            }
            return TokenString;
        }

        public string createTokenWithDateAndTimeAsJson()
        {
            //string dateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string dateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            PatternToken patternToken = new PatternToken(createToken(), dateTime);
            string jsonData = JsonConvert.SerializeObject(patternToken);
            return jsonData;
        }
    }
}
