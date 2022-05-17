using System;
using System.IO;

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
        private GameType gameType;
        private const int WinsAmountToWin = 3;

        private bool doesBotGoFirst;
        private string WinnerName;

        private PlayerInfo Player1;
        private PlayerInfo Player2;
        private Serialization serialization = new Serialization();

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
            WriteProfilesInfo();
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
            CreateProfiles();
            CreateBotsNames();
        }

        private void CreateProfiles()
        {
            if (gameType == GameType.HumanvsHuman)
            {
                Player1 = CreatePlayer(Player1);
                Player2 = CreatePlayer(Player2);
                return;
            }

            if (gameType == GameType.HumanvsBot)
            {
                PlayerInfo Player = doesBotGoFirst ? Player2 : Player1;
                Player = CreatePlayer(Player);
                //if (doesBotGoFirst)
                //    Player2 = CreatePlayer(Player2);
                //else
                //    Player1 = CreatePlayer(Player1);
            }
        }

        private PlayerInfo CreatePlayer(PlayerInfo Profile)
        {
            string Name = InputController.InputName();
            if (!File.Exists($"{Name}.xml"))
                return CreateProfile(Profile, Name);
            else
                return LoadProfile(Name);
        }

        private PlayerInfo CreateProfile(PlayerInfo profile, string name)
        {
            profile.Name = name;
            profile.WinsAmount = 0;
            serialization.SerializeProfile(profile, FileMode.Create);
            return profile;
        }

        private PlayerInfo LoadProfile(string name) =>
            serialization.GetProfileInfo(name);

        private void WriteProfilesInfo()
        {
            WriteProfileInfo(Player1);
            WriteProfileInfo(Player2);
            Console.ReadKey();
        }

        private void WriteProfileInfo(PlayerInfo profile)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Profile: {profile.Name}, wins amount {profile.WinsAmount}");
            Console.ForegroundColor = ConsoleColor.White;   
        }

        private void CreateBotsNames()
        {
            if(gameType == GameType.BotvsBot)
            {
                CreateBotName(Player1, "1");
                CreateBotName(Player2, "2");
                return;
            }

            if (gameType == GameType.HumanvsBot)
            {
                PlayerInfo Bot = doesBotGoFirst ? Player1 : Player2;
                CreateBotName(Bot, "");
            }
        }

        private void CreateBotName(PlayerInfo playerInfo, string botNameIndex)
        {
            playerInfo.Name = "Bot" + botNameIndex;
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
            AddWinToPlayer();
            WriteScore();
            WriteProfilesInfo();

            IsEndGame = HasSomebodyWin();

            if (IsEndGame)
            {
                DefineWinner();
                SaveProfiles();
            }
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
            Console.ForegroundColor = ConsoleColor.Magenta;
            WritePlayerScore(Player1);
            WritePlayerScore(Player2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        private void AddWinToPlayer()
        {
            PlayerInfo WinPlayer = Round.IsFirstPlayerWin ? Player1 : Player2;
            IncreaseWinsAmount(WinPlayer);
        }

        private void IncreaseWinsAmount(PlayerInfo Player)
        {
            Player.WinsAmount++;
            Player.GameWinsAmount++;
        }

        private void WritePlayerScore(PlayerInfo Player) =>
            Console.WriteLine($"{Player.Name}: {Player.GameWinsAmount}");

        private bool HasSomebodyWin() =>
            Player1.GameWinsAmount == WinsAmountToWin || Player2.GameWinsAmount == WinsAmountToWin;

        private void DefineWinner()
        {
            if (Player1.GameWinsAmount == WinsAmountToWin)
                WinnerName = Player1.Name;
            else
                WinnerName = Player2.Name;
        }

        private void SaveProfiles()
        {
            if (gameType == GameType.BotvsBot) return;

            if (gameType == GameType.HumanvsBot)
            {
                PlayerInfo Player = doesBotGoFirst ? Player2 : Player1;
                serialization.SerializeProfile(Player, FileMode.Open);
            }

            serialization.SerializeProfile(Player1, FileMode.Open);
            serialization.SerializeProfile(Player2, FileMode.Open);
        }

        public void WriteGameResult()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Congratulations {WinnerName}");
            Console.WriteLine();
            WriteProfilesInfo();
        }
    }
}