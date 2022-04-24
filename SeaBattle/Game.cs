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


                Console.Clear();

                if (IsTheRighMove())
                {
                    DrawFields();
                    Console.ReadKey();
                    Console.Clear();
                }

                IsFirstPlayerMove = !IsFirstPlayerMove;
            } while (true);
        }

        private void CreateFields()
        {
            OpenField1 = field.CreateOpenField();
            if (GameType != GameType2) 
                OpenField2 = field.CreateOpenField();
            if (GameType == GameType1)
                HiddenField1 = field.CreateEmptyField();
            if (GameType != GameType3)
                HiddenField2 = field.CreateEmptyField();
        }

        private void DrawFields()
        {
            if (IsSecondFieldOpen())
                field.DrawField(OpenField1);
            else
                field.DrawField(HiddenField1);

            Console.WriteLine();

            if (IsFirstPlayerMove)
                field.DrawField(OpenField2);
            else
                field.DrawField(HiddenField2);
        }

        private bool IsSecondFieldOpen() =>
            !IsFirstPlayerMove && GameType == GameType1;

        private bool IsTheRighMove() =>
            GameType == GameType1 || (GameType == GameType2 && IsFirstPlayerMove);

        private (int, int) GetSelectedCell()
        {
            (int, int) SelectedCell = (0, 0);
            if (IsTheRighMove())
                SelectedCell = InputCell();
            else
                SelectedCell = GetRandomPosition();
                
            return SelectedCell;
        }

        private (int, int) InputCell()
        {
            (int, int) CurrentPosition = Converting.GetCursorPosition();
            (int, int) NewPosition;
            do
            {
                InputCell();
                NewPosition = Converting.GetCursorPosition();
            } while (CurrentPosition == NewPosition && !IsPlaceFree(NewPosition));
            return NewPosition;
        }

        private bool IsPlaceFree((int i, int j) newPosition) =>
            HiddenField1[newPosition.i, newPosition.j] == CellSymbol.EmptySymbol || 
            (HiddenField2[newPosition.i, newPosition.j] == CellSymbol.EmptySymbol && !IsFirstPlayerMove);

        private (int, int) GetRandomPosition()
        {
            return (0, 0);
        }
    }
}