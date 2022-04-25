using System;

namespace SeaBattle
{
    public class Game
    {
        private char[,] OpenedField1;
        private char[,] OpenedField2;
        private char[,] HiddenField1;
        private char[,] HiddenField2;
        private Field field;
        private (int i, int j) NewCellPosition;

        public static int GameType1 = 1;
        public static int GameType2 = 2;
        public static int GameType3 = 3;
        private int GameType = 1;

        private bool IsFirstPlayerMove;
        public static bool IsFirstPlayerWin;

        private int FirstPlayerHitsAmount = 0;
        private int SecondPlayerHitsAmount = 0;

        private Random random = new Random();
        public void StartNewRound(int gameType)
        {
            GameType = gameType;

            field = new Field();
            CreateFields();

            IsFirstPlayerMove = true;

            GameLoop();
            IsFirstPlayerWin = IsPlayerWin(FirstPlayerHitsAmount);
        }

        private void GameLoop()
        {
            do
            {
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

                IsFirstPlayerMove = !IsFirstPlayerMove;
            } while (!IsEndRound());
        }

        private void CreateFields()
        {
            OpenedField1 = field.CreateOpenedField();
            OpenedField2 = field.CreateOpenedField();
            HiddenField1 = field.CreateEmptyField();
            HiddenField2 = field.CreateEmptyField();
        }

        private void DrawFields()
        {
            field.DrawField(DefineDrawingField(OpenedField1, HiddenField2, HiddenField1));
            Console.WriteLine();
            field.DrawField(DefineDrawingField(OpenedField2, OpenedField1, OpenedField2));
        }

        private char[,] DefineDrawingField(char[,] openedField, char[,] field1, char[,] field2)
        {
            if (IsFieldOpen())
            {
                return openedField;
            }
            else
            {
                if (!IsFirstPlayerMove && IsTheHumanMove())
                    return field2;
                else
                    return field1;
            }
        }

        private bool IsFieldOpen() =>
            GameType == GameType3;

        private bool IsTheHumanMove() =>
            GameType == GameType1 || (GameType == GameType2 && IsFirstPlayerMove);

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
                NewPosition = Converting.GetCursorPosition();
            } while (!IsPlaceFree(NewPosition));
            return NewPosition;
        }

        private bool IsPlaceFree((int i, int j) newPosition) =>
            IsCellEmpty(newPosition, HiddenField2) || (IsCellEmpty(newPosition, HiddenField1) && !IsFirstPlayerMove);

        private bool IsCellEmpty((int i, int j) CellPosition, char[,] Field) =>
            Field[CellPosition.i, CellPosition.j] == CellSymbol.EmptySymbol;

        private (int, int) GetNewRandomPosition()
        {
            (int, int) NewPosition;
            do
            {
                NewPosition = GetRandomPosition();
            } while (!IsPlaceFree(NewPosition));
            return NewPosition;
        }

        private (int, int) GetRandomPosition() =>
            (random.Next(0, Field.FieldSize.i), random.Next(Field.FieldSize.i));

        private bool WillShipDrown()
        {
            if (IsFirstPlayerMove)
                return IsShipShotDown(OpenedField2);
            else
                return IsShipShotDown(OpenedField1);
        }

        private bool IsShipShotDown(char[,] OpenedField) =>
            Field.IsCellPositionShip(NewCellPosition, OpenedField);

        private void Move(bool WillShipDrown)
        {
            if (WillShipDrown)
            {
                if (IsFirstPlayerMove)
                    BringDownShip(ref OpenedField2, ref HiddenField2, ref FirstPlayerHitsAmount);
                else
                    BringDownShip(ref OpenedField1, ref HiddenField1, ref SecondPlayerHitsAmount);
            }
            else
            {
                if (IsFirstPlayerMove)
                    GotIntoEmpty(ref OpenedField2, ref HiddenField2);
                else
                    GotIntoEmpty(ref OpenedField1, ref HiddenField1);
            }
        }

        private void BringDownShip(ref char[,] openedField, ref char[,] hiddenField, ref int hitsAmount)
        {
            hitsAmount++;
            openedField[NewCellPosition.i, NewCellPosition.j] = CellSymbol.HitInShipSymbol;
            hiddenField[NewCellPosition.i, NewCellPosition.j] = CellSymbol.HitInShipSymbol;
        }

        private void GotIntoEmpty(ref char[,] openedField, ref char[,] hiddenField)
        {
            openedField[NewCellPosition.i, NewCellPosition.j] = CellSymbol.HitOutEmptySymbol;
            hiddenField[NewCellPosition.i, NewCellPosition.j] = CellSymbol.HitOutEmptySymbol;
        }

        private bool IsEndRound() =>
            IsPlayerWin(FirstPlayerHitsAmount) || IsPlayerWin(SecondPlayerHitsAmount);

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