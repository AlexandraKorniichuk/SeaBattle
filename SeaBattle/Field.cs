using System;

namespace SeaBattle
{
    public class Field
    {
        public static (int i, int j) FieldSize = (1, 1);
        public static int ShipCount;
        private double ShipsPersantage = 0.2;

        public Field()
        {
            ShipCount = (int)(FieldSize.i * FieldSize.j * ShipsPersantage);
            if (ShipCount == 0) ShipCount = 1;
        }

        public char[,] CreateOpenedField()
        {
            (int i, int j)[] ShipsPositions = GetShipPositions();

            char[,] Field = CreateEmptyField();
            for(int i = 0; i < FieldSize.i; i++)
            {
                for (int j = 0; j < FieldSize.j; j++)
                {
                    if (IsCellShip(ShipsPositions, (i, j)))
                        Field[i, j] = CellSymbol.ShipSymbol;
                }
            }
            return Field;
        }

        public char[,] CreateEmptyField()
        {
            char[,] Field = new char[FieldSize.i, FieldSize.j];
            for (int i = 0; i < FieldSize.i; i++)
            {
                for (int j = 0; j < FieldSize.j; j++)
                {
                    Field[i, j] = CellSymbol.EmptySymbol;
                }
            }
            return Field;
        }

        private (int i, int j)[] GetShipPositions()
        {
            (int i, int j)[] ShipsPositions = new (int, int)[ShipCount];

            for(int i = 0; i < ShipsPositions.Length; i++)
            {
                ShipsPositions[i] = Converting.GetRandomPosition();
                for (int j = i - 1; j >= 0; j--)
                {
                    while (ShipsPositions[i] == ShipsPositions[j])
                        ShipsPositions[i] = Converting.GetRandomPosition();
                }
            }
            return ShipsPositions;
        }

        private bool IsCellShip((int i, int j)[] ShipsPositions, (int i, int j) CurrentCell)
        {
            for (int i = 0; i < ShipCount; i++)
            {
                if (ShipsPositions[i] == CurrentCell)
                    return true;
            }
            return false;
        }

        public void DrawField(char[,] Field)
        {
            for (int i = 0; i < FieldSize.i; i++)
            {
                for (int j = 0; j < FieldSize.j; j++)
                {
                    Console.ForegroundColor = Converting.GetCellColor(Field[i, j]);

                    Console.Write(Field[i, j]);
                    Console.ForegroundColor = ConsoleColor.White;   
                }
                Console.WriteLine();
            }
        }

        public static bool IsPositionInsideField((int i, int j) NewPosition) =>
            NewPosition.i < FieldSize.i && NewPosition.j < FieldSize.j && NewPosition.i >= 0 && NewPosition.j >= 0;

        public bool IsCellEmpty((int i, int j) CellPosition, char[,] Field) =>
            Field[CellPosition.i, CellPosition.j] == CellSymbol.EmptySymbol;

        public bool IsCellPositionShip((int i, int j) newCellPosition, char[,] openField) =>
            openField[newCellPosition.i, newCellPosition.j] == CellSymbol.ShipSymbol;
    }
}