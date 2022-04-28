using System;

namespace SeaBattle
{
    public class Cursor
    {
        public static void MoveCursorIfCan(ConsoleKey inputKey)
        {
            (int, int) direction = Converting.GetDirection(inputKey.ToString());
            if (direction == (0, 0)) return;

            (int, int) newPosition = Converting.GetNewPosition(GetCutsorPosition(), direction);

            if (CanCursorMove(newPosition))
                Move(newPosition);
        }

        public static (int, int) GetCutsorPosition() =>
            (Console.CursorTop, Console.CursorLeft);

        private static bool CanCursorMove((int i, int j) NewPosition) =>
            Field.IsPositionInsideField(NewPosition);

        private static void Move((int i, int j) NewPosition)
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