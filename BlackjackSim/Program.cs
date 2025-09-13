using BlackjackSim;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Data;
using System.Text;
using ExcelDataReader;




namespace BlackjackSim
{
    internal class Program
    {
        static void Main(string[] args)
        {



            BasicStrategy test = new BasicStrategy();
            string test_value = test.Lookup(test.player_hard_table, "16", "Q");
            Console.WriteLine(test_value);




        }
    }


    class DeckManager
    {
        private List<string> allCards;
        private Random rng = new Random();
        private int numDecks;


        public DeckManager(int NumDecks)
        {
            numDecks = NumDecks;
            BuildDecks();
        }


        public int NumCards
        {
            get { return 52 * numDecks; }
        }

        private void BuildDecks()
        {
            string[] oneSuit = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };


            // create single deck
            string[] singleDeck = new string[52];
            for (int i = 0; i < 4; i++)
            {
                Array.Copy(oneSuit, 0, singleDeck, i * oneSuit.Length, oneSuit.Length);
            }

            // create all decks
            string[] allCardsArray = new string[52 * numDecks];
            for (int i = 0; i < numDecks; i++)
            {
                Array.Copy(singleDeck, 0, allCardsArray, i * singleDeck.Length, singleDeck.Length);
            }
            allCards = allCardsArray.ToList();
        }


        public List<string> DrawCards(int numCards)
        {
            List<string> cards = new List<string>(numCards);

            for (int i = 0; i < numCards; i++)
            {
                int index = rng.Next(allCards.Count);
                cards.Add(allCards[index]);
                allCards.RemoveAt(index);
            }
            return cards;
        }


        public void ShuffleDeck()
        {
            BuildDecks();
        }
    }


    class Hand
    {
        public List<string> Cards = new List<string>();
        public bool isBust = false;

        public void Add(List<string> newCards)
        {
            Cards.AddRange(newCards);
        }

        public void Clear()
        {
            Cards = [];
        }

        public string Type()
        {
            if (Cards.Count == 2 && Cards[0] == Cards[1])
            {
                return "Pair";
            }
            else if (Cards.Contains("A"))
            {
                return "Soft";
            }
            else
            {
                return "Hard";
            }
        }

        public int Value()
        {
            int total = 0;
            int value = 0;

            // sum all cards, leaving ace as 1
            foreach (string card in Cards)
            {
                if (int.TryParse(card, out int result))
                {
                    total += result;
                }
                else if (card == "A")
                {
                    total += 1;
                }
                else
                {
                    total += 10;
                }
            }

            // handle aces
            int numAces = Cards.Count(c => c == "A");
            if (numAces != 0)
            {
                int minTotal = total;
                int maxTotal = total + 10; // 2 aces as 10's is bust

                // check for bust and/or return highest value
                if (maxTotal <= 21)
                {
                    value = maxTotal;
                }
                else if (minTotal <= 21)
                {
                    value = minTotal;
                }
                else
                {
                    isBust = true;
                }

            }
            else // no aces
            {
                // check for bust
                if (total > 21)
                {
                    isBust = true;
                }
                else
                {
                    value = total;
                }
            }

            return value;
        }
    }

    class BasicStrategy
    {
        private string path = @"C:\Users\peter\OneDrive\Projects\csharp\blackjack_sim\basic_strategy.xlsx";
        public DataTable player_hard_table;
        private DataTable player_soft_table;
        private DataTable player_pair_table;
        private DataTable dealer_hard_table;
        private DataTable dealer_soft_table;

        public BasicStrategy()
        {
            player_hard_table = ReadSheet("player_hard");
            player_soft_table = ReadSheet("player_soft");
            player_pair_table = ReadSheet("player_pair");
            dealer_hard_table = ReadSheet("dealer_hard");
            dealer_soft_table = ReadSheet("dealer_soft");
        }

        public string Decision(string ParticipantType, string HandType, int DealerHandValue, int PlayerHandValue = 0)
        {
            string dealerHandValue = Convert.ToString(DealerHandValue);
            string playerHandValue = Convert.ToString(PlayerHandValue);

            string decisionKey = $"{ParticipantType}{HandType}";

            switch (decisionKey)
            {
                case "PlayerHard":
                    return Lookup(player_hard_table, playerHandValue, dealerHandValue);
                case "PlayerSoft":
                    return Lookup(player_soft_table, playerHandValue, dealerHandValue);
                case "PlayerPair":
                    return Lookup(player_pair_table, playerHandValue, dealerHandValue);
                case "DealerHard":
                    return Lookup(dealer_hard_table, dealerHandValue);
                case "DealerSoft":
                    return Lookup(dealer_soft_table, dealerHandValue);
                default: return "Invalid";
            }
        }

        private DataTable ReadSheet(string sheetName = null)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // put in static method?

            using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var ds = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
            });
            return sheetName != null ? ds.Tables[sheetName] : ds.Tables[0];
        }

        public string Lookup(DataTable table, string rowKey, string colKey = "0")
        {
            DataRow row = table.AsEnumerable()
                .First(r => r["Total"].ToString() == rowKey);
            return row[colKey].ToString();
        }
    }

    class Participant
    {
        public Hand Hand = new Hand();
    }

    class Player : Participant
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


    class Game
    {
        private int numRounds;
        private int numDecks;
        private int cash;
        private int betAmount;
        private bool doubleDownAllowed;
        private bool surrenderAllowed;
        private DeckManager deck;
        private BasicStrategy strategy = new BasicStrategy();
        private Player player = new Player();
        private Participant dealer = new Participant();

        private Game(int NumDecks, int NumRounds, int Cash, int BetAmount, bool DoubleDownAllowed, bool SurrenderAllowed)
        {
            numDecks = NumDecks;
            numRounds = NumRounds;
            cash = Cash;
            betAmount = BetAmount;
            doubleDownAllowed = DoubleDownAllowed;
            surrenderAllowed = SurrenderAllowed;
            deck = new DeckManager(NumDecks);

            

            for (int i = 0; i < numRounds; i++)
            {
                PlayRound();
            }

        }


        public string PlayRound()
        {
            string playerDecision;
            string dealerDecision;
            string result;

            player.Hand.Clear();
            dealer.Hand.Clear();
            deck.ShuffleDeck();

            player.Hand.Add(deck.DrawCards(2));
            dealer.Hand.Add(deck.DrawCards(1));

            do
            {
                playerDecision = strategy.Decision("Player", player.Hand.Type(), dealer.Hand.Value(), player.Hand.Value());

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

            return result;
        }
    }

    class Bank
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


