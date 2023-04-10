namespace ship_battle.Data.ComputerPlayerData
{
    public class RoundMem
    {
        public int CoorX {  get; private set; }
        public int CoorY { get; private set; }
        public bool IsHit;
        public RoundMem(int coorX, int coorY, bool isHit) 
        { 
            CoorX = coorX;
            CoorY = coorY;
            IsHit = isHit;
        }
        public override string ToString()
        {
            return "X: " + CoorX.ToString() + " Y: " + CoorY.ToString()+ " is hit: " + IsHit.ToString();
        }
    }
}
