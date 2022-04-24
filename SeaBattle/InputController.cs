using System;

namespace SeaBattle
{
    internal class InputController
    {
        public static ConsoleKey InputKey()
        {
            ConsoleKey inputKey;
            do
            {
                inputKey = Console.ReadKey(true).Key;
            } while (inputKey != ConsoleKey.D1);
            return inputKey;
        }
    }
}