using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class Hand
    {
        protected List<string> _cards = new List<string>();
        public List<string> Cards => _cards;

        public void Draw(IDeck deck, int numCards)
        {
            List<string> newCards = deck.Deal(numCards);
            if (newCards == null) throw new ArgumentNullException("Deck dealt no cards");
            _cards.AddRange(newCards);
        }
        public void Clear()
        {
            _cards = [];
        }
    }
}
