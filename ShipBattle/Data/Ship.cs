namespace ShipBattle.Data
{
    public class Ship
    {
        public enum ShipType : int
        {
            Carrier = 5,
            Battleship = 4,
            Cruiser = 3,
            Submarine = Cruiser,
            Destroyer = 2,
        }

        private ShipType Type { get; set; }

        private int OriginCol { get; set; }
        private int OriginRow { get; set; }
        private bool Rotate { get; set; }
        
        

        public Ship(ShipType type, int origincol, int originrow, bool rotate)
        {
            Type = type;
            OriginCol = origincol;
            OriginRow = originrow;
            Rotate = rotate;
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
        public string CssShipName()
        {
            if (Type == ShipType.Carrier) { return "Carrier"; }
            if (Type == ShipType.Battleship) { return "Battleship"; }
            if (Type == ShipType.Cruiser) { return "Cruiser"; }
            if (Type == ShipType.Submarine) { return "Submarine"; }
            return "Destroyer";
        }

    }
}
