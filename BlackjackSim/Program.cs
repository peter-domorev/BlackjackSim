using BlackjackSim;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace BlackjackSim
{
    internal class Program
    {
        static void Main(string[] args)
        {


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
    }


    class Hand
    {
        public List<string> Cards = new List<string>();

        public void Add(List<string> newCards)
        {
            Cards.AddRange(newCards);
        }

        public void Clear()
        {
            Cards = [];
        }

        public int[] Value()
        {
            int total = 0;

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
                return new int[] { minTotal, maxTotal };
            }
            else
            {
                return new int[] { total };
            }
        }
    }

    class Participant
    {

        public Hand Hand
        {
            get;
        }

        public Participant()
        {
            Hand Hand = new Hand();     
        }

    }

    class Player : Participant
    {
        private int profit = 0;
        private int betAmount;

        public Player(int Cash, int BetAmount)
        {
            profit = Cash;
            betAmount = BetAmount;
        }

        public int BetAmount
        {
            get { return betAmount; }
        }

        public int Profit
        {
            get { return profit; }
            set { profit = value; }
        }
    }

    class Dealer : Participant
    {
        public string Action()
        {
            string decision;


            return decision;
        }
    }


    class Game
    {
        private int numRounds;
        private int numDecks;
        private int cash;
        private int betAmount;
        private DeckManager deck;
        private Player player;
        private Dealer dealer;

        private Game(int NumDecks, int NumRounds, int Cash, int BetAmount)
        {
            numDecks = NumDecks;
            numRounds = NumRounds;
            cash = Cash;
            betAmount = BetAmount;
            deck = new DeckManager(NumDecks);

            StartGame(cash, betAmount);

            for (int i = 0; i < numRounds; i++)
            {
                PlayRound();
            }

        }

        public void StartGame(int Cash, int BetAmount)
        {
            player = new Player(Cash, BetAmount);
            dealer = new Dealer();


        }

        public void PlayRound()
        {
            player.Hand.Add(deck.DrawCards(2));
            dealer.Hand.Add(deck.DrawCards(1));

            
        }
    }

    class Bank
    {

    }
}


