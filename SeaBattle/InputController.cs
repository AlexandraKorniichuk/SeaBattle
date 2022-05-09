using System;

namespace SeaBattle
{
    public class InputController
    {
        public static void MoveCursor()
        {
            ConsoleKey inputKey;
            Cursor cursor = new Cursor();
            do
            {
                inputKey = Console.ReadKey(true).Key;
                cursor.MoveCursorIfCan(inputKey);
            } while (inputKey != ConsoleKey.Enter);
        }

        public static ConsoleKey InputGameTypeKey()
        {
            ConsoleKey inputKey;
            do
            {
                inputKey = Console.ReadKey(true).Key;
            } while (inputKey != ConsoleKey.D1 && inputKey != ConsoleKey.D2 && inputKey != ConsoleKey.D3);
            return inputKey;
        }

        public static ConsoleKey InputBotMoveKey()
        {
            ConsoleKey inputKey;
            do
            {
                inputKey = Console.ReadKey(true).Key;
            } while (inputKey != ConsoleKey.D1 && inputKey != ConsoleKey.D2);
            return inputKey;
        }

        public static ConsoleKey GetInputKey() =>
            Console.ReadKey(true).Key;
    }
}