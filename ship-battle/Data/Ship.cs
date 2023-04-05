namespace ship_battle.Data
{
    public class Ship
    {
        public enum ShipType
        {
            None,
            Carrier,
            Battleship,
            Cruiser,
            Submarine,
            Destroyer,
        }
        public ShipType Type { get; private set; }
        public int OriginCol { get; private set; }
        public int OriginRow { get; private set; }
        public int EndCol { get; private set; }
        public int EndRow { get; private set; }

        public int OriginCol1Based
        {
            get { return OriginCol + 1; }
        }

        public int OriginRow1Based
        {
            get { return OriginRow + 1; }
        }
        public int EndCol1Based
        {
            get { return EndCol + 1; }
        }
        public int EndRow1Based
        {
            get { return EndRow + 1; }
        }

        public bool Rotate { get; }

        private int Hits { get; set; }

        public Ship(ShipType type, int origincol, int originrow, bool rotate)
        {
            Type = type;
            OriginCol = origincol;
            OriginRow = originrow;
            Rotate = rotate;
            EndCol = !rotate ? origincol : origincol + Length();
            EndRow = !rotate ? originrow + Length() : originrow;
            Hits = 0;
        }

        public string Name()
        {
            if (Type == ShipType.Carrier) { return "Carrier"; }
            if (Type == ShipType.Battleship) { return "Battleship"; }
            if (Type == ShipType.Cruiser) { return "Cruiser"; }
            if (Type == ShipType.Submarine) { return "Submarine"; }
            return "Destroyer";
        }
        public static string Name(ShipType shipType)
        {
            if (shipType == ShipType.Carrier) { return "Carrier"; }
            if (shipType == ShipType.Battleship) { return "Battleship"; }
            if (shipType == ShipType.Cruiser) { return "Cruiser"; }
            if (shipType == ShipType.Submarine) { return "Submarine"; }
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
        public static int Length(ShipType shipType)
        {
            if (shipType == ShipType.Carrier) { return 5; }
            if (shipType == ShipType.Battleship) { return 4; }
            if (shipType == ShipType.Cruiser) { return 3; }
            if (shipType == ShipType.Submarine) { return 3; }
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
            if (col > 9 || row > 9)
            {
                return false;
            }

            if (Rotate)
            {
                if (row != OriginRow) { return false; }
                if (col >= OriginCol && col < OriginCol + Length()) { return true; }
                return false;
            }
            if (col != OriginCol) { return false; }
            if (row >= OriginRow && row < OriginRow + Length()) { return true; }
            return false;
        }
        public bool IsSunk()
        {
            return (Hits == Length());
        }

        public bool DoesCollide(Ship ship)
        {
            for (int i = 0; i < Length(); i++)
            {
                if (Rotate)
                {
                    if (ship.IsThere(OriginCol + i, OriginRow))
                    {
                        return true;
                    }
                    continue;
                }
                if (ship.IsThere(OriginCol, OriginRow + i))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
