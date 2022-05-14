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
        private bool IsEndGame = true;
        private PlayerInfo Player1;
        private PlayerInfo Player2;

        private GameType gameType;
        private bool doesBotGoFirst;

        private string WinnerName;
        private const int WinsAmountToWin = 3;

        public void StartLobby()
        {
            OpenLobby();
            do
            {
                StartNewRound();
                EndRound();
            } while (!IsEndGame);
            WriteGameResult();
        }

        private void OpenLobby()
        {
            ShowGreating();
            gameType = GetInputGameTypeKey();
            doesBotGoFirst = DoesBotGoFirst();
            CreatePlayers();
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
            Console.WriteLine("Choose one type of game:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. Human vs human");
            Console.WriteLine("2. Human vs Bot");
            Console.WriteLine("3. Bot vs Bot");
        }

        private GameType GetInputGameTypeKey() =>
            Converting.GetGameType(InputController.InputGameTypeKey());

        private bool DoesBotGoFirst()
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

        private void CreatePlayers()
        {
            Player1 = new PlayerInfo();
            Player2 = new PlayerInfo();
            CreateNames();
        }

        private void CreateNames()
        {
            if(gameType == GameType.BotvsBot)
            {
                SetNames("Bot1", "Bot2");
                return;
            }

            if (gameType == GameType.HumanvsHuman)
                SetNames(InputController.InputName(), InputController.InputName());
            else if (gameType == GameType.HumanvsBot && !doesBotGoFirst)
                SetNames(InputController.InputName(), "Bot");
            else if (gameType == GameType.HumanvsBot && doesBotGoFirst)
                SetNames("Bot", InputController.InputName());
        }

        private void SetNames(string Name1, string Name2)
        {
            Player1.Name = Name1;
            Player2.Name = Name2;
        }

        private void SetConsoleSettings() =>
            Console.CursorSize = 100;

        private void StartNewRound()
        {
            Round round = new Round(Player1.Name, Player2.Name);
            round.StartNewRound(gameType, doesBotGoFirst);
        }


        public void EndRound()
        {
            WriteRoundResult();
            
            WriteScore();
            IsEndGame = HasSomebodyWin();

            if (IsEndGame) DefineWinner();
            Console.ReadKey();
            Console.Clear();
        }

        private void WriteRoundResult()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            (string WinPlayer, string LosePlayer) PlayersNames = PlaceNamesRight();

            Console.WriteLine($"All ships of {PlayersNames.LosePlayer} have reached the bottom. {PlayersNames.WinPlayer}, congratulations");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        private (string, string) PlaceNamesRight() =>
            Round.IsFirstPlayerWin ? (Player1.Name, Player2.Name) : (Player2.Name, Player1.Name);

        private void WriteScore()
        {
            AddWinToPlayer();
            Console.ForegroundColor = ConsoleColor.Magenta;
            WritePlayerScore(Player1);
            WritePlayerScore(Player2);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void AddWinToPlayer()
        {
            if (Round.IsFirstPlayerWin)
                Player1.WinsAmount++;
            else
                Player2.WinsAmount++; 
        }

        private void WritePlayerScore(PlayerInfo Player) =>
            Console.WriteLine($"{Player.Name}: {Player.WinsAmount}");

        private bool HasSomebodyWin() =>
            Player1.WinsAmount == WinsAmountToWin || Player2.WinsAmount == WinsAmountToWin;

        private void DefineWinner()
        {
            if (Player1.WinsAmount == WinsAmountToWin)
                WinnerName = Player1.Name;
            else
                WinnerName = Player2.Name;
        }

        public void WriteGameResult()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Congratulations {WinnerName}");
        }
    }
}