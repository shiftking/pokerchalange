using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

namespace PockerGamev0._2
{
    class Program
    {
        static void Main(string[] args)
        {

            //this program is designed to showcase a pocker round
            //the function takes a array of players that all have a hand delt to them, it then returns a List of Players(winners)
            //Assumptions:
                //-Player array has been initialized,
                //-Player array is greater than 1 player
                //-Cards for each of players in the list is 5 cards
                //-value of the cards for each hand of the player in player list is either(D,H,S,C) and Range(0-12) - 12 being ace
                //
            //
            //WIN CONDITIONS
                //for a flush tie, cards ranking is considered, so highest card wins, if that is a tie, the next highest card is considered, if they get to the end of the hand, they tie
                //for three of a kind, Highest card wins, since all of the cards are unique, there wont be a chance to have the same three cards in two or more hands
                //for two of a kind, if that is a tie, the next highest card is considered, if they get to the end of the hand, they tie
                //for High card tie, the next highest card is considered, if they get to the end of the hand, they tie

            //Cards:
            // Suits: 
            //   Diamons, D
            //   Clubs C, 
            //   Hearts, H
            //   Spades, S

            // Values:
            //   0, 2
            //   1, 3
            //   2, 4
            //   3, 5
            //   4, 6
            //   5, 7
            //   6, 8
            //   7, 9
            //   8, 10
            //   9, J
            //   10,Q
            //   11,K 
            //   12,A
            bool main_done = false;

            while (!main_done)
            {
                Console.WriteLine("Welcome to the Pocker Game Chalange, this program is designed to show how well a winning determining engine works");
                Console.WriteLine("For simulating a real game enter 'real'");
                            
                Console.WriteLine("For using hard coded cases enter 'test1'");
                Console.WriteLine("To quit enter 'quit'");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "real":
                    case "Real":

                        //section one simulates a game with a limited amount of rounds depending on the number of players, amount of delt cards is limited to 52

                        //these hands are all pulled from the same deck, that assumes that there are no duplicates 
                        //of cards in any of the hands, also assuming that the total number cards delt to all of the users does not exceed 52
                        //that means that # of players divided by five is less than 52,
                        //also that there can only be no more than 52 cards out before a reset.k
                        Console.WriteLine("How Many players in this game?");
                        int value;

                        value = Int32.Parse(Console.ReadLine());
                        Dealer dealer = new Dealer(value, 5);
                        PockerGame pockergame = new PockerGame();
                        bool done = false;
                        char response = ' ';
                        while (!done)
                        {
                            //Console.Clear(); //clear the console for better formating
                            Console.WriteLine("Here are the options of the game");
                            Console.WriteLine("Please enter the option: 'd' for dealing a new set of hands to the players");
                            Console.WriteLine("Please enter the option: 'q' to quit the current game");
                            Console.WriteLine("Please enter the option: 'r' to restart the current game");

                            response = System.Convert.ToChar(Console.ReadLine()); // get the response of the user for the question below


                            switch (response)
                            {
                                case 'd':
                                    //Tuple<bool, string> dealResponse = game.deal();
                                    if (!dealer.deal().Item1)//if this passes then something went wrong
                                    {
                                        Console.WriteLine(dealer.deal().Item2);
                                        while (response != 'q' && response != 'r')
                                        {
                                            Console.WriteLine("Would you like to restart 'r' or quit 'q'?");
                                            response = System.Convert.ToChar(Console.ReadLine());
                                        }
                                        switch (response)
                                        {
                                            case 'r':
                                                dealer.ResetDeck();
                                                break;
                                            case 'q':
                                                dealer.End();
                                                done = true;
                                                break;
                                        }
                                    }
                                    else//else everything is fine
                                    {
                                        dealer.ShowHands();

                                        try
                                        {
                                            List<Player> real_results = pockergame.DecideWinner(dealer.GetPlayers());
                                            for (int i = 0; i < real_results.Count; i++)
                                            {
                                                Console.WriteLine("The winner of the round is {0}", real_results[i].GetPlayerName());
                                            }
                                        }
                                        catch(Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                        //game.DetermineWinner();
                                    }
                                    break;
                                case 'q':
                                    dealer.End();
                                    done = true;
                                    break;
                                case 'r':
                                    dealer.ResetDeck();
                                    break;
                                default:
                                    Console.WriteLine("Seems like you didn't enter a value that was expected, try again");
                                    done = false;
                                    break;


                            }

                        }
                        break;
                    case "test1":
                    case "test":
                    case "Test1":
                    case "Test":
                    //section two is the bulk testing section were more specific testing takes place, either user provided tests, or hard coded tests
                    //
                    //flush heart, if there is two flushes then the highest card of the two wins
                    Player[] game_players;
                    PockerGame game;
                    Player p1;
                    Player p2;
                    List<Player> results;
                    //flush heart, if there is two flushes then the highest card of the two wins
                    List<Card> HeartFlush_4High = new List<Card>
                    {
                        new Card('H',0),
                        new Card('H',1),
                        new Card('H',3),
                        new Card('H',4),
                        new Card('H',5),
                    };
                    List<Card> HeartFlush_5High = new List<Card>
                    {
                        new Card('H',1),
                        new Card('H',2),
                        new Card('H',3),
                        new Card('H',4),
                        new Card('H',5),
                    };
                    
                    
                    try
                    {
                        p1 = new Player("Dylan", 5);
                        p1.SetHand(HeartFlush_4High.ToArray());
                        //
                        p2 = new Player("Taylor", 5);
                        p2.SetHand(HeartFlush_5High.ToArray());
                        game_players = new Player[2] { p1, p2 };
                        game = new PockerGame();
                        Console.WriteLine("Winner of this round is going to be {0}", p2.GetPlayerName());
                        results = game.DecideWinner(game_players);
                            for (int i = 0; i < results.Count; i++)
                        {
                            Console.WriteLine("Winner of the round is {0}", results[i].GetPlayerName());
                        }

                    }
                    catch(Exception e )
                    {
                        Console.WriteLine(e.Message);
                    }

                        //three of a kind, one player has a higher card
                        List<Card> ThreeOfKind_5High_10High = new List<Card>
                    {
                        new Card('C',5),
                        new Card('H',5),
                        new Card('D',5),
                        new Card('S',0),
                        new Card('H',10),
                    };
                        List<Card> ThreeOfKind_5High_4High = new List<Card>
                    {
                        new Card('C',5),
                        new Card('H',5),
                        new Card('D',5),
                        new Card('S',0),
                        new Card('H',4),
                    };
                    
                   
                        try
                        {
                            p1 = new Player("Dylan", 5);
                            p1.SetHand(ThreeOfKind_5High_10High.ToArray());
                            //
                            p2 = new Player("Taylor", 5);
                            p2.SetHand(ThreeOfKind_5High_4High.ToArray());
                            game_players = new Player[2] { p1, p2 };
                            game = new PockerGame();
                            Console.WriteLine("Winner of this round is going to be {0}", p2.GetPlayerName());
                            results = game.DecideWinner(game_players);
                            for (int i = 0; i < results.Count; i++)
                            {
                                Console.WriteLine("Winner of the round is {0}", results[i].GetPlayerName());
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        //one pair same value, and same high cards
                        List<Card> OnePair_5High_10High = new List<Card>
                    {
                        new Card('C',5),
                        new Card('H',5),
                        new Card('D',3),
                        new Card('S',4),
                        new Card('H',10),
                    };
                        List<Card> OnePair_6High_10High = new List<Card>
                    {
                        new Card('C',5),
                        new Card('H',5),
                        new Card('D',3),
                        new Card('S',4),
                        new Card('H',10),
                    };
                    
                    
                    try
                    {
                        p1 = new Player("Dylan", 5);
                        p1.SetHand(OnePair_5High_10High.ToArray());
                        //
                        p2 = new Player("Taylor", 5);
                        p2.SetHand(OnePair_6High_10High.ToArray());
                        game_players = new Player[2] { p1, p2 };
                        game = new PockerGame();
                        Console.WriteLine("Winner of this round is going to be {0} and {1}", p1.GetPlayerName(), p2.GetPlayerName());
                        results = game.DecideWinner(game_players);
                        for (int i = 0; i < results.Count; i++)
                        {
                            Console.WriteLine("Winner of the round is {0}", results[i].GetPlayerName());
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }


                        //High card test, not same hand
                        List<Card> AceHighCard = new List<Card>
                    {
                        new Card('C',12),
                        new Card('H',11),
                        new Card('D',10),
                        new Card('S',9),
                        new Card('H',8),
                    };
                        List<Card> AceHighCard_NextQueen = new List<Card>
                    {
                        new Card('C',12),
                        new Card('H',10),
                        new Card('D',9),
                        new Card('S',8),
                        new Card('H',7),
                    };
                    
                    try
                    {
                        p1 = new Player("Dylan", 5);
                        p1.SetHand(AceHighCard.ToArray());
                        //
                        p2 = new Player("Taylor", 5);
                        p2.SetHand(AceHighCard_NextQueen.ToArray());
                        game_players = new Player[2] { p1, p2 };
                        game = new PockerGame();
                        Console.WriteLine("Winner of this round is going to be {0}", p1.GetPlayerName());
                        results = game.DecideWinner(game_players);
                        for (int i = 0; i < results.Count; i++)
                        {
                            Console.WriteLine("Winner of the round is {0}", results[i].GetPlayerName());
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                        //incorrect hand for user, Suit
                        List<Card> BadCardSuit = new List<Card>
                    {
                        new Card('F',12),
                        new Card('H',11),
                        new Card('D',10),
                        new Card('S',9),
                        new Card('H',8),
                    };
                        List<Card> DummyValue = new List<Card>
                    {
                        new Card('C',12),
                        new Card('H',10),
                        new Card('D',9),
                        new Card('S',8),
                        new Card('H',7),
                    };
                    
                    try
                    {
                        p1 = new Player("Dylan", 5);
                        p1.SetHand(BadCardSuit.ToArray());
                        //
                        p2 = new Player("Taylor", 5);
                        p2.SetHand(DummyValue.ToArray());
                        game_players = new Player[2] { p1, p2 };
                        game = new PockerGame();
                        Console.WriteLine("This will fail, testing inccorect suit value");
                        results = game.DecideWinner(game_players);
                        for (int i = 0; i < results.Count; i++)
                        {
                            Console.WriteLine("Winner of the round is {0}", results[i].GetPlayerName());
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                        //inccorect Hand for user Value
                        List<Card> BadCardValue = new List<Card>
                    {
                        new Card('C',15),
                        new Card('H',11),
                        new Card('D',10),
                        new Card('S',9),
                        new Card('H',8),
                    };
                        List<Card> DummySuite = new List<Card>
                    {
                        new Card('C',12),
                        new Card('H',10),
                        new Card('D',9),
                        new Card('S',8),
                        new Card('H',7),
                    };
                   
                    try
                    {
                        p1 = new Player("Dylan", 5);
                        p1.SetHand(BadCardValue.ToArray());
                        //
                        p2 = new Player("Taylor", 5);
                        p2.SetHand(DummySuite.ToArray());
                        game_players = new Player[2] { p1, p2 };
                        game = new PockerGame();
                        Console.WriteLine("This will fail, testing inccorect values");
                        results = game.DecideWinner(game_players);
                        for (int i = 0; i < results.Count; i++)
                        {
                            Console.WriteLine("Winner of the round is {0}", results[i].GetPlayerName());
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                        //only one active player
                        List<Card> PlayerFail = new List<Card>
                    {
                        new Card('C',12),
                        new Card('H',10),
                        new Card('D',9),
                        new Card('S',8),
                        new Card('H',7),
                    };
                        
                        try
                        {
                            p1 = new Player("Dylan", 5);
                            p1.SetHand(BadCardValue.ToArray());
                            //

                            game_players = new Player[1] { p1 };
                            game = new PockerGame();
                            Console.WriteLine("This will fail, Testing one player case");
                            results = game.DecideWinner(game_players);
                            for (int i = 0; i < results.Count; i++)
                            {
                                Console.WriteLine("Winner of the round is {0}", results[i].GetPlayerName());
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }


                        //incorrect hand for user, too few cards
                        List<Card> PlayerCardFail = new List<Card>
                    {
                        new Card('C',12),
                        new Card('H',10),
                        new Card('D',9),
                        new Card('S',8),
                        
                    };
                    
                        //
                        List<Card> DummyPlayerSuit = new List<Card>
                    {
                        new Card('C',12),
                        new Card('H',10),
                        new Card('D',9),
                        new Card('S',8),
                        new Card('H',7),
                    };
                        
                    try
                    {
                        p1 = new Player("Dylan", 4);
                        p1.SetHand(PlayerCardFail.ToArray());
                        p2 = new Player("Taylor", 5);
                        p2.SetHand(DummyPlayerSuit.ToArray());
                        game_players = new Player[2] { p1, p2 };
                        game = new PockerGame();
                        Console.WriteLine("This will fail, testing hand < 4 cards");
                            results = game.DecideWinner(game_players);
                        for (int i = 0; i < results.Count; i++)
                        {
                            Console.WriteLine("Winner of the round is {0}", results[i].GetPlayerName());
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                        //inccorect hand for user, too many cards
                        List<Card> PlayerCardFailLarge = new List<Card>
                    {
                        new Card('C',12),
                        new Card('H',10),
                        new Card('D',9),
                        new Card('S',8),
                        new Card('S',10),
                        new Card('S',2),

                    };
                        
                        //
                        List<Card> DummyPlayerSuit1 = new List<Card>
                    {
                        new Card('C',12),
                        new Card('H',10),
                        new Card('D',9),
                        new Card('S',8),
                        new Card('H',7),
                    };
                        
                    try
                    {
                        p1 = new Player("Dylan", 6);
                        p1.SetHand(PlayerCardFailLarge.ToArray());
                        p2 = new Player("Taylor", 5);
                        p2.SetHand(DummyPlayerSuit1.ToArray());
                        game_players = new Player[2] { p1, p2 };
                        game = new PockerGame();
                        Console.WriteLine("This will fail, testing player hand > 5 cards");
                        results = game.DecideWinner(game_players);
                        for (int i = 0; i < results.Count; i++)
                        {
                            Console.WriteLine("Winner of the round is {0}", results[i].GetPlayerName());
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                        break;
                case "quit":
                case "Quit":
                    main_done = true;
                    break;
                default:
                    break;
                }
            }
        }
    }
}
