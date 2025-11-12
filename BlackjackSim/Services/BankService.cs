using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class BankService
    {
        public int CalculateNetProfit(Outcome outcome, List<string> cards, string playerDecision, int betAmount)
        {
            int netProfit = 0; // change to just declaration

            switch (outcome)
            {
                case Outcome.PlayerWins:
                    switch (playerDecision)
                    {
                        case "":
                            Console.WriteLine("Hi");
                            break;
                    }
                    break;
                case Outcome.DealerWins:
                    netProfit = -betAmount;
                    break;
                case Outcome.Push:
                    netProfit = 0;
                    break;
                
            }


            return netProfit;
        }
    }
}
