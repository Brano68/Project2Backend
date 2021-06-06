using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeJson
{
    public class ModelErrorForFilter
    {
        public String Message { get; set; }

        public ModelErrorForFilter(String message) {
            Message = message;
        }

        public String getJson()
        {
            string jsonData = JsonConvert.SerializeObject(this);
            return jsonData;
        }
    }
}
