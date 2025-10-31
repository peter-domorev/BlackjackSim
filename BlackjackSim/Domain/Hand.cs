using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackSim
{
    public class Hand
    {
        private List<string> cards = new List<string>();
        public List<string> Cards => cards;

        public void Add(List<string> newCards)
        {
            Cards.AddRange(newCards);
        }
        public void Clear()
        {
            cards = [];
        }
    }
}
