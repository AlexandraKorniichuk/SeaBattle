using System;

namespace SeaBattle
{
    public class InputController
    {
        public static void InputCell()
        {
            ConsoleKey inputKey;
            do
            {
                inputKey = Console.ReadKey(true).Key;
                Cursor.MoveCursorIfCan(inputKey);
            } while (inputKey != ConsoleKey.Enter);
        }

        public static ConsoleKey InputKey()
        {
            ConsoleKey inputKey;
            do
            {
                inputKey = Console.ReadKey(true).Key;
            } while (inputKey != ConsoleKey.D1);
            return inputKey;
        }

        public static ConsoleKey GetInputKey() =>
            Console.ReadKey(true).Key;
    }
}