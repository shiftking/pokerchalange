using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Game
{
    /// <summary>
    /// represent a standard playing card, with a suit and value
    /// </summary>
    class Card
    {
        private char suit;
        private int value;
        public Card()
        {
            suit = ' ';
            value = 14;
        }
        public Card(char suit_, int value_)
        {
            suit = suit_;
            value = value_;
        }
        /// <summary>
        /// sets the value and the suite of the card
        /// </summary>
        /// I was going to set the value at the card level, but proved to be 
        /// so I will be handling the dealing at the Game level instead
        /// <summary>
        /// </summary>
        /// <param name="suite_">suit of card</param>
        /// <param name="value_">value of the suite</param>
        public void Set(char suit_,int value_)
        {
            suit = suit_;
            value = value_;
        }
        /// <summary>
        /// Sets the value of the Card object from a exsisting card
        /// </summary>
        /// <param name="newCard">Card to be copied</param>
        public void Set(Card newCard)
        {
            suit = newCard.Suit();
            value = newCard.Value();
        }
        /// <summary>
        /// This retrives the suite of the current card
        /// </summary>
        /// <returns>suit of the card</returns>
        public char Suit()
        {
            return suit;
        }
        /// <summary>
        /// This retrives the value of the card 
        /// </summary>
        /// <returns>value of the suit</returns>
        public int Value()
        {
            return value;
        }
        public void Clear()
        {
            suit = ' ';
            value = 0;
        }
    }
       
}
