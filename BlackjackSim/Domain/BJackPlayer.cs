using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class BJackPlayer
    {
        private Dictionary<Hand, int> hands = new Dictionary<Hand, int>(); // key: hand, value: betAmount

        public Dictionary<Hand, int> Hands => hands;

        public BJackPlayer()
        {

        }

        public void Draw(IDeck deck, int numCards) => _hand.Draw(deck, numCards);
        public void Clear() => hands = new Dictionary<Hand, int>();



        private int profit = 0;



        public int Profit
        {
            get { return profit; }
            set { profit = value; }
        }

    }
}
