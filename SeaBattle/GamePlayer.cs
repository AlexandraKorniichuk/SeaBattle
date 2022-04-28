namespace SeaBattle
{
    public class GamePlayer
    {
        public char[,] OpenedField;
        public char[,] HiddenField;
        public int HitsAmount;
        public GamePlayer()
        {
            Field field = new Field();
            OpenedField = field.CreateOpenedField();
            HiddenField = field.CreateEmptyField();
            HitsAmount = 0;
        }
    }
}