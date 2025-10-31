using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackSim
{
    class Deck
    {
        private List<string> allCards;
        private Random rng = new Random();
        private int numDecks;


        public Deck(int NumDecks)
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

}
