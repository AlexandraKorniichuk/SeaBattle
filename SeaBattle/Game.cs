using System;

namespace SeaBattle
{
    public class Game
    {
        private char[,] OpenField1;
        private char[,] OpenField2;
        private char[,] HiddenField1;
        private char[,] HiddenField2;

        public static bool IsFirstPlayerWin;
        public void StartNewRound(int GameType)
        {
            Field field = new Field();
            CreateFields(field);
            field.DrawField(OpenField1);
        }

        private void CreateFields(Field field)
        {
            OpenField1 = field.CreateOpenField();
            OpenField2 = field.CreateOpenField();
        }
    }
}