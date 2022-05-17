using System;

namespace SeaBattle
{
    [Serializable]
    public class PlayerInfo
    {
        public string Name;
        public int WinsAmount;
        [NonSerialized] public int GameWinsAmount;

        public PlayerInfo()
        {
            GameWinsAmount = 0;
        }
    }
}