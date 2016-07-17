using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Game
{
    /// <summary>
    /// Holds the lisf of cards for a hand
    /// </summary>
    class Hand
    {
        //change this is the number of cards per hand is differnet than 5
        private int maxNumberofCards;
        //Current hand for the user
        //thought about using a stack for this, but need to maintain the order and the cards for further itterations, so it makes sence to keep as an array
        private Card[] cardHand;
        

        public Hand(int maxNumberofCards_)
        {
            
            maxNumberofCards = maxNumberofCards_ ;
            cardHand = new Card[maxNumberofCards];
            for( int i =0;i< maxNumberofCards; i++)
            {
                cardHand[i] = new Card();
            }
        }
        public void SetHand(Card[] newHand)
        {
            if(newHand.Length < 5)
            {
                throw new System.ArgumentException("Length of Submitted cannot be < 5", "Card[].Length < 5");
            }else if (newHand.Length > 5)
            {
                throw new System.ArgumentException("Length of Submitted cannot be > 5", "Card[].Length > 5");
            }
            cardHand = newHand;
        }
        /// <summary>
        /// This assigns a card to the hand
        /// it checks to see if the card can be assigned, i.e. current # of cards less than 5 
        /// if that passes then we can add it otherwise is returns false to tell the game that 
        /// too many cards are divided out
        /// </summary>
        /// <param name="newCard"></param>
        public bool SetCard(Card newCard)
        {

            //check current position if the next avalible spot
            for(int i = 0;i< maxNumberofCards; i++)
            {
                if(cardHand[i].Suit() == ' ' )
                {
                    //if there is a space then we assign the new card to this position
                    cardHand[i].Set(newCard);
                    return true;
                }
            }
            return false; //there is not a open space for another card
            
        }

        public Card[] GetCards()
        {
            return cardHand;
        }
        /// <summary>
        /// Displays the current hand the this hand
        /// </summary>
        public void ShowCards()
        {
            for(int i = 0;i < maxNumberofCards; i++)
            {
                Console.Write(System.Convert.ToString(cardHand[i].Value()) + cardHand[i].Suit() + ", ");
            }
        }
        /// <summary>
        /// Clears out the cards in this hand
        /// </summary>
        public void Clear()
        {
            for(int i = 0; i < maxNumberofCards; i++)
            {
                cardHand[i].Clear();
            }
        }

    }
}
