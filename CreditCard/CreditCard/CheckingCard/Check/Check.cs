using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckingCard.Check
{
    class Check
    {
        Card.Card card1 = new Card.Card("123456789012", "1234", "123", 10);
        Card.Card card2 = new Card.Card("111111111111", "1111", "111", 20);
        Card.Card card3 = new Card.Card("222222222222", "2222", "222", 30);
        Card.Card card4 = new Card.Card("333333333333", "3333", "333", 40);
        Card.Card card5 = new Card.Card("444444444444", "4444", "444", 50);

        public List<Card.Card> cards = new List<Card.Card>();

        public void AddCards()
        {
            cards.Add(card1);
            cards.Add(card2);
            cards.Add(card3);
            cards.Add(card4);
            cards.Add(card5);
        }

        public void PrintAllCards()
        {
            foreach (Card.Card card in cards)
            {
                Console.WriteLine(card.CardNumber);
            }
        }

        public void Test()
        {
            Console.WriteLine("1");
            Console.WriteLine(card1.CardNumber);
            Console.WriteLine(2);
        }
        public bool Checking(string cardNumber, string expirationDate, string cvc, int sum)
        {

            AddCards();
            foreach (Card.Card card in cards)
            {
                if (card.CardNumber.Equals(cardNumber) && card.ExpirationDate.Equals(expirationDate) && card.Cvc.Equals(cvc))
                {
                    Console.WriteLine("The card is valid");
                    if (sum < card.Sum)
                    {
                        Console.WriteLine("Payment was successful");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Not enough money in the account");
                        return false;
                    }

                }
            }
            Console.WriteLine("The card is invalid");
            return false;
        }
    }
}
