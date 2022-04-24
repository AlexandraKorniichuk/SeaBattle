using System;

namespace SeaBattle
{
    public class Converting
    {
        public static int GetInputKey(ConsoleKey inputKey)
        {
            if (inputKey == ConsoleKey.D1) return 1;
            if (inputKey == ConsoleKey.D2) return 2;
            if (inputKey == ConsoleKey.D3) return 3;
            return 0;
        }
    }
}