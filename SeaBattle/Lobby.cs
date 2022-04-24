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
            SetConsoleSettings();
        }

        private void ShowGreating()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Welcome to the Sea battle");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Use arrows to choose cell and enter to select");
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

        private void SetConsoleSettings() =>
            Console.CursorSize = 100;

        public void EndRound()
        {
            WriteResultMessage();
            WriteOfferMessage();
            IsEndGame = !HavePlayAgainKeyInput();
            Console.Clear();
        }

        private void WriteResultMessage()
        {
            
        }

        private void WriteOfferMessage() =>
            Console.WriteLine($"If you want to play again - press '{PlayAgainKey}'");

        private bool HavePlayAgainKeyInput() =>
            InputController.GetInputKey() == PlayAgainKey;
    }
}