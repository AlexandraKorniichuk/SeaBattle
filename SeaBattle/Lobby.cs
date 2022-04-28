using System;

namespace SeaBattle
{
    public enum GameType
    {
        HumanvsHuman,
        HumanvsBot,
        BotvsBot
    }

    public class Lobby
    {
        public GameType GameType;
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

        private GameType GetInputGameTypeKey() =>
            Converting.GetInputKey(InputController.InputKey());

        private void SetConsoleSettings() =>
            Console.CursorSize = 100;

        public void EndRound()
        {
            WriteResultMessage();
            //forlater
            //WriteOfferMessage();
            //IsEndGame = !HavePlayAgainKeyInput();
            //Console.Clear();
        }

        private void WriteResultMessage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (Game.IsFirstPlayerWin)
                Console.WriteLine("All ships of second player have reached the bottom. First player, congratulations");
            else
                Console.WriteLine("All ships of first player have reached the bottom. Second player, congratulations");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void WriteOfferMessage() =>
            Console.WriteLine($"If you want to play again - press '{PlayAgainKey}'");

        private bool HavePlayAgainKeyInput() =>
            InputController.GetInputKey() == PlayAgainKey;
    }
}