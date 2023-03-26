using System.Security.Cryptography.X509Certificates;

namespace ShipBattle.Data
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
        private SquareType[,] HitRecord;
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
        public bool AddShip(Ship ship)
        {
            if (Ships.Contains(ship)) 
            { 
               return false;
            }
            Ships.Add(ship);
            return true;

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
                    HitRecord[x,y] = SquareType.Hit;
                    return SquareType.NewHit;
                }
            }
            HitRecord[x, y] = SquareType.Miss;
            return SquareType.NewMiss;
        }
        public Ship getShip(int x, int y)
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
    }
}
