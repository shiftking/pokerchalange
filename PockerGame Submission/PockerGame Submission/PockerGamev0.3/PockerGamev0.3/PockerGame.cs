using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Game
{
    class PockerGame
    {




        /// <summary>
        /// Determins if the player hand is flush
        /// </summary>
        /// <returns>Returns a Tuple that holds the results, either pass/fail, pass also returns the highest card of the hand, fail does not return a valid card</returns>
        /// 
        public Tuple<bool, Card> IsFlush(Player currentPlayer)
        {
            if(currentPlayer == null)
            {
                throw new System.ArgumentException("Parameter cannot bo null", "Player");
            }
            Card[] currentHand = currentPlayer.GetHand();
            for (int i = 0; i < currentHand.Length - 1; i++)
            {
                if (currentHand[i].Suit() != currentHand[i + 1].Suit())
                {
                    return new Tuple<bool, Card>(false, null);
                }
            }
            return new Tuple<bool, Card>(true, currentHand[0]);
        }
        /// <summary>
        /// Determins if the players hand has a three of a kind
        /// </summary>
        /// <param name="currentPlayer">Player object to determine what kind of hand it had</param>
        /// <returns>Returns a Tuple that holds the results, either pass/fail, pass also returns the card for the three of a kind, fail does not return a valid card</returns>
        public Tuple<bool, Card> IsThreeOfKind(Player currentPlayer)
        {
            if (currentPlayer == null)
            {
                throw new System.ArgumentException("Parameter cannot bo null", "Player");
            }
            int sameValue = 0;
            Card[] currentHand = SortCards(currentPlayer.GetHand());
            for (int i = 0; i < currentHand.Length - 1; i++)
            {
                if (currentHand[i].Value() == currentHand[i + 1].Value())
                {
                    sameValue++;
                    if (sameValue == 2)
                    {
                        return new Tuple<bool, Card>(true, currentHand[i]);
                    }
                }
                else
                {
                    sameValue = 0;
                }
            }
            return new Tuple<bool, Card>(false, null);
        }
        /// <summary>
        /// Returns true and the value of the card it has multiples of
        /// </summary>
        /// <param name="currentPlayer">Player object to determine what kind of hand it had</param>
        /// <returns>Returns a Tuple that holds the results, either pass/fail, pass also returns the card that is a pair, fail does not return a valid card</returns>
        public Tuple<bool, Card> IsPair(Player currentPlayer)
        {
            if (currentPlayer == null)
            {
                throw new System.ArgumentException("Parameter cannot bo null", "Player");
            }
            int sameValue = 0;
            Card[] currentHand = SortCards(currentPlayer.GetHand());
            for (int i = 0; i < currentHand.Length - 1; i++)
            {
                if (currentHand[i].Value() == currentHand[i + 1].Value())
                {
                    sameValue++;
                    if (sameValue == 1)
                    {
                        return new Tuple<bool, Card>(true, currentHand[i]);
                    }
                }
                else
                {
                    sameValue = 0;
                }
            }
            return new Tuple<bool, Card>(false, null);
        }
        /// <summary>
        /// This method determins the highest card of a players hand
        /// </summary>
        /// <param name="currentPlayer">Player object to determine what kind of hand it had</param>
        /// <returns>Returns a Tuple that holds the results, either pass/fail, pass also returns the card that is the highest card, fail does not return a valid card</returns>

        private Card GetHighestCard(Player currentPlayer)
        {
            if (currentPlayer == null)
            {
                throw new System.ArgumentException("Parameter cannot bo null", "Player");
            }
            Card[] currentHand = currentPlayer.GetHand();
            Card currentHighest = currentHand[0];
            for (int i = 1; i < currentHand.Length; i++)
            {

                if (currentHighest.Value() < currentHand[i].Value()) // if the value is higher than current, then we change the value of the current highest
                {
                    currentHighest = currentHand[i];
                }

            }
            return currentHighest;
        }

        

        /// <summary>
        /// Determines the winner of a group of players based on Five card Pocker
        /// </summary>
        /// <param name="players">List of active players</param>
        /// <returns>List of players that won that round</returns>
        public List<Player> DecideWinner(Player[] players)
        {
            
            if(players.Length < 2)
            {
                throw new System.ArgumentException("Array must be greater than 1", "Player[]");
            }
            List<Player> playerList;
            List<Tuple<Player, Card, int>> winningPlayers = new List<Tuple<Player, Card, int>>();
            for (int i = 0; i < players.Length; i++)
            {
                //check to see if the entered players have the correct cards

                ValidateHand(players[i]);
                
                //check if there is a flush in the hand
                Tuple<bool, Card> flushResults = IsFlush(players[i]);
                if (flushResults.Item1)
                {
                    Card newHighestCard = GetHighestCard(players[i]);
                    if (winningPlayers.Count == 0)
                    {
                        winningPlayers.Add(new Tuple<Player, Card, int>(players[i], newHighestCard, 0));
                    }
                    else
                    {

                        int previousCount = winningPlayers.Count;
                        for (int j = 0; j < previousCount; j++)
                        {
                            if (winningPlayers[j].Item2.Value() < newHighestCard.Value())
                            {
                                winningPlayers.Clear(); //we remove the less winning hand from the winning hand list
                                winningPlayers.Add(new Tuple<Player, Card, int>(players[i], newHighestCard, 0));
                            }
                            else if (winningPlayers[j].Item2.Value() == GetHighestCard(players[i]).Value())
                            { //if the value is the same then we need to check the next highest card
                                Card[] potentialWinninghand = SortCards(winningPlayers[j].Item1.GetHand());
                                Card[] handToCompare = SortCards(players[i].GetHand());
                                bool replace = false; //if we swapped the two hands or not
                                for (int k = 1; k < potentialWinninghand.Length; k++) //start at one cause we already checked the highest
                                {
                                    if (potentialWinninghand[k].Value() < handToCompare[k].Value())
                                    {
                                        replace = true;
                                        winningPlayers.Clear();
                                        winningPlayers.Add(new Tuple<Player, Card, int>(players[i], newHighestCard, 0));
                                    }
                                    else if (potentialWinninghand[k].Value() > handToCompare[k].Value())
                                    {
                                        replace = true; //means that they are not the same and the one we are comparing to the recorded winning hand lost
                                    }

                                }
                                if (!replace) //this only passes if they are the same value all the way thourgh the hand
                                {
                                    winningPlayers.Add(new Tuple<Player, Card, int>(players[i], newHighestCard, 0));
                                }

                            }
                        }
                    }
                }

            }
            if (winningPlayers.Count != 0) //if this is at least 1 long, then we have our winners, past this point is a waste since nothing else can beat a flush
            {
                playerList = new List<Player>();
                for (int i = 0; i < winningPlayers.Count; i++)
                {
                    playerList.Add(winningPlayers[i].Item1);
                }
                return playerList;
            }//but if we havn't found a winner we need to check if we have three of a kind

            for (int i = 0; i < players.Length; i++)
            {
                Tuple<bool, Card> threeofKindResults = IsFlush(players[i]);
                if (threeofKindResults.Item1)
                {
                    //Card newHighestCard = GetHighestCard(players[i]);
                    if (winningPlayers.Count == 0)
                    {
                        winningPlayers.Add(new Tuple<Player, Card, int>(players[i], threeofKindResults.Item2, 1));
                    }
                    else
                    {
                        int previousCount = winningPlayers.Count;
                        for (int j = 0; j < previousCount; j++)
                        {
                            if (winningPlayers[j].Item2.Value() < threeofKindResults.Item2.Value())
                            {
                                winningPlayers.Clear(); //we remove the less winning hand from the winning hand list
                                winningPlayers.Add(new Tuple<Player, Card, int>(players[i], threeofKindResults.Item2, 1));
                            }
                            else if (winningPlayers[j].Item2.Value() == threeofKindResults.Item2.Value())
                            { //if the priority is the same then we need to check the highest card
                                Card[] potentialWinninghand = SortCards(winningPlayers[j].Item1.GetHand());
                                Card[] handToCompare = SortCards(players[i].GetHand());
                                bool replace = false; //if we swapped the two hands or not
                                for (int k = 0; k < potentialWinninghand.Length; k++)
                                {
                                    if (potentialWinninghand[k].Value() < handToCompare[k].Value())
                                    {
                                        replace = true;
                                        winningPlayers.Clear();
                                        winningPlayers.Add(new Tuple<Player, Card, int>(players[i], threeofKindResults.Item2, 1));
                                    }
                                    else if (potentialWinninghand[k].Value() >  handToCompare[k].Value())
                                    {
                                        replace = true;
                                    }

                                }
                                if (!replace) //this only passes if they are the same value all the way thourgh the hand
                                {
                                    winningPlayers.Add(new Tuple<Player, Card, int>(players[i], threeofKindResults.Item2, 1));
                                }

                            }
                        }
                    }
                }
            }
            if (winningPlayers.Count != 0) //if this is at least 1 long, then we have our winners, past this point is a waste since nothing past this point can beat a three of a kind
            {
                playerList = new List<Player>();
                for (int i = 0; i < winningPlayers.Count; i++)
                {
                    playerList.Add(winningPlayers[i].Item1);
                }
                return playerList;
            }//but if we havn't found a winner we need to check if we have a pair
            for (int i = 0; i < players.Length; i++)
            {
                Tuple<bool, Card> pairKindResults = IsPair(players[i]);
                if (pairKindResults.Item1)
                {
                    if (winningPlayers.Count == 0)
                    {
                        winningPlayers.Add(new Tuple<Player, Card, int>(players[i], pairKindResults.Item2, 2));
                    }
                    else
                    {
                        int previousCount = winningPlayers.Count;
                        for (int j = 0; j < previousCount; j++)
                        {
                            if (winningPlayers[j].Item2.Value() < pairKindResults.Item2.Value())
                            {
                                winningPlayers.Clear(); //we remove the less winning hand from the winning hand list
                                winningPlayers.Add(new Tuple<Player, Card, int>(players[i], pairKindResults.Item2, 2));
                            }
                            else if (winningPlayers[j].Item2.Value() == pairKindResults.Item2.Value())
                            { //if the priority is the same then we need to check the highest card
                                Card[] potentialWinninghand = SortCards(winningPlayers[j].Item1.GetHand());
                                Card[] handToCompare = SortCards(players[i].GetHand());
                                bool replaced = false; //if we swapped the two hands or not
                                for (int k = 0; k < potentialWinninghand.Length; k++)
                                {
                                    if (potentialWinninghand[k].Value() < handToCompare[k].Value()) //if this passes then we know that the current highst hand is not the highest
                                    {
                                        replaced = true;
                                        winningPlayers.Clear();
                                        winningPlayers.Add(new Tuple<Player, Card, int>(players[i], pairKindResults.Item2, 2));
                                    }
                                    else if (potentialWinninghand[k].Value() > handToCompare[k].Value())
                                    {
                                        replaced = true; 
                                    }

                                }
                                if (!replaced) //this only passes if they are the same value all the way thourgh the hand
                                {
                                    winningPlayers.Add(new Tuple<Player, Card, int>(players[i], pairKindResults.Item2, 2));
                                }

                            }
                        }
                    }
                }
            }
            if (winningPlayers.Count != 0) //if this is at least 1 long, then we have our winners, past this point is a waste since nothing past this point can beat a pair 
            {
                playerList = new List<Player>();
                for (int i = 0; i < winningPlayers.Count; i++)
                {
                    playerList.Add(winningPlayers[i].Item1);
                }
                return playerList;
            }//but if we havn't found a winner we need to check if we have a high card winner
             // 

            List<Player> currentHighest = new List<Player>();
            currentHighest.Add(players[0]);
            for (int i = 1; i < players.Length; i++)
            {
                for (int j = 0; j < currentHighest.Count; j++)
                {
                    Card newHighestCard = GetHighestCard(players[i]);
                    Card currentHighestCard = GetHighestCard(currentHighest[j]);
                    if (GetHighestCard(currentHighest[j]).Value() < newHighestCard.Value())
                    {
                        currentHighest.Clear();
                        currentHighest.Add(players[i]);
                    }
                    else if (currentHighestCard.Value() == newHighestCard.Value())
                    {
                        Card[] potentialWinninghand = SortCards(currentHighest[j].GetHand());
                        Card[] handToCompare = SortCards(players[i].GetHand());
                        bool replaced = false; //if we swapped the two hands or not
                        for (int k = 0; k < potentialWinninghand.Length; k++)
                        {
                            if (potentialWinninghand[k].Value() < handToCompare[k].Value()) //if this passes then we know that the current highst hand is not the highest
                            {
                                replaced = true;
                                currentHighest.Clear();
                                currentHighest.Add(players[i]);
                            }
                            else if (potentialWinninghand[k].Value() > handToCompare[k].Value())
                            {
                                replaced = true; //this doesn't really make logical sence since we didn't swap anything, we just need to get out of the for loop cause the recorded hand wins
                            }

                        }
                        if (!replaced) //they are the same value because we didn;t swapp anything
                        {
                            currentHighest.Add(players[i]);
                        }
                    }
                }

            }
            return currentHighest;

        }
        //check





        /// <summary>
        /// Checks to see if the passed player has a valid hand
        /// </summary>
        /// <param name="player">player that need to be validatated</param>
        /// <returns>returns true if the submitted hand meets the requirements of the validation program</returns>
        private bool ValidateHand(Player player)
        {
            if(player == null)
            {
                throw new System.ArgumentException("Argument cannot be null", "Player");
            }
            Card[] currentHand = player.GetHand();
            if(currentHand.Length < 5)
            {
                throw new System.FieldAccessException("Field is not propperly set up, Player.playerHand.Length < 5");
            }
            else if(currentHand.Length > 5)
            {
                throw new System.FieldAccessException("Field is not propperly set up, Player.playerHand.Length > 5");
            }
            for (int i = 0; i < currentHand.Length; i++)
            {
                //check if the suit is correct
                if (currentHand[i].Suit() != 'D' && currentHand[i].Suit() != 'C' && currentHand[i].Suit() != 'H' && currentHand[i].Suit() != 'S')
                {
                    throw new System.ArgumentException("Argument is not in correct range", "Suit");
                }
                //check to see if the value is in the appropriate range
                if (currentHand[i].Value() < 0 || currentHand[i].Value() > 12)
                {
                    throw new System.ArgumentException("Argument is not in correct range", "Value");
                }
            }

            currentHand = SortCards(player.GetHand());
            //now check if there is a correct number of same values <= 4, and that the suits dont match if the values do
            int sameValueCount; //keeps track of the similar value
            for (int i = 0; i < currentHand.Length; i++)
            {
                sameValueCount = 0;
                for (int j = i + 1; j < currentHand.Length; j++)
                {
                    if (currentHand[i].Value() == currentHand[j].Value() && currentHand[i].Suit() == currentHand[j].Suit())
                    {
                        throw new System.ArgumentException("Card has duplicates", "Multiple Suits");
                    }
                    if (currentHand[i].Suit() != currentHand[j].Suit() && currentHand[i].Value() == currentHand[j].Value())
                    {
                        sameValueCount++;
                    }
                }
                if (sameValueCount > 4)
                {
                    throw new System.ArgumentException("Card has duplicates", "Multiple Value");
                }
            }


            return true;
        }


        /// <summary>
        /// Sorts the cards by value
        /// </summary>
        /// <param name="currentCards">array to sort</param>
        /// <returns>returns an array of sorted cards</returns>
        private Card[] SortCards(Card[] currentCards)
        {

            //this should never trip, because I validate the hand before, but for safety still including it

            for (int i = 1; i < currentCards.Length; i++)
            {
                for (int j = 0; j < currentCards.Length - i; j++)
                {
                    //the lower the index the higher the priority of the that suite
                    if (currentCards[j].Value() < currentCards[j + 1].Value())
                    {
                        //swap the values
                        Card temp = currentCards[j];
                        currentCards[j] = currentCards[j + 1];
                        currentCards[j + 1] = temp;

                    }
                }
            }

            return currentCards;
        }
    }
      

    
}
