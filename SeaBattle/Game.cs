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

                (int, int) SelectedCell = GetSelectedCell();
                Move(SelectedCell);

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

        private void Move((int i, int j) NewCellPosition)
        {
            if (IsFirstPlayerMove)
                TryBringDownShip(NewCellPosition, ref OpenField2, ref HiddenField2, ref FirstPlayerHitsAmount);
            else
                TryBringDownShip(NewCellPosition, ref OpenField1, ref HiddenField1, ref SecondPlayerHitsAmount);
        }

        public void TryBringDownShip((int, int) NewCellPosition, ref char[,] OpenField, ref char[,] HiddenField, ref int HitsAmount)
        {
            if (Field.IsCellPositionShip(NewCellPosition, OpenField))
            {
                HitsAmount++;
                TakeAShot(NewCellPosition, ref OpenField, ref HiddenField, CellSymbol.HitInShipSymbol);
            }
            else
            {
                TakeAShot(NewCellPosition, ref OpenField, ref HiddenField, CellSymbol.HitOutEmptySymbol);
            }
        }

        private void TakeAShot((int i, int j) newCellPosition, ref char[,] openField, ref char[,] hiddenField, char symbol)
        {
            openField[newCellPosition.i, newCellPosition.j] = symbol;
            hiddenField[newCellPosition.i, newCellPosition.j] = symbol;
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