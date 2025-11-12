using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class GameRules
    {
        public class DoubleRules
        {
            public readonly bool DoubleAllowed = true;
            public readonly bool DoubleAfterSplitAllowed = true;

            public DoubleRules(bool doubleAllowed, bool doubleAfterSplitAllowed)
            {
                DoubleAllowed = doubleAllowed;

                if (!doubleAllowed)
                    doubleAfterSplitAllowed = false; // ensure rules don't contradict

                DoubleAfterSplitAllowed = doubleAfterSplitAllowed;
                
            }
        }


        public class SplitRules
        {
            public readonly SplitType SplitType = SplitType.IdentialRanks;
            public readonly int NumSplitsAllowed = 3;

            public SplitRules(SplitType splitType , int numSplitsAllowed)
            {
                SplitType = splitType;

                if (splitType == SplitType.NotAllowed)
                    numSplitsAllowed = 0; // ensure rules don't contradict

                NumSplitsAllowed = numSplitsAllowed;
            }
        }

        public class SplitAceRules
        {
            public readonly bool ResplitAcesAllowed;
            public readonly bool OneCardPerAce;
        }

        public class SurrenderRules
        {
            public readonly SurrenderType SurrenderType = SurrenderType.NotAllowed;

            public SurrenderRules(SurrenderType surrenderType)
            {
                SurrenderType = surrenderType;
            }
        }



        public GameRules()
        {


        } 

    }
}
