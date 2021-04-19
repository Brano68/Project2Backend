using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.CheckDateTime
{
    public class ChcekDateAndTime
    {
        //toto bude sluzit na porovnavanie datumov ci je dobry
        public Boolean checkDates(DateTime fromDt, DateTime toDt, DateTime fromFront, DateTime toFront){
            if (fromDt > fromFront && fromDt > toFront)
            {
                return true;
            } else if (fromFront > toDt) {
                return true;
            }
            return false;
        }

    }
}
