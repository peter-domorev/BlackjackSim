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

/* 
 * How am I going to deal with splits in the decision making sheet, being dependent on rules on splits?
 * How am I going to deal with hands that want to be split, but the total number of split hands has been reached?
 * 
 * How am I going to deal with how much is bet on each hand?
 * How am I going to deal with wanting to double down or splitting, but there isn't enough to do that bet?
 * 
 * How am I going to deal with splitting a hand, and then running it through the do turn?
 * How am I going to keep track with what hand is finished and what isn't?
 */

/*
 * Basic strategy table requires:
 *
 * 
 * defaults:
 * split, else (aces resplit, split re split, too many split)
 * double, else (not allowed, split hand)
 * surrender, else (split hand)
 * 
 * Dealing with:
 * Re split previous hands, but then you hit the limit, initial hands get more choice, so therefore better odds (skewed)
 * Not knowing that you have aces that can't be resplit
 * 
 */




namespace BJackSim
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Settings
            int numDecks = 4;




            // Domain
            Deck deck = new Deck(numDecks);

            // Policy
            GameRules gameRules = new GameRules();
            PayoutPolicy payoutPolicy = new PayoutPolicy(gameRules);

            // Servies
            BankService bankService = new BankService();
            HandService handService = new HandService(gameRules);
            StrategyService strategryService = new StrategyService();

            // Commands
            //BJackGame bJackGame = new BJackGame()

            int i = 0;
            string? currentCard;
            do
            {
                currentCard = deck.Deal(1).ToString();
                i++;

                Console.WriteLine(currentCard);
                Console.WriteLine($"Number of cards printed: {i}");

            }
            while (currentCard != null);

            Console.WriteLine("All cards have been dealt");
            

            







        }
    }
}


