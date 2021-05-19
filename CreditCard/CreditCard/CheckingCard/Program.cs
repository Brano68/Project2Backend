using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckingCard
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckingCard.Check.Check check = new CheckingCard.Check.Check();
            Console.WriteLine("Srart");
            check.Checking("123456789012", "1234", "123", 4);



            Console.Read();
        }
    }
}
