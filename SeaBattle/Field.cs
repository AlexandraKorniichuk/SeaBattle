﻿using System;

namespace SeaBattle
{
    public class Field
    {
        public static (int i, int j) FieldSize = (10, 10);
        private const int ShipCount = 20;

        private Random rand = new Random();
        public char[,] CreateOpenField()
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
                ShipsPositions[i] = GetRandomPostion();
                for (int j = i - 1; j >= 0; j--)
                {
                    while (ShipsPositions[i] == ShipsPositions[j])
                        ShipsPositions[i] = GetRandomPostion();
                }
            }
            return ShipsPositions;
        }

        private (int i, int j) GetRandomPostion() =>
            (rand.Next(0, FieldSize.i), rand.Next(0, FieldSize.j));

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
                    if (Field[i, j] == CellSymbol.EmptySymbol)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else if (Field[i, j] == CellSymbol.ShipSymbol)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if (Field[i, j] == CellSymbol.HitInShipSymbol)
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.Write(Field[i, j]);
                    Console.ForegroundColor = ConsoleColor.White;   
                }
                Console.WriteLine();
            }
                
        }

        public static void TryBringDownShip((int, int) NewCellPosition, ref char[,] OpenField, ref char[,] HiddenField)
        {
            if (IsCellPositionShip(NewCellPosition, OpenField))
                TakeAShot(NewCellPosition, ref OpenField, ref HiddenField, CellSymbol.HitInShipSymbol);
            else
                TakeAShot(NewCellPosition, ref OpenField, ref HiddenField, CellSymbol.HitOutEmptySymbol);
        }

        private static void TakeAShot((int i, int j) newCellPosition, ref char[,] openField, ref char[,] hiddenField, char symbol)
        {
            openField[newCellPosition.i, newCellPosition.j] = symbol;
            hiddenField[newCellPosition.i, newCellPosition.j] = symbol;
        }

        private static bool IsCellPositionShip((int i, int j) newCellPosition, char[,] openField) =>
            openField[newCellPosition.i, newCellPosition.j] == CellSymbol.ShipSymbol;
    }
}