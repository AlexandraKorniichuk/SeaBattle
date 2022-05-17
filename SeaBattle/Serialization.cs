using System.IO;
using System.Xml.Serialization;

namespace SeaBattle
{
    public class Serialization
    {
        private XmlSerializer Serializer = new XmlSerializer(typeof(PlayerInfo));

        public void SerializeProfile(PlayerInfo player, FileMode fileMode)
        {
            using (FileStream stream = new FileStream($"{player.Name}.xml", fileMode))
                Serializer.Serialize(stream, player);
        }

        public PlayerInfo GetProfileInfo(string name)
        {
            using (FileStream stream = new FileStream($"{name}.xml", FileMode.Open))
            {
                PlayerInfo profile = (PlayerInfo)Serializer.Deserialize(stream);
                return profile;
            }
        }
    }
}