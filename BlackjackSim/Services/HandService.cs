using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class HandService : IHandService
    {

        public HandType GetHandType(List<string> cards)
        {
            if (cards.Count == 2 && cards[0] == cards[1])
            {
                return HandType.Pair;
            }
            else if (cards.Contains("A"))
            {
                return HandType.Soft;
            }
            else
            {
                return HandType.Hard;
            }
        }

        public int CalculateValue(List<string> cards)
        {
            int value = 0;

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

            return value;
        }

        public bool IsBust(List<string> cards)
        {
            if (CalculateValue(cards) > 21) return true;
            return false;
        }
    }
}
