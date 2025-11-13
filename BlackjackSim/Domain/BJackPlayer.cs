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
        private IDeck _deck;
        private IHandService _handService;
        private IStrategyService _strategy;
        private GameRules _gameRules;

        private List<PlayerHand> _hands = new List<PlayerHand>();

        public List<PlayerHand> AllHands => _hands;
        public List<PlayerHand> NormalHands => _hands.FindAll(h => h.IsSplitHand == false);
        public List<PlayerHand> SplitHands => _hands.FindAll(h => h.IsSplitHand == true);






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
            _hands = new List<PlayerHand>();
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





        private void CompleteTurns(int dealerHandValue)
        {
            List<PlayerHand> newNormalHands = new List<PlayerHand>();
            List<PlayerHand> newSplitHands = new List<PlayerHand>();

            // work flow: 
            // 


            // deal with normal hands first
            foreach (PlayerHand hand in NormalHands)
            {                
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
                                hand.BetAmount *= 2;
                                hand.Draw(_deck, numCardsToDraw);
                                break;
                            }
                            goto case Decision.Hit;


                        case Decision.Split:
                            if (_gameRules.Split.SplitType == SplitType.NotAllowed)
                                throw new Exception("Split not allowed, player action not implemented");
                            
                            switch (_gameRules.Split.SplitType)
                            {
                                case SplitType.EqualValues: // need to deal with decision service splitting when not allowed, change of rules
                                case SplitType.IdentialRanks:

                                    List<PlayerHand> splitHands = hand.Split();
                                    newSplitHands.AddRange(splitHands);
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
                                case SurrenderType.Late:
                                    hand.Surrendered = true;
                                    break;
                            }
                            break;

                    }

                    isBust = _handService.IsBust(cards);
                }
                while (!isBust || (decision != Decision.Stand || decision != Decision.Double));

                newNormalHands.Add(hand); // add to new hands list

            }

            _hands = newHands; // replace with new hand, as _hands can't be directly modified
        }

        private void CompleteNormalTurn(int dealerHandValue)
        {

        }

        private void CompleteSplitTurns(int dealerHandValue, List<PlayerHand> splitHands)
        {
            foreach (PlayerHand hand in splitHands)
            {

                List<string> cards = hand.Cards;

                Decision decision;
                bool isBust;
                int numCardsToDraw = 1;

                List<PlayerHand> newSplitHands = new List<PlayerHand>();


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
                            if (_gameRules.Double.DoubleAllowed 
                                && _gameRules.Double.DoubleAfterSplitAllowed) // added rule
                            {
                                hand.BetAmount *= 2; 
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
                            if (_gameRules.Surrender.SurrenderType == SurrenderType.NotAllowed
                                || _gameRules.Surrender.SurrenderOnSplit == false) // added rule
                            {
                                goto case Decision.Hit;
                            }

                            switch (_gameRules.Surrender.SurrenderType)
                            {
                                case SurrenderType.Early:
                                case SurrenderType.Late:
                                    hand.Surrendered = true;
                                    break;
                            }
                            break;

                    }

                    isBust = _handService.IsBust(cards);
                }
                while (!isBust || (decision != Decision.Stand || decision != Decision.Double));

                newSplitHands.Add(hand); // add new split hand to newSplitHands
            }

        }





        private int profit = 0;



        public int Profit
        {
            get { return profit; }
            set { profit = value; }
        }

    }
}
