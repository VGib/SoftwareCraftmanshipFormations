using System;

namespace _36
{
    class Program
    {
        private const string NextGameState = "#######################################";

        static void Main(string[] args)
        {
            Console.WriteLine("WellCome in 36 app");
            var game = new _36();

            Console.WriteLine(NextGameState);
            Console.WriteLine("Enter players");
            DoUntilNextGameState(game, g =>
            {
                Console.WriteLine($"{game.Players.Count} players, type a name, type b name to begin game");
                string name = Console.ReadLine();

                if (string.IsNullOrEmpty(name))
                {
                    game.StartGame();
                }
                else
                {
                    game.AddPlayer(name);
                }
            });

            Console.WriteLine(NextGameState);
            Console.WriteLine("Playing");

            DoUntilNextGameState(game, g =>
            {
                Console.WriteLine($"Player who lost {string.Join(" , ",game.LostPlayers)}");
                Console.WriteLine($"Cumulative Score {game.CumulativeScore}");
                Console.WriteLine($"It's {game.PlayerTurn} turn, please enter dice value");

                var diceValue = Console.ReadLine();
                if (!int.TryParse(diceValue, out var diceValueAsInt))
                {
                    throw new Exception("incorrect dice value");
                }


                game.Roll(diceValueAsInt);
            });

            Console.WriteLine(NextGameState);
            Console.WriteLine("Game has end");

            if(game.HasWinner)
            {
                Console.WriteLine($"winner is {game.Winner}");
            }
            else
            {
                Console.WriteLine("No Winner");
            }
        }

        private static void DoUntilNextGameState(_36 game, Action<_36> action)
        {
            var state = game.GameState;

            do
            {
                try
                {
                    action(game);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Something go wrong, {exception.Message}, please redo");
                }
            }
            while (game.GameState == state);
        }
    }
}
