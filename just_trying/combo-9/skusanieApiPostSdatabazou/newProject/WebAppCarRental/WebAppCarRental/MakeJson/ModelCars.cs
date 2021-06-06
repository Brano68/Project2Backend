using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeJson
{
    public class ModelCars
    {
        public List<String> AllModels = new List<string>();

        public ModelCars(List<String> allModels)
        {
            AllModels = allModels;
        }

        //metoda na vytvorenie jsonu
        public String getJson()
        {
            string jsonData = JsonConvert.SerializeObject(this);
            return jsonData;
        }

    }


}
