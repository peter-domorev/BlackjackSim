using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class PlayerHand : Hand
    {
        private bool isSplitHand = false;
        
        public int BetAmount { get; set; }
        public bool IsSplitHand => isSplitHand;
        public bool Surrendered { get; set; }



        /// <summary>
        /// Returns this hand split
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<PlayerHand> Split()
        {
            if (this._cards.Count != 2) throw new Exception("Hand being split contains more than 2 cards");

            PlayerHand splitHand1 = this;
            splitHand1.isSplitHand = true; // validate this works
            splitHand1._cards = new List<string> { this._cards[0] };

            PlayerHand splitHand2 = this;
            splitHand2.isSplitHand = true;
            splitHand2._cards = new List<string> { this._cards[1] };

            List<PlayerHand> newHands = new List<PlayerHand> { splitHand1, splitHand2 };

            return newHands;

        }
    }
}
