using System;

namespace SeaBattle
{
    public class Converting
    {
        public static GameType GetInputKey(ConsoleKey inputKey)
        {
            if (inputKey == ConsoleKey.D1) return GameType.HumanvsHuman;
            if (inputKey == ConsoleKey.D2) return GameType.HumanvsBot;
            if (inputKey == ConsoleKey.D3) return GameType.BotvsBot;
            return GameType.HumanvsHuman;
        }

        public static (int, int) GetDirection(string directionString)
        {
            int i = 0, j = 0;
            if (directionString == "UpArrow")
                j = -1;
            else if (directionString == "DownArrow")
                j = 1;
            else if (directionString == "RightArrow")
                i = 1;
            else if (directionString == "LeftArrow")
                i = -1;
            return (i, j);
        }

        public static (int, int) GetNewPosition((int i, int j) OldPosition, (int i, int j) direction) =>
            (OldPosition.i + direction.j, OldPosition.j + direction.i);
    }
}