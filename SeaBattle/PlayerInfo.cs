using System;
using System.Xml.Serialization;

namespace SeaBattle
{
    [Serializable]
    public class PlayerInfo
    {
        public string Name;
        public double MMR;
        [XmlIgnore] 
        public int GameWinsAmount;
        [XmlIgnore]
        public const double ShipsLeftValueInMMR = 0.2;
        
        public PlayerInfo()
        {
            GameWinsAmount = 0;
        }
    }
}