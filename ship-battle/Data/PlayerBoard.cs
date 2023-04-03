﻿namespace ship_battle.Data
{
    public class PlayerBoard
    {
        public int Player { get; }
        public enum SquareType
        {
            Unknown,
            Hit,
            Miss,
            NewHit,
            NewMiss,
        }
        const int BOARD_SIZE = 10;
        public SquareType[,] HitRecord { get; }
        public List<Ship> Ships { get; private set; }
        public PlayerBoard(int player)
        {
            Player = player;
            HitRecord = new SquareType[BOARD_SIZE, BOARD_SIZE];
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    HitRecord[i, j] = SquareType.Unknown;
                }
            }
            Ships = new List<Ship>();
        }
        public static string GetSquareTypeName(SquareType square)
        {
            if (square == SquareType.Unknown) { return "Unknown"; }
            if (square == SquareType.Hit) { return "Hit"; }
            if (square == SquareType.Miss) { return "Miss"; }
            if (square == SquareType.NewMiss) { return "NewMiss"; }
            return "NewHit";
        }
        public bool AddShip(Ship ship)
        {
            if (Ships.Contains(ship))
            {
                return false;
            }
            if (!IsValidNewShipLocation(ship))
            {
                return false;
            }
            Ships.Add(ship);
            return true;

        }
        public void Populate(int seed1, int seed2)
        {
            int[] seedArr = RandArr.GetArrSeed(seed1, seed2, 20);
            int seedPos = 2;
            foreach (Ship.ShipType ship_type in Enum.GetValues(typeof(Ship.ShipType)))
            {
                while (true)
                {

                    if (AddShip(new Ship(ship_type, Math.Abs(seedArr[seedPos % 20]),
                        Math.Abs(seedArr[(seedPos + 1) % 20]), Math.Abs(seedArr[(seedPos + 2) % 20]) > 4))) { break; }
                    seedPos += 2;
                    if (seedPos > 20) { seedPos = 1; }

                }


            }
        }
        public SquareType Hit(int x, int y)
        {
            if (HitRecord[x, y] != SquareType.Unknown)
            {
                return HitRecord[x, y];
            }
            foreach (Ship ship in Ships)
            {
                if (ship.IsHit(x, y))
                {
                    HitRecord[x, y] = SquareType.Hit;
                    return SquareType.NewHit;
                }
            }
            HitRecord[x, y] = SquareType.Miss;
            return SquareType.NewMiss;
        }
        public Ship GetShip(int x, int y)
        {
            foreach (Ship ship in Ships)
            {
                if (ship.IsThere(x, y))
                {
                    return ship;
                }
            }
            throw new Exception("No ship found at specified coordinates.");
        }
        private bool IsValidNewShipLocation(Ship new_ship)
        {
            if (!IsInBounds(new_ship)) { return false; }
            foreach (Ship ship in Ships)
            {
                if (ship.DoesCollide(new_ship)) { return false; }
            }
            return true;
        }
        private static bool IsInBounds(Ship ship)
        {
            if (ship.Rotate)
            {
                if (ship.OriginCol + ship.Length() - 1 > 9)
                {
                    return false;
                }
                return true;
            }
            if (ship.OriginRow + ship.Length() - 1 > 9)
            {
                return false;
            }
            return true;
        }
    }
}
