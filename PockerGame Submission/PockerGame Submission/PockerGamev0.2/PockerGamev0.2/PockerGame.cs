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
        /// This determins the winner of every round, currently only supporting Flush, Three of a kind, Pair, and High Card
        /// </summary>
        /// <returns></returns>
        /// 
        public Tuple<bool, Card> IsFlush(Player currentPlayer)
        {

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
        public Tuple<bool, Card> IsThreeOfKind(Player currentPlayer)
        {
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
        /// <param name="currentPlayer"></param>
        /// <returns></returns>
        public Tuple<bool, Card> IsPair(Player currentPlayer)
        {
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


        private Card GetHighestCard(Player currentPlayer)
        {
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
        /// <returns>returns a tuple of it succesfully determining the correct number of winners, the list of players that won, and description for a failed event</returns>
        public Tuple<bool, List<Player>, string> DecideWinner(Player[] players)
        {
            /*THIS IS TO ADDED IF THE MAIN PROGAM ASSUMTIONS ARE NOT USED
            //assumes that all hands are the same lenghth of 5 cards
            //if(players.Length > 9)
            //{
            //    return new Tuple<bool, List<Tuple<Player, int>>, string>(false, null, "Too many Players");
            //}
            //if(players[0].GetHand().Length > 5)
            //{
            //    return new Tuple<bool, List<Tuple<Player, int>>, string>(false, null, "Too many cards in hands");
            //}
            //Card[] Deck =  new Card[players.Length*players[0].GetHand().Length];
            //int counter = 0;
            //for(int i = 0;i < players.Length; i++)
            //{
            //    Card[] addedCards = players[i].GetHand();
            //    for(int j = 0 ;j < addedCards.Length; j++)
            //    {
            //        Deck[counter] = addedCards[j];
            //        counter++;
            //    }
                
            //}*/
            List<Player> playerList;
            List<Tuple<Player, Card, int>> winningPlayers = new List<Tuple<Player, Card, int>>();
            for (int i = 0; i < players.Length; i++)
            {
                //check to see if the entered players have the correct cards
                
                if (!ValidateHand(players[i]).Item1)
                {
                    return new Tuple<bool, List<Player>, string>(false, null, ValidateHand(players[i]).Item2);
                }
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
                return new Tuple<bool, List<Player>, string>(true, playerList, "winning players");
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
                return new Tuple<bool, List<Player>, string>(true, playerList, "winning players");
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
                return new Tuple<bool, List<Player>, string>(true, playerList, "winning players");
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
            return new Tuple<bool, List<Player>, string>(true, currentHighest, "Found a winner");

        }
        //check





        /// <summary>
        /// Checks to see if the passed player has a valid hand
        /// </summary>
        /// <param name="player">player for which the hand needs to be checked</param>
        /// <returns></returns>
        private Tuple<bool, string> ValidateHand(Player player)
        {
            Card[] currentHand = player.GetHand();
            for (int i = 0; i < currentHand.Length; i++)
            {
                //check if the suit is correct
                if (currentHand[i].Suit() != 'D' && currentHand[i].Suit() != 'C' && currentHand[i].Suit() != 'H' && currentHand[i].Suit() != 'S')
                {
                    return new Tuple<bool, string>(false, "Incorrect Suite");
                }
                //check to see if the value is in the appropriate range
                if (currentHand[i].Value() < 0 || currentHand[i].Value() > 12)
                {
                    return new Tuple<bool, string>(false, "Incorrect Value");
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
                        return new Tuple<bool, string>(false, "Multiple Cards of the same suite and value");
                    }
                    if (currentHand[i].Suit() != currentHand[j].Suit() && currentHand[i].Value() == currentHand[j].Value())
                    {
                        sameValueCount++;
                    }
                }
                if (sameValueCount > 4)
                {
                    return new Tuple<bool, string>(false, "Multiple Cards of differnet suit, but same value");
                }
            }


            return new Tuple<bool, string>(true, "Cards are okay");
        }


        /// <summary>
        /// Sorts the cards in either by suite or by value
        /// </summary>
        /// <param name="currentCards">array to sort</param>
        /// <param name="bySuite">either doring by suit or value</param>
        /// <returns></returns>
        private Card[] SortCards(Card[] currentCards)
        {


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
