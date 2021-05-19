using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckingCard.Check
{
    public class Check
    {
        Card.Card card1 = new Card.Card("123456789012", "12.34", 123, 10);
        Card.Card card2 = new Card.Card("111111111111", "11.11", 111, 20);
        Card.Card card3 = new Card.Card("222222222222", "22.22", 222, 30);
        Card.Card card4 = new Card.Card("333333333333", "33.33", 333, 40);
        Card.Card card5 = new Card.Card("444444444444", "44.44", 444, 50);

        public List<Card.Card> cards = new List<Card.Card>();

        public void AddCards()
        {
            cards.Add(card1);
            cards.Add(card2);
            cards.Add(card3);
            cards.Add(card4);
            cards.Add(card5);
        }

        public int Checking(string cardNumber, string expirationDate, int cvc, int sum)
        {

            AddCards();
            foreach (Card.Card card in cards)
            {
                if (card.CardNumber.Equals(cardNumber) && card.ExpirationDate.Equals(expirationDate) && card.Cvc == cvc)
                {
                    if (sum < card.Sum)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            return -1;
        }
    }
}
