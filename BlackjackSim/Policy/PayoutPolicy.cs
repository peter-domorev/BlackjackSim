using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class PayoutPolicy
    {
        public bool SplitBJackIsBJack { get; }

        public PayoutPolicy(GameRules gameRules, bool splitBJackIsBJack = false)
        {
            // ensure rules don't contradict
            if (gameRules.Split.SplitType == SplitType.NotAllowed) 
                splitBJackIsBJack = false;

            SplitBJackIsBJack = splitBJackIsBJack;

            

        }



    }
}
