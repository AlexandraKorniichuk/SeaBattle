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
        public static bool IsFirstPlayerWin;

        public void StartNewRound(GameType gameType)
        {
            GameType = gameType;

            Player1 = new GamePlayer();
            Player2 = new GamePlayer();

            field = new Field();

            IsFirstPlayerMove = true;

            GameLoop();
            IsFirstPlayerWin = IsPlayerWin(Player1.HitsAmount);
        }

        private void GameLoop()
        {
            do
            {
                GetPlayers();
                DrawFields();
                WriteWhosMove();

                NewCellPosition = GetSelectedCell();

                bool willShipDrown = WillShipDrown();
                if (willShipDrown)
                {
                    Move(WillShipDrown: willShipDrown);
                    Console.Clear();
                    continue;
                }
                else
                {
                    Move(WillShipDrown: willShipDrown);
                }

                Console.Clear();

                DrawFields();
                WriteWhosMove();

                Console.ReadKey();
                Console.Clear();

                SetPlayers();
                IsFirstPlayerMove = !IsFirstPlayerMove;
            } while (!IsEndRound());
        }

        private void GetPlayers()
        {
            if (IsFirstPlayerMove)
            {
                CurrentPlayer = Player1;
                NotCurrentPlayer = Player2;
            }
            else
            {
                CurrentPlayer = Player2;
                NotCurrentPlayer = Player1;
            }
        }

        private void SetPlayers()
        {
            if (IsFirstPlayerMove)
            {
                Player1 = CurrentPlayer;
                Player2 = NotCurrentPlayer;
            }
            else
            {
                Player2 = CurrentPlayer;
                Player1 = NotCurrentPlayer;
            }
        }

        private void DrawFields()
        {
            field.DrawField(DefineDrawingField(NotCurrentPlayer.HiddenField, Player2.HiddenField, Player1.OpenedField));
            Console.WriteLine();
            field.DrawField(DefineDrawingField(CurrentPlayer.OpenedField, Player1.OpenedField, Player2.OpenedField));
        }

        private char[,] DefineDrawingField(char[,] humanField, char[,] botOrHumanField, char[,] botField)
        {
            if (GameType == GameType.HumanvsHuman)
                return humanField;
            else if (GameType == GameType.HumanvsBot)
                return botOrHumanField;
            else
                return botField;
        }

        private bool IsTheHumanMove() =>
            GameType == GameType.HumanvsHuman || (GameType == GameType.HumanvsBot && IsFirstPlayerMove);

        private (int, int) GetSelectedCell()
        {
            (int, int) SelectedCell;
            if (IsTheHumanMove())
                SelectedCell = InputCell();
            else
                SelectedCell = GetNewRandomPosition();

            return SelectedCell;
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
            IsCellEmpty(newPosition, NotCurrentPlayer.HiddenField);

        private bool IsCellEmpty((int i, int j) CellPosition, char[,] Field) =>
            Field[CellPosition.i, CellPosition.j] == CellSymbol.EmptySymbol;

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
            IsShipShotDown(NotCurrentPlayer.OpenedField);

        private bool IsShipShotDown(char[,] OpenedField) =>
            IsCellPositionShip(NewCellPosition, OpenedField);

        private bool IsCellPositionShip((int i, int j) newCellPosition, char[,] openField) =>
            openField[newCellPosition.i, newCellPosition.j] == CellSymbol.ShipSymbol;

        private void Move(bool WillShipDrown)
        {
            if (WillShipDrown)
            {
                CurrentPlayer.HitsAmount++;
                TakeAshot(ref NotCurrentPlayer, CellSymbol.HitInShipSymbol);
            }
            else
            {
                TakeAshot(ref NotCurrentPlayer, CellSymbol.HitOutEmptySymbol);
            }
        }

        private void TakeAshot(ref GamePlayer Player, char symbol)
        {
            Player.OpenedField[NewCellPosition.i, NewCellPosition.j] = symbol;
            Player.HiddenField[NewCellPosition.i, NewCellPosition.j] = symbol;
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