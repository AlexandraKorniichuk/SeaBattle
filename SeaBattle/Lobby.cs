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
        public bool IsEndGame = true;
        private ConsoleKey PlayAgainKey = ConsoleKey.Spacebar;
        public void OpenLobby()
        {
            ShowGreating();
            GameType gameType = GetInputGameTypeKey();
            bool doesBotGoFirst = DoesBotGoFirst(gameType);
            SetConsoleSettings();

            Game game = new Game();
            game.StartNewRound(gameType, doesBotGoFirst);
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
            Console.WriteLine("Choose one type of game:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. Human vs human");
            Console.WriteLine("2. Human vs  Bot");
            Console.WriteLine("3. Bot vs Bot");
        }

        private GameType GetInputGameTypeKey() =>
            Converting.GetGameType(InputController.InputGameTypeKey());

        private bool DoesBotGoFirst(GameType gameType)
        {
            if (gameType != GameType.HumanvsBot)
                return true;
            ShowOptions();
            return Converting.GetBotMoveBool(InputController.InputBotMoveKey());
        }

        private void ShowOptions()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Define who move first:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. Human");
            Console.WriteLine("2. Bot");
        }

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
            (string WinPlayer, string LosePlayer) PlayersNames = ("First Player", "Second Player");
            PlayersNames = PlaceNamesRight(PlayersNames);

            Console.WriteLine($"All ships of {PlayersNames.LosePlayer} have reached the bottom. {PlayersNames.WinPlayer}, congratulations");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private (string, string) PlaceNamesRight((string Player1, string Player2) playersNames) =>
            Game.IsFirstPlayerWin ? (playersNames.Player1, playersNames.Player2) : (playersNames.Player2, playersNames.Player1);

        private void WriteOfferMessage() =>
            Console.WriteLine($"If you want to play again - press '{PlayAgainKey}'");

        private bool HavePlayAgainKeyInput() =>
            InputController.GetInputKey() == PlayAgainKey;
    }
}