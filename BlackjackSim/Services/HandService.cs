using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class HandService : IHandService
    {
        private const int DEFAULT_ACE_VALUE = 1;
        private const int ADDITIONAL_ACE_VALUE = 10; // used to add 10 to default ace value to get 11
        private const int FACE_CARD_VALUE = 10;

        private GameRules _gameRules;

        public HandService(GameRules gameRules)
        {
            _gameRules = gameRules;
        }

        public HandType GetHandType(List<string> cards)
        {

            if (cards.Count == 2)
            {
                switch (_gameRules.Split.SplitType)
                {

                    case SplitType.IdentialRanks:
                        if (cards[0] == cards[1]) 
                            return HandType.Pair;
                        break;

                    case SplitType.EqualValues:
                        if (IndividualValue(cards[0]) == IndividualValue(cards[1]))
                            return HandType.Pair;
                        break;
                }
            }
            
            if (cards.Contains("A"))
            {
                return HandType.Soft;
            }
            else
            {
                return HandType.Hard;
            }
        }

        public int CalculateValue(List<string> cards) // calculates the highest useful value
        {
            int totalValue = 0;

            int numAces = 0;

            foreach (string card in cards) // assume acces have a value of 1
            {
                int value = IndividualValue(card);
                totalValue += value;

                if (card == "A") numAces++;
            }


            if (numAces != 0) // manage aces having multiple values
            {
                int minTotal = totalValue;
                int maxTotal = totalValue + ADDITIONAL_ACE_VALUE; // 2 aces as 10's is bust, so only 1 10 needs to be added

                // return highest value
                if (maxTotal > 21)
                {
                    totalValue = minTotal;
                }
                else
                {
                    totalValue = maxTotal;
                }

            }

            return totalValue;
        }

        private int IndividualValue(string card) // aces are assumed to be 1
        {
            int value;

            if (int.TryParse(card, out int result))
            {
                value = result;
            }
            else if (card == "A")
            {
                value = DEFAULT_ACE_VALUE;
            }
            else
            {
                value = FACE_CARD_VALUE;
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
