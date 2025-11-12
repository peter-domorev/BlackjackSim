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
        private IDeck _deck; // do you still make references in this style _deck? or just deck
        private IHandService _handService;
        private IStrategyService _strategy;

        private GameRules _gameRules;



        private Dictionary<Hand, int> _normalHands = new Dictionary<Hand, int>();  // key: hand, value: betAmount
        private Dictionary<Hand, int> _splitHands = new Dictionary<Hand, int>();
        public Dictionary<Hand, int> NormalHands => _normalHands;
        public Dictionary<Hand, int> SplitHands => _splitHands;



        

        public BJackPlayer(IDeck deck, IHandService handService, IStrategyService strategySerivce, GameRules gameRules)
        {
            _deck = deck;
            _handService = handService;
            _strategy = strategySerivce;
            _gameRules = gameRules;
        }


        /// <summary>
        /// Resets all hands
        /// </summary>
        public void Clear()
        {
            _normalHands = new Dictionary<Hand, int>();
            _splitHands = new Dictionary<Hand, int>();
        }



        /// <summary>
        /// Player makes a decision based on an indivdual hand
        /// </summary>
        /// <param name="handType">Type of the individual hand</param>
        /// <param name="playerHandValue">Value of the individual hand</param>
        /// <param name="dealerHandValue">Value of the dealer's hand</param>
        /// <returns></returns>
        private Decision MakeDecision(HandType handType, int playerHandValue, int dealerHandValue)
        {
            ParticipantType participantType = ParticipantType.Player;
            return _strategy.MakeDecision(participantType, handType, dealerHandValue, playerHandValue);
        }



        public void CompleteTurns(int dealerHandValue)
        {
            Dictionary<Hand, int> newHands = new Dictionary<Hand, int>(); //
            Dictionary<Hand, int> splitHands = new Dictionary<Hand, int>(); //


            

        }


        private void CompleteNormalTurns(int dealerHandValue)
        {
            

            foreach (var kvp in _normalHands)
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
                            if (_gameRules.Double.DoubleAllowed)
                            {
                                betAmount = betAmount * 2;
                                hand.Draw(_deck, numCardsToDraw);
                                break;
                            }
                            goto case Decision.Hit;


                        case Decision.Split:
                            if (_gameRules.Split.SplitType == SplitType.NotAllowed)
                                throw new Exception("Split not allowed, player action not implemented");
                            
                            switch (_gameRules.Split.SplitType)
                            {
                                case SplitType.EqualValues:

                                    break;
                                case SplitType.IdentialRanks:

                                    break;
                            }

                            throw new Exception("No split action taken");
                       


                        case Decision.SurrenderOrHit:
                            if (_gameRules.Surrender.SurrenderType == SurrenderType.NotAllowed)
                            {
                                goto case Decision.Hit;
                            }

                            switch (_gameRules.Surrender.SurrenderType)
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
                while (!isBust || (decision != Decision.Stand || decision != Decision.Double));

                newHands.Add(hand, betAmount); // add new hand to newHands
            }

            _hands = newHands; // replace with new hand, as _hands can't be directly modified
        }

        private void CompleteSplitTurns(int dealerHandValue)
        {
            foreach (var kvp in _splitHands)
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
                            if (_gameRules.Double.DoubleAllowed)
                            {
                                betAmount = betAmount * 2;
                                hand.Draw(_deck, numCardsToDraw);
                                break;
                            }
                            goto case Decision.Hit;


                        case Decision.Split:
                            if (_gameRules.Split.SplitType == SplitType.NotAllowed)
                                throw new Exception("Split not allowed, replacement player action not implemented");

                            if (_gameRules.Split.DealerRestrictsReSplit)
                                throw new Exception("Split not allowed, replacement player action not implemented");

                            if (_splitHands.Count == _gameRules.Split.MaxSplitsAllowed)
                                throw new Exception("Split not allowed, replacement player action not implemented");


                            switch (_gameRules.Split.SplitType)
                            {
                                case SplitType.EqualValues:
                                    //
                                    break;
                                case SplitType.IdentialRanks:
                                    //
                                    break;
                            }

                            throw new Exception("No split action taken");



                        case Decision.SurrenderOrHit:
                            if (_gameRules.Surrender.SurrenderType == SurrenderType.NotAllowed)
                            {
                                goto case Decision.Hit;
                            }

                            switch (_gameRules.Surrender.SurrenderType)
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
                while (!isBust || (decision != Decision.Stand || decision != Decision.Double));

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
