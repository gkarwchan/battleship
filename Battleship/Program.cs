using System;
using Battleship.Model;

namespace Battleship
{
    class Program
    {

        private static Game _game;

        static void Main(string[] args)
        {
            Console.WriteLine("Game Starting");
           
           _game = GameFactory.CreateGame(8, 8, "player 1", "player 2");
            
            
            handlePlayerShipInput();
            PrintBoard(true);

            handlePlayerShipInput();
            PrintBoard(false);

            while (!_game.IsGameOver)
            {
                

                handlePlayerHitInput();
                PrintBoard(true);
                PrintBoard(false);
            }
            
            Console.WriteLine("Game over");
            Console.WriteLine("Congratulation {0} , you are the winner", _game.CurrentPlayerName);
        }

        



        static void handlePlayerHitInput()
        {
            Console.WriteLine("{0}: Provide a location to hit {1} ship. Format: B5", _game.CurrentPlayerName, _game.OppenentPlayerName);
            var inputProcessDone = false;
            do
            {
                var input = Console.ReadLine();
                try
                {
                    _game.PlayerPlayTurn(input);
                    inputProcessDone = true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("{0}: Error in input: coordination out of the board. Please try again", _game.CurrentPlayerName);
                }

                catch (NotValidShapeForShipException)
                {
                    Console.WriteLine("{0}: Ship should be one dimension and 3 unit size. Please try again", _game.CurrentPlayerName);
                }

            } while (!inputProcessDone);

        }
        

        static void handlePlayerShipInput()
        {
            Console.WriteLine("Please Enter the ship location for {0}. Format: A3 A5", _game.CurrentPlayerName);
            var inputProcessDone = false;

            do
            {
                var input = Console.ReadLine();
                try
                {
                    _game.PlaceShipInCurrentBoard(input);
                    inputProcessDone = true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Error in input: coordination out of the board. Please try again");
                }

                catch (NotValidShapeForShipException)
                {
                    Console.WriteLine("Ship should be one dimension and 3 unit size. Please try again");
                }
                
            } while (!inputProcessDone);
            
        }


       

        
        static void PrintBoard(bool isFirstPlayer)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Player {0} Board", isFirstPlayer ? "1" : "2");
            DumpBoard(_game.GetPlayerBoard(isFirstPlayer));
            Console.WriteLine();
            Console.WriteLine();

        }

        static void DumpBoard(BoardCellStatus[,] cells)
        {
            string[] statusDisplay = {"-", "S", "*", "X"};
            ConsoleColor[] statusColor = {ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Yellow};
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.Write("  ");
            for (int col = 65; col < (65 + 8); col++)
            {
                Console.Write("{0} ", (char)col);
            }
            Console.WriteLine();
            for (int row = 0; row < 8; row++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("{0} ", row + 1);
                for (int col = 0; col < 8; col++)
                {
                    Console.ForegroundColor = statusColor[(int) cells[row, col]];
                    Console.Write("{0} ", statusDisplay[(int) cells[row, col]]);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

        }
    }
}
