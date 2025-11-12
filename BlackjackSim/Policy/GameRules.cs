using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class GameRules
    {
        // nested classes violate encapsulation?

        public class DoubleRules
        {
            public bool DoubleAllowed { get; }
            public bool DoubleAfterSplitAllowed { get; }

            public DoubleRules(bool doubleAllowed = true, bool doubleAfterSplitAllowed = true)
            {
                DoubleAllowed = doubleAllowed;

                if (!doubleAllowed)
                    doubleAfterSplitAllowed = false; // ensure rules don't contradict

                DoubleAfterSplitAllowed = doubleAfterSplitAllowed;
                
            }


            internal static DoubleRules Default = new DoubleRules();
        }


        public class SplitRules
        {
            /// <summary>
            /// If dealer has Ace or 10 showing, some tables restrict resplitting
            /// </summary>
            public bool DealerRestrictsReSplit { get; }

            public bool SplitBJackIsBJack { get; }
            public SplitType SplitType { get; }
            public int MaxSplitsAllowed { get; }

            

            public SplitRules(bool dealerRestrictsReSplit = true, bool splitBJackIsBJack = false, SplitType splitType = SplitType.IdentialRanks, int numSplitsAllowed = 3)
            {
                DealerRestrictsReSplit = dealerRestrictsReSplit;
                SplitBJackIsBJack = splitBJackIsBJack;
                SplitType = splitType;

                if (splitType == SplitType.NotAllowed)
                {
                    // ensure rules don't contradict
                    DealerRestrictsReSplit = false;
                    SplitBJackIsBJack = false;
                    numSplitsAllowed = 0;
                }

                MaxSplitsAllowed = numSplitsAllowed;
            }

            internal static SplitRules Default = new SplitRules();
        }

        public class SplitAceRules
        {
            public bool ReSplitAcesAllowed { get; }
            public bool OneCardPerAce { get; }

            public SplitAceRules(bool reSplitAcesAllowed = false, bool oneCardPerAce = true)
            {
                ReSplitAcesAllowed = reSplitAcesAllowed;
                OneCardPerAce = oneCardPerAce;
            }

            internal static SplitAceRules Default = new SplitAceRules();
        }

        public class SurrenderRules
        {
            public bool SurrenderOnSplit { get; }
            public SurrenderType SurrenderType { get; }

            public SurrenderRules(bool surrenderOnSplit = false, SurrenderType surrenderType = SurrenderType.NotAllowed)
            {
                SurrenderOnSplit = surrenderOnSplit;
                SurrenderType = surrenderType;
            }

            internal static SurrenderRules Default = new SurrenderRules();
        }


        public DoubleRules Double { get; }
        public SplitRules Split { get; }
        public SplitAceRules SplitAce { get; }
        public SurrenderRules Surrender { get; }


        // better way to handle rule changes? rather than creating a new object in this constructor?
        public GameRules(DoubleRules? doubleRules = null, SplitRules? splitRules = null, SplitAceRules? splitAceRules = null, SurrenderRules? surrenderRules = null)
        {
            Double = doubleRules ?? DoubleRules.Default;
            Split = splitRules ?? SplitRules.Default;
            SplitAce = splitAceRules ?? SplitAceRules.Default;
            Surrender = surrenderRules ?? SurrenderRules.Default;

        } 

    }
}
