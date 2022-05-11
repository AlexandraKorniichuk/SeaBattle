namespace SeaBattle
{
    public class GamePlayer
    {
        public char[,] OpenedField;
        public char[,] HiddenField;
        public int HitsAmount;
        public string Name;

        public GamePlayer(string name)
        {
            Field field = new Field();
            OpenedField = field.CreateOpenedField();
            HiddenField = field.CreateEmptyField();
            HitsAmount = 0;
            Name = name;
        }

        public GamePlayer TakeAShot(char symbol, (int i, int j) NewPosition)
        {
            OpenedField[NewPosition.i, NewPosition.j] = symbol;
            HiddenField[NewPosition.i, NewPosition.j] = symbol;
            return this;
        }
    }
}