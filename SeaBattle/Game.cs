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

        private static bool IsFirstPlayerMove;
        public static bool IsFirstPlayerWin;

        private Random random = new Random();
        public void StartNewRound(int gameType)
        {
            GameType = gameType;

            field = new Field();
            CreateFields();

            IsFirstPlayerMove = true;

            GameLoop();
        }

        private void GameLoop()
        {
            do
            {
                DrawFields();

                (int, int) SelectedCell = GetSelectedCell();
                Move(SelectedCell);

                Console.Clear();

                DrawFields();
                Console.ReadKey();
                Console.Clear();

                IsFirstPlayerMove = !IsFirstPlayerMove;
            } while (true);
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
                Field.TryBringDownShip(NewCellPosition, ref OpenField2, ref HiddenField2);
            else
                Field.TryBringDownShip(NewCellPosition, ref OpenField1, ref HiddenField1);
        }
    }
}