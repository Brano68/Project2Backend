using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.MakeJson
{
    public class MyOrders
    {
        public List<Order> myOrders { get; set; }


        public MyOrders(List<Order> myOrders)
        {
            this.myOrders = myOrders;
        }
    }
}
