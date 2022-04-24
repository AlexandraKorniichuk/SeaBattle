﻿using System;

namespace SeaBattle
{
    public class Cursor
    {
        public static void MoveCursorIfCan(ConsoleKey inputKey)
        {
            (int, int) direction = Converting.GetDirection(inputKey.ToString());
            if (direction == (0, 0)) return;

            (int, int) newPosition = Converting.GetNewPosition(GetCurrentPosition(), direction);

            if (CanCursorMove(newPosition))
                Move(newPosition);
        }

        private static (int, int) GetCurrentPosition() =>
            (Console.CursorLeft, Console.CursorTop);

        private static bool CanCursorMove((int i, int j) NewPosition) => 
            NewPosition.i < Field.FieldSize.i && NewPosition.j < Field.FieldSize.j && NewPosition.i >= 0 && NewPosition.j >= 0;

        private static void Move((int i, int j) NewPosition)
        {
            Console.CursorLeft = NewPosition.i;
            Console.CursorTop = NewPosition.j;
        }
    }
}