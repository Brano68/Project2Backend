using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckingCard.Check
{
    public class Check
    {
        Card.Card card1 = new Card.Card("1111222233334444", "Peter Novak", "05.23", 123, 900);
        Card.Card card2 = new Card.Card("5555666677778888", "Pavel Kopkas", "11.24", 111, 30);
        Card.Card card3 = new Card.Card("9999000011112222", "Matus Popovic", "06.22", 222, 15);
        Card.Card card4 = new Card.Card("1234567890123456", "Daniela Jurpakova", "06.24", 333, 2500);
        Card.Card card5 = new Card.Card("2222444466668888", "Laura Bogarova", "04.22", 444, 50);

        public List<Card.Card> cards = new List<Card.Card>();

        public void AddCards()
        {
            cards.Add(card1);
            cards.Add(card2);
            cards.Add(card3);
            cards.Add(card4);
            cards.Add(card5);
        }

        public int Checking(string cardNumber, string cardName, string expirationDate, int cvc, int sum)
        {

            AddCards();
            foreach (Card.Card card in cards)
            {
                if (card.CardNumber.Equals(cardNumber) && card.CardName.Equals(cardName) && card.ExpirationDate.Equals(expirationDate) && card.Cvc == cvc)
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
