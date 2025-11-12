using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class BJackPlayer
    {
        private Dictionary<Hand, int> _hands = new Dictionary<Hand, int>(); // key: hand, value: betAmount


        private IDeck _deck; // do you still make references in this style _deck? or just deck
        private IHandService _handService;
        private IStrategyService _strategy;

        private GameRules _gameRules;


        public Dictionary<Hand, int> Hands => _hands;

        public BJackPlayer(IDeck deck, IHandService handService, IStrategyService strategySerivce, GameRules gameRules)
        {
            _deck = deck;
            _handService = handService;
            _strategy = strategySerivce;
            _gameRules = gameRules;
        }

        public void Clear() => _hands = new Dictionary<Hand, int>(); 

        private Decision MakeDecision(HandType handType, int playerHandValue, int dealerHandValue)
        {
            ParticipantType participantType = ParticipantType.Player;
            return _strategy.MakeDecision(participantType, handType, dealerHandValue, playerHandValue);
        }



        public void CompleteTurns(int dealerHandValue)
        {
            Dictionary<Hand, int> newHands = new Dictionary<Hand, int>();

            foreach (var kvp in _hands)
            {
                Hand hand = kvp.Key;
                int betAmount = kvp.Value;

                List<string> cards = hand.Cards;
                Decision decision;
                bool isBust;

                int numCardsToDraw = 1;


                do
                {
                    HandType handType = _handService.GetHandType(cards);
                    int handValue = _handService.CalculateValue(cards);
                    decision = MakeDecision(handType, handValue, dealerHandValue);

                    switch (decision)
                    {
                        case Decision.Hit:
                            hand.Draw(_deck, numCardsToDraw);
                            break;


                        case Decision.Double:
                            if (_gameRules.DoubleAllowed)
                            {
                                betAmount = betAmount * 2;
                                hand.Draw(_deck, numCardsToDraw);
                                break;
                            }
                            goto case Decision.Hit;


                        case Decision.Split:
                            Dictionary<Hand, int> splitHand = new Dictionary<Hand, int>(); //

                            break;


                        case Decision.SurrenderOrHit:
                            if (_gameRules.SurrenderType == SurrenderType.NotAllowed)
                            {
                                goto case Decision.Hit;
                            }

                            switch (_gameRules.SurrenderType)
                            {
                                case SurrenderType.Early:
                                    // lose half of bet
                                    break;

                                case SurrenderType.Late:
                                    // lose half of bet if dealer doesn't have blackjack
                                    break;
                            }
                            break;
                            
                    }

                    isBust = _handService.IsBust(cards);
                }
                while (!isBust || (decision != Decision.Stand|| decision != Decision.Double));

                newHands.Add(hand, betAmount); // add new hand to newHands
            }

            _hands = newHands; // replace with new hand, as _hands can't be directly modified

        }





        private int profit = 0;



        public int Profit
        {
            get { return profit; }
            set { profit = value; }
        }

    }
}
