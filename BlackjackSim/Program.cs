using BJackSim;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Data;
using System.Text;
using ExcelDataReader;




namespace BJackSim
{
    public class Program
    {
        static void Main(string[] args)
        {

            StrategyService strategy = new StrategyService();
            BJackDealer dealer = new BJackDealer();
            BJackPlayer player = new BJackPlayer();

            Type dealerType = dealer.GetType();
            Type playerType = player.GetType();

            Console.WriteLine(dealerType);
            Console.WriteLine(playerType);








        }
    }
}


