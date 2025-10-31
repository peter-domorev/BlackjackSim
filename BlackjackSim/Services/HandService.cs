using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackSim
{
    public interface IHandService
    {
        public int CalculateValue(Hand hand);
    }
    public class HandService : IHandService
    {
        /*
        public string Type()
        {
            if (Cards.Count == 2 && Cards[0] == Cards[1])
            {
                return "Pair";
            }
            else if (Cards.Contains("A"))
            {
                return "Soft";
            }
            else
            {
                return "Hard";
            }
        }
        */
        public int CalculateValue(Hand hand)
        {
            int value = 0;
            List<string> cards = hand.Cards;

            // sum all cards, leaving ace as 1
            foreach (string card in cards)
            {
                if (int.TryParse(card, out int result))
                {
                    value += result;
                }
                else if (card == "A")
                {
                    value += 1;
                }
                else
                {
                    value += 10;
                }
            }

            // handle aces
            int numAces = cards.Count(c => c == "A");
            if (numAces != 0)
            {
                int minTotal = value;
                int maxTotal = value + 10; // 2 aces as 10's is bust

                // return highest value
                if (maxTotal <= 21)
                {
                    value = maxTotal;
                }
                else
                {
                    value = minTotal;
                }

            }
            else // no aces
            {
                // check for bust
                if (value > 21)
                {
                    isBust = true;
                }
                else
                {
                    value = value;
                }
            }

            return value;
        }
    }
}
