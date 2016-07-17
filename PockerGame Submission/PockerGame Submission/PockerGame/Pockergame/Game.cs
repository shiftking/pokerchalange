using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// This represents an entire game of cards(poker)
    /// </summary>


    class PockerGame
    {
        // Each game has a list of players
        // and each player has a hand
        // and the hand is a list of cards with the approriate suit and value 
        // in this game we are assuming the ace is low or high given the context, i.e. A-2-3-4-5, or 10-J-Q-K-A


        //thought of creating a list of all possible cards 
        List<Card> cardsplayed; // holder of all of the cards that be played
        //int numberofCardsPlayed; // keeps track of the number of cards added to the play stack
        private Player[] players;
        private int numberofPlayers;
        private int maxNumberofCards;
        static char[] suits = { 'D', 'C', 'H', 'S' };

        /// <summary>
        /// constructer for creating a new game
        /// </summary>
        /// <param name="numberofPlayers_">Number players in the this game</param>
        /// <param name="maxNumberofCards_">Number of cards per hand in this game</param>
        public PockerGame(int numberofPlayers_, int maxNumberofCards_)
        {
            maxNumberofCards = maxNumberofCards_;
            numberofPlayers = numberofPlayers_;
            players = new Player[numberofPlayers];
            for (int i = 0; i < numberofPlayers; i++)
            {
                Console.WriteLine("Please enter a name for the new player");
                players[i] = new Player(Console.ReadLine(), maxNumberofCards);
            }
            cardsplayed = new List<Card>();
            //numberofCardsPlayed = 0;
        }

        /// <summary>
        /// Deals out all of the cards for the current round, or hand to all players
        /// </summary>
        public Tuple<bool, string> deal()
        {
            for (int i = 0; i < numberofPlayers; i++)
            {
                players[i].Clear();
            }
            var response = new Tuple<bool, string>(false, " "); // temporary holder for response tuple
            //Randomly generate suite and card value
            //suits 1(diamonds: d), 2(clubs: c) , 3(hearts: h), 4(spades: s) 
            //value 1(ace) to 13(king)
            Card new_card = new Card();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < numberofPlayers; j++)
                {
                    //generate card and assign it
                    Random seed = new Random(); //seed for the random seletions
                    int new_suit_pos = seed.Next(0, 4); //random number between 0 and 3
                    int new_value = seed.Next(13);
                    new_card = new Card(suits[new_suit_pos], new_value);
                    //we only want one of each card to created, if we have already created the card then we need to create a new one
                    if (cardsplayed.Count >= 51)
                    {
                        return response = new Tuple<bool, string>(false, "All cards are spent, consider setting the deck and redealing");
                    }
                    else if (cardsplayed.Count < 1)
                    {
                        cardsplayed.Add(new_card);//if we havn;t delt any cards then we know that there is nothing to check so we place it in the first position
                    }
                    // now check if this card has been delt before if so then it keeps creating a card until it hasn't been delt, this can run for while, depending on the probablity of hitting the avalible card

                    while (BeenDelt(new_card))
                    {
                        seed = new Random();
                        new_card = new Card();
                        new_suit_pos = seed.Next(0, 4);
                        new_value = seed.Next(0, 13);
                        new_card.Set(suits[new_suit_pos], new_value);
                    }

                    cardsplayed.Add(new_card); //update the list of used cards to hold the most recent one that was delt

                    if (!players[j].SetCard(new_card))
                    {

                        return response = new Tuple<bool, string>(false, "Player hand is full");
                    }






                }

            }
            return response = new Tuple<bool, string>(true, "Nothing to report");
        }
        /// <summary>
        /// resets the deck for a new game
        /// </summary>
        public void ResetDeck()
        {
            cardsplayed = new List<Card>();
            //numberofCardsPlayed = 0;
            for (int i = 0; i < numberofPlayers; i++)
            {
                players[i].Clear();
            }
        }
        /// <summary>
        /// Displays the current hand that all players have
        /// </summary>
        public void ShowHands()
        {
            for (int i = 0; i < numberofPlayers; i++)
            {
                players[i].ShowHand();
            }
        }

        public Player[] GetPlayers()
        {
            return players;
        }
        /// <summary>
        /// internal function the determines if the new card has been delt
        /// </summary>
        /// <param name="newCard"></param>
        /// <returns></returns>
        private bool BeenDelt(Card newCard)
        {
            for (int i = 0; i < cardsplayed.Count; i++)
            {
                if (cardsplayed[i].Suit() == newCard.Suit() && cardsplayed[i].Value() == newCard.Value())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Ends a current game
        /// </summary>
        public void End()
        {
            //display winner with most points
            ResetDeck();
        }
    }
}
