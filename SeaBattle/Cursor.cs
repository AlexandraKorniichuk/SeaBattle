using System;

namespace SeaBattle
{
    public class Cursor
    {
        public void MoveCursorIfCan(ConsoleKey inputKey)
        {
            (int, int) direction = Converting.GetDirection(inputKey.ToString());
            if (direction == (0, 0)) return;

            (int, int) newPosition = Converting.GetNewPosition(GetCursorPosition(), direction);

            if (CanCursorMove(newPosition))
                Move(newPosition);
        }

        public static (int, int) GetCursorPosition() =>
            (Console.CursorTop, Console.CursorLeft);

        private bool CanCursorMove((int i, int j) NewPosition) =>
            Field.IsPositionInsideField(NewPosition);

        private void Move((int i, int j) NewPosition)
        {
            Console.CursorTop = NewPosition.i;
            Console.CursorLeft = NewPosition.j;
        }

        public static void SetCursorPosition()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
        }
    }
}