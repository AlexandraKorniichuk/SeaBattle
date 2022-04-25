using System;

namespace SeaBattle
{
    public class Game
    {
        private char[,] OpenField1;
        private char[,] OpenField2;
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
            OpenField1 = field.CreateOpenField();
            OpenField2 = field.CreateOpenField();
            HiddenField1 = field.CreateEmptyField();
            HiddenField2 = field.CreateEmptyField();
        }

        private void DrawFields()
        {
            field.DrawField(DefineDrawingField(OpenField1, HiddenField2, HiddenField1));
            Console.WriteLine();
            field.DrawField(DefineDrawingField(OpenField2, OpenField1, OpenField2));
        }

        private char[,] DefineDrawingField(char[,] openField, char[,] field1, char[,] field2)
        {
            if (IsFirstFieldOpen())
            {
                return openField;
            }
            else
            {
                if (IsFirstPlayerMove)
                    return field1;
                else
                    return field2;
            }
        }

        private bool IsFirstFieldOpen() =>
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
                return IsShipShotDown(OpenField2);
            else
                return IsShipShotDown(OpenField1);
        }

        private bool IsShipShotDown(char[,] OpenField) =>
            Field.IsCellPositionShip(NewCellPosition, OpenField);

        private void Move(bool WillShipDrown)
        {
            if (WillShipDrown)
            {
                if (IsFirstPlayerMove)
                    BringDownShip(ref OpenField2, ref HiddenField2, ref FirstPlayerHitsAmount);
                else
                    BringDownShip(ref OpenField1, ref HiddenField1, ref SecondPlayerHitsAmount);
            }
            else
            {
                if (IsFirstPlayerMove)
                    GotIntoEmpty(ref OpenField2, ref HiddenField2);
                else
                    GotIntoEmpty(ref OpenField1, ref HiddenField1);
            }
        }

        private void BringDownShip(ref char[,] openField, ref char[,] hiddenField, ref int hitsAmount)
        {
            hitsAmount++;
            openField[NewCellPosition.i, NewCellPosition.j] = CellSymbol.HitInShipSymbol;
            hiddenField[NewCellPosition.i, NewCellPosition.j] = CellSymbol.HitInShipSymbol;
        }

        private void GotIntoEmpty(ref char[,] openField, ref char[,] hiddenField)
        {
            openField[NewCellPosition.i, NewCellPosition.j] = CellSymbol.HitOutEmptySymbol;
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