
using System.Reflection.Metadata.Ecma335;
using static ship_battle.Data.Ship;

namespace ship_battle.Data
{
    public class PlayerBoard
    {
        public enum SquareType
        {
            Unknown,
            Hit,
            Miss,
            NewHit,
            NewMiss,
        }
        public enum PlayerType
        {
            Friendly,
            Enemy,
        }
        public PlayerType Player { get; }
        public const int BOARD_SIZE = 10;
        public SquareType[,] HitRecord { get; }
        public List<Ship> Ships { get; private set; }
        public PlayerBoard(PlayerType player)
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
        public static string GetPlayerType(PlayerType player)
        {
            if (player == PlayerType.Enemy) { return "Enemy"; }
            return "Friendly";
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
        public bool IsEveryShipSunk()
        {
            foreach (Ship ship in Ships)
            {
                if (!ship.IsSunk()) { return false; }
            }
            return true;
        }
        public bool IsAlreadyAdded(Ship.ShipType shipType)
        {
            foreach (Ship ship in Ships)
            {
                if (ship.Type == shipType) { return true; }
            }
            return false;
        }
        public void Populate(RandStringNum randString)
        {
            int seedPos = 2;
            foreach (Ship.ShipType ship_type in Enum.GetValues(typeof(Ship.ShipType)))
            {
                if (ship_type == Ship.ShipType.None) continue;
                while (true)
                {
                    if (AddShip(new Ship(ship_type, randString.GetIntByPos(seedPos % 20),
                        randString.GetIntByPos((seedPos + 1) % 20), randString.GetIntByPos(seedPos + 2 % 20) > 4))) { break; }
                    seedPos += 2;
                    if (seedPos == randString.RandomStringLength - 2) { seedPos = 1; }
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
        public void ClearBoard()
        {
            Ships.Clear();
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    HitRecord[i, j] = SquareType.Unknown;
                }
            }
        }
        public bool RemoveShip(Ship ship)
        {
            return Ships.Remove(ship);
        }
        public int GetNumSunk()
        {
            int i = 0;
            if (Ships.Count == 0) { return 0; }
            foreach (Ship ship in Ships) { if (ship.IsSunk()) { i++; } }
            return i;
        }
    }
}
