using System;

namespace SeaBattle
{
    public class Lobby
    {
        public int GameType = 1;
        public bool IsEndGame = true;
        private ConsoleKey PlayAgainKey = ConsoleKey.Spacebar;
        public void OpenLobby()
        {
            ShowGreating();
            GameType = GetInputGameTypeKey();
            Console.Clear();
        }

        private void ShowGreating()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Welcome to the Sea battle");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Use arrows to choose cell");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Choose one type of game:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. Human vs human");
            Console.WriteLine("2. Soon");
            Console.WriteLine("3. Soon");
        }

        private int GetInputGameTypeKey() =>
            Converting.GetInputKey(InputController.InputKey());

        //public void EndRound()
        //{
        //    WriteResultMessage();
        //    WriteOfferMessage();
        //    IsEndGame = !HavePlayAgainKeyInput();
        //    Console.Clear();
        //}

        //private void WriteResultMessage()
        //{
        //    if (Game.IsWin)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Yellow;
        //        Console.WriteLine("Congratulations, you made it out");
        //    }
        //    else
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine("Your time is up");
        //    }
        //    Console.ForegroundColor = ConsoleColor.White;
        //}

        //private void WriteOfferMessage() =>
        //    Console.WriteLine($"If you want to play again - press '{PlayAgainKey}'");

        //private bool HavePlayAgainKeyInput() =>
        //    InputController.GetInputKey() == PlayAgainKey;
    }
}