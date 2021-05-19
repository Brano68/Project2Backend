using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckingCard.Card
{
    public class Card
    {
        private String cardNumber;
        private String expirationDate;
        private int cvc;
        private int sum;

        public Card(string cardNumber, string expirationDate, int cvc, int sum)
        {
            this.cardNumber = cardNumber;
            this.expirationDate = expirationDate;
            this.cvc = cvc;
            this.sum = sum;
        }

        public String CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }
        public String ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; }
        }
        public int Cvc
        {
            get { return cvc; }
            set { cvc = value; }
        }
        public int Sum
        {
            get { return sum; }
            set { sum = value; }
        }
    }
}
