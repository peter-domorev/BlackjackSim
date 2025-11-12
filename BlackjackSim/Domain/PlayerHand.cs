using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class PlayerHand : Hand
    {
        private int _numTimesSplit = 0;
        
        public int BetAmount { get; set; }
        public int NumTimesSplit { 
            get { return _numTimesSplit; }
            set { _numTimesSplit = value; }
        }
        public bool Surrendered { get; set; }
    }
}
