namespace ShipBattle.Data
{
    public class Ship
    {
        public enum ShipType
        {
            Carrier,
            Battleship,
            Cruiser,
            Submarine,
            Destroyer,
        }
        private ShipType Type { get; set; }

        private int OriginCol { get; set; }
        private int OriginRow { get; }
        private bool Rotate { get; set; }

        private int Hits { get; set; }

        public Ship(ShipType type, int origincol, int originrow, bool rotate)
        {
            Type = type;
            OriginCol = origincol;
            OriginRow = originrow;
            Rotate = rotate;
            Hits = 0;
        }

        public string CssOriginCol()
        {
            return (OriginCol * 10).ToString() + "%";
        }
        public string CssOriginRow()
        {
            return (OriginRow * 10).ToString() + "%";
        }
        public string CssRotate()
        {
            if (Rotate)
            {
                return "-90deg";
            }
            return "0deg";
        }
        public string Name()
        {
            if (Type == ShipType.Carrier) { return "Carrier"; }
            if (Type == ShipType.Battleship) { return "Battleship"; }
            if (Type == ShipType.Cruiser) { return "Cruiser"; }
            if (Type == ShipType.Submarine) { return "Submarine"; }
            return "Destroyer";
        }
        public int Length()
        {
            if (Type == ShipType.Carrier) { return 5; }
            if (Type == ShipType.Battleship) { return 4; }
            if (Type == ShipType.Cruiser) { return 3; }
            if (Type == ShipType.Submarine) { return 3; }
            return 2;
        }
        public bool IsHit(int col, int row)
        {
            if (IsThere(col, row))
            {
                Hits++;
                return true;
            }
            return false;
        }
        public bool IsThere(int col, int row)
        {
            if (Rotate)
            {
                if (row != OriginRow) { return false; }
                if (col >= OriginCol && col <= OriginCol + Length()) { return true; }
                return false;
            }
            if (col != OriginCol) { return false; }
            if (row >= OriginRow && row <= OriginRow + Length()) { return true; }
            return false;
        }
        public bool IsSunk()
        {
            return (Hits == Length());
        }
    }
}
