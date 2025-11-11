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

        private IStrategyService _strategy;

        public List<string> Cards => _hand.Cards;


        public BJackDealer(IStrategyService strategyService)
        {
            _strategy = strategyService;
        }

        public Decision MakeDecision(HandType handType, int dealerHandValue, int playerHandValue = 0)
        {
            playerHandValue = 0; // ensure value is 0, had to be done cos of interface IParticipant
            return _strategy.MakeDecision(ParticipantType.Dealer, handType, dealerHandValue);
        }

        public void Draw(IDeck deck, int numCards) => _hand.Draw(deck, numCards);
        public void Clear() => _hand.Clear();
    }
}
