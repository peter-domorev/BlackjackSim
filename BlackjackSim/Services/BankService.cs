using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackSim
{
    public class BankService
    {
        public int CalculateNetProfit(string result, List<string> cards, string playerDecision, int betAmount)
        {
            int netProfit = 0; // change to just declaration

            switch (result)
            {
                case "DealerWins":
                    netProfit = -betAmount;
                    break;
                case "Push":
                    netProfit = 0;
                    break;
                case "PlayerWins":
                    switch (playerDecision)
                    {
                        case "":
                            Console.WriteLine("Hi");
                            break;
                    }
                    break;
            }


            return netProfit;
        }
    }
}
