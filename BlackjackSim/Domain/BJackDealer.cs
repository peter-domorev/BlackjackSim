using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class BJackDealer
    {
        private Hand _hand = new Hand();
        private List<string> _cards => _hand.Cards;

        private IDeck _deck;
        private IStrategyService _strategy;
        private IHandService _handService;

        public List<string> Cards => _cards; // not needed?


        public BJackDealer(IDeck deck, IStrategyService strategyService, IHandService handService)
        {
            _deck = deck;
            _strategy = strategyService;
            _handService = handService;
        }

        private Decision MakeDecision(HandType handType, int dealerHandValue, int playerHandValue = 0)
        {
            playerHandValue = 0; // ensure value is 0, had to be done cos of interface IParticipant
            ParticipantType participantType = ParticipantType.Dealer;
            return _strategy.MakeDecision(participantType, handType, dealerHandValue);
        }

        public void CompleteTurn()
        {
            Decision decision;
            bool isBust;

            do
            {
                HandType handType = _handService.GetHandType(_cards);
                int handValue = _handService.CalculateValue(_cards);
                decision = MakeDecision(handType, handValue);

                if (decision == Decision.Hit)
                {
                    int numCardsToDraw = 1;
                    _hand.Draw(_deck, numCardsToDraw);

                }
                isBust = _handService.IsBust(_cards);
            }
            while (!isBust || (decision != Decision.Stand));
        }

        public void Clear() => _hand.Clear();
    }
}
