using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;


class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("How Many players in this game?");
        int value;
        
        value = Int32.Parse(Console.ReadLine());
        PockerGame game = new PockerGame(value,5);
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
                    Tuple<bool, string> dealResponse = game.deal();
                    if (!dealResponse.Item1)//if this passes then something went wrong
                    {
                        Console.WriteLine(dealResponse.Item2);
                        while (response != 'q' && response != 'r')
                        {
                            Console.WriteLine("Would you like to restart 'r' or quit 'q'?");
                            response = System.Convert.ToChar(Console.ReadLine());
                        }
                        switch (response)
                        {
                            case 'r':
                                game.ResetDeck();
                                break;
                            case 'q':
                                game.End();
                                done = true;
                                break;
                        }
                    }else//else everything is fine
                    {
                        game.ShowHands();
                        //game.DetermineWinner();
                    }
                    break;
                case 'q':
                    game.End();
                    done = true;
                    break;
                case 'r':
                    game.ResetDeck();
                    break;
                default:
                    Console.WriteLine("Seems like you didn't enter a value that was expected, try again");
                    done = false;
                    break;


            }
            
        }
        

    }
}
