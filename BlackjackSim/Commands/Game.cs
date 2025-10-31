using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackSim
{
    class Game
    {
        private int numRounds;
        private int numDecks;
        private int cash;
        private int betAmount;
        private bool doubleDownAllowed;
        private bool surrenderAllowed;
 

        private Game(int NumDecks, int NumRounds, int Cash, int BetAmount, bool DoubleDownAllowed, bool SurrenderAllowed)
        {
            numDecks = NumDecks;
            numRounds = NumRounds;
            cash = Cash;
            betAmount = BetAmount;
            doubleDownAllowed = DoubleDownAllowed;
            surrenderAllowed = SurrenderAllowed;


            for (int i = 0; i < numRounds; i++)
            {
                PlayRound();
            }

        }


        public void PlayRound()
        {
            string playerDecision;
            string dealerDecision;
            string result;

            // clear hands

            // draw cards

            /*
            do
            {
                // make player deicion
                
                switch (playerDecision)
                {
                    case "H":
                        player.Hand.Add(deck.DrawCards(1));
                        break;
                    case "D":
                        if (doubleDownAllowed)
                        {
                            // double the bet
                            player.Hand.Add(deck.DrawCards(1));
                            break;
                        }
                        goto case "H";
                    //case "SP":
                    //break;
                    case "R/H":
                        if (surrenderAllowed)
                        {
                            // halve the bet
                            result = "DealerWins"; // end game
                            break;
                        }
                        goto case "H";
                }
            }
            while (!player.Hand.isBust || (playerDecision != "S" || playerDecision != "D"));


            if (player.Hand.isBust)
            {
                result = "DealerWins";
            }
            else
            {
                do
                {
                    dealerDecision = strategy.Decision("Dealer", dealer.Hand.Type(), dealer.Hand.Value());

                    if (dealerDecision == "H")
                    {
                        dealer.Hand.Add(deck.DrawCards(1));
                    }
                }
                while (!dealer.Hand.isBust || (dealerDecision != "S"));


                if (dealer.Hand.isBust)
                {
                    result = "PlayerWins";
                }
                else
                {
                    if (dealer.Hand.Value() > player.Hand.Value())
                    {
                        result = "DealerWins";
                    }
                    else if (dealer.Hand.Value() < player.Hand.Value())
                    {
                        result = "PlayerWins";
                    }
                    else // tie
                    {
                        result = "Push";
                    }
                }
            }

            return result;*/
        }
    }
}
