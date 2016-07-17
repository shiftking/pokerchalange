using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// Player class contains a hand for the game of cards they are playing
    /// </summary>
    class Player
    {
        private Hand playerHand; //keeps track of the users current hand
        private string playerName;
        int score; //keeps track of the current users score, # of times they have won during that game
        /// <summary>
        /// initilize the score for the player
        /// </summary>
        public Player(string playerName_,int maxNumberofCards_)
        {
            playerHand = new Hand(maxNumberofCards_);
            score = 0;
            playerName = playerName_;

        }

        public void SetHand(Card[] newHand)
        {
            playerHand.SetHand(newHand);
        }

        /// <summary>
        /// Sets the next availible card spot for the user
        /// </summary>
        /// <param name="new_card">A new card to add to the hand</param>
        /// <returns>true if it cand add it, else false</returns>



        public bool SetCard(Card new_card)
        {
            return playerHand.SetCard(new_card); 
        }
        /// <summary>
        /// Incriments the players current score when then win
        /// </summary>
        public void IncrimentScore()
        {
            score++;
        }
        /// <summary>
        /// Clears the current hand of this user
        /// </summary>
        public void Clear()
        {
            playerHand.Clear();
        }
        /// <summary>
        /// Shows the hand of this user
        /// </summary>
        public void ShowHand()
        {
            Console.Write(playerName + " ");
            playerHand.ShowCards();
            Console.WriteLine("");
        }
        /// <summary>
        /// Returns a list of cards in the current users hand
        /// </summary>
        /// <returns></returns>
        public Card[] GetHand()
        {
            return playerHand.GetCards();
        }
        /// <summary>
        /// returns the active players name
        /// </summary>
        /// <returns></returns>
        public string GetPlayerName()
        {
            return playerName;
        }
    }

}
