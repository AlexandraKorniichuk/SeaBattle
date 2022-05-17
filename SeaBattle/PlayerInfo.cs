using System;
using System.Xml.Serialization;

namespace SeaBattle
{
    [Serializable]
    public class PlayerInfo
    {
        public string Name;
        public int WinsAmount;
        [XmlIgnore] 
        public int GameWinsAmount;

        public PlayerInfo()
        {
            GameWinsAmount = 0;
        }
    }
}