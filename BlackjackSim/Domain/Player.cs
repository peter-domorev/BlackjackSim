using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackSim
{
    public class Player : Participant
    {
        private int profit = 0;
        private int betAmount;

        public int BetAmount
        {
            get { return betAmount; }
            set { betAmount = value; }
        }

        public int Profit
        {
            get { return profit; }
            set { profit = value; }
        }

    }
}
