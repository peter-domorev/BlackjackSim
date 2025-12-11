using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class Deck : IDeck
    {
        private List<string> _cards;
        private int _numDecks;
        private Random _rng = new Random();

        public Deck(int NumDecks = 1)
        {
            _numDecks = NumDecks;
            BuildNewDeck();
        }


        private void BuildNewDeck()
        {
            string[] oneSuit = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };


            // create single deck
            string[] singleDeck = new string[52];
            for (int i = 0; i < 4; i++)
            {
                Array.Copy(oneSuit, 0, singleDeck, i * oneSuit.Length, oneSuit.Length);
            }

            // create all decks
            string[] allCardsArray = new string[52 * _numDecks];
            for (int i = 0; i < _numDecks; i++)
            {
                Array.Copy(singleDeck, 0, allCardsArray, i * singleDeck.Length, singleDeck.Length);
            }

            _cards = allCardsArray.ToList();
        }


        public List<string> Deal(int numCards)
        {
            List<string> dealtCards = new List<string>(numCards);

            for (int i = 0; i < numCards; i++)
            {
                int index = _rng.Next(_cards.Count);
                
                dealtCards.Add(_cards[index]);
                _cards.RemoveAt(index);
            }
            return dealtCards;
        }


        public void Shuffle()
        {
            BuildNewDeck();
        }
    }

}
