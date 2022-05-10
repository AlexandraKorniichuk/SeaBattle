using System;

namespace SeaBattle
{
    public class Game
    {
        private GamePlayer Player1;
        private GamePlayer Player2;
        private GamePlayer CurrentPlayer;
        private GamePlayer NotCurrentPlayer;

        private Field field;
        private (int i, int j) NewCellPosition;

        private GameType GameType;

        private bool IsFirstPlayerMove;
        private bool DoesBotGoFirst;
        public static bool IsFirstPlayerWin;

        bool willShipDrown = false;

        public void StartNewRound(GameType gameType, bool doesBotGoFirst)
        {
            GameType = gameType;

            Player1 = new GamePlayer();
            Player2 = new GamePlayer();

            field = new Field();

            DoesBotGoFirst = doesBotGoFirst;
            IsFirstPlayerMove = true;

            GameLoop();
            IsFirstPlayerWin = IsPlayerWin(Player1.HitsAmount);
        }

        private void GameLoop()
        {
            do
            {
                SetMovePlayers();
                Draw();
                Input();
                Move();
                ShowNewMove();
                EndMove();
            } while (!IsEndRound());
        }

        private void SetMovePlayers() =>
            (CurrentPlayer, NotCurrentPlayer) = DefinePlayers(Player1, Player2);

        private void SetGamePlayers() =>
            (Player1, Player2) = DefinePlayers(CurrentPlayer, NotCurrentPlayer);

        private (GamePlayer, GamePlayer) DefinePlayers(GamePlayer Player1, GamePlayer Player2) =>
            (IsFirstPlayerMove ? (Player1, Player2) : (Player2, Player1));

        private void EndMove()
        {
            if (!willShipDrown)
            {
                SetGamePlayers();
                IsFirstPlayerMove = !IsFirstPlayerMove;
                WriteEndMoveMessage();
            }
            Console.ReadKey();
            Console.Clear();
        }

        private void WriteEndMoveMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Move is done");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void ShowNewMove()
        {
            Console.Clear();
            Draw();
        }

        private void Draw()
        {
            Console.Clear();
            DrawFields();
            WriteWhosMove();
        }

        private void DrawFields()
        {
            char[,] FirstFieldToDraw = DefineDrawingField(NotCurrentPlayer.HiddenField, Player1.HiddenField, Player2.HiddenField, Player1.OpenedField);
            char[,] SecondFieldToDraw = DefineDrawingField(CurrentPlayer.OpenedField, Player2.OpenedField, Player1.OpenedField, Player2.OpenedField);
            
            field.DrawField(FirstFieldToDraw);
            Console.WriteLine();
            field.DrawField(SecondFieldToDraw);
        }

        private char[,] DefineDrawingField(char[,] humanField, char[,] botOrHumanField1, char[,] botOrHumanField2, char[,] botField)
        {
            if (GameType == GameType.HumanvsHuman)
                return humanField;
            else if (GameType == GameType.HumanvsBot && DoesBotGoFirst)
                return botOrHumanField1;
            else if (GameType == GameType.HumanvsBot && !DoesBotGoFirst)
                return botOrHumanField2;
            else
                return botField;
        }

        private bool IsTheHumanMove() =>
            GameType == GameType.HumanvsHuman || (GameType == GameType.HumanvsBot && (IsFirstPlayerMove != DoesBotGoFirst));

        private void Input() =>
            NewCellPosition = GetSelectedCell();

        private (int, int) GetSelectedCell()
        {
            if (IsTheHumanMove())
                return InputCell();
            else
                return GetNewRandomPosition();
        }

        private (int, int) InputCell()
        {
            (int, int) NewPosition;
            Cursor.SetCursorPosition();
            do
            {
                InputController.MoveCursor();
                NewPosition = Cursor.GetCursorPosition();
            } while (!IsPlaceFree(NewPosition));
            return NewPosition;
        }

        private bool IsPlaceFree((int i, int j) newPosition) =>
            field.IsCellEmpty(newPosition, NotCurrentPlayer.HiddenField);

        private (int, int) GetNewRandomPosition()
        {
            (int, int) NewPosition;
            do
            {
                NewPosition = Converting.GetRandomPosition();
            } while (!IsPlaceFree(NewPosition));
            return NewPosition;
        }

        private bool WillShipDrown() =>
            field.IsCellPositionShip(NewCellPosition, NotCurrentPlayer.OpenedField);

        private void Move()
        {
            willShipDrown = WillShipDrown();
            if (willShipDrown)
                CurrentPlayer.HitsAmount++;

            char symbol = willShipDrown ? CellSymbol.HitInShipSymbol : CellSymbol.HitOutEmptySymbol;
            NotCurrentPlayer = NotCurrentPlayer.TakeAShot(symbol, NewCellPosition);
        }

        private bool IsEndRound() =>
            IsPlayerWin(CurrentPlayer.HitsAmount);

        private bool IsPlayerWin(int PlayerHitsAmount) =>
            PlayerHitsAmount == Field.ShipCount;

        private void WriteWhosMove()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            if (IsFirstPlayerMove)
                Console.WriteLine("First Player");
            else
                Console.WriteLine("Second Player");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}