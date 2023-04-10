using ship_battle.Data;

namespace ship_battle.Data.ComputerPlayerData
{
    public class ComputerPlayer
    {
        private TargetComp TargetComp;
        private readonly PlayerBoard Board;
        private int RandStringPointer;
        private readonly RandStringNum RandString = new(200);
        private List<RoundMem> Rounds;
        private bool IsHit;
        public ComputerPlayer(PlayerBoard playerBoard)
        {
            Rounds = new();
            TargetComp = new(Rounds, playerBoard);
            RandStringPointer = 0;
            IsHit = false;
            Board = playerBoard;
            
            Console.WriteLine(RandString.ToString());
        }
        public void PlayTurn()
        {
            // start out hitting random position
            if (Rounds.Count == 0) { PlayTurnRandom(); return; }
            // if last turn was a hit
            if (Rounds[Rounds.Count - 1].IsHit && IsHit == false) { IsHit = true; TargetComp = new(Rounds, Board); }
            if (IsHit)
            {
                if (TargetComp.AttemptHit()) { return; }
                IsHit = false;
            }
            PlayTurnRandom();
        }
        private void PlayTurnRandom()
        {
            bool fallBack = false;
            int x;
            int y;
            while (true)
            {
                x = RandString.GetIntByPos(RandStringPointer);
                y = RandString.GetIntByPos(RandStringPointer + 1);
                if (RandStringPointer == RandString.RandomStringLength - 2) { RandStringPointer = 1; continue; }
                if (RandStringPointer == RandString.RandomStringLength - 3) { fallBack = true; }
                if (fallBack)
                {
                    PlayTurnNext();
                    return;
                }
                if (IsSquareAlreadyHit(x, y)) { RandStringPointer += 2; continue; }
                Rounds.Add(new RoundMem(x, y, Hit(x, y)));
                break;
            }
            return;
        }
        private void PlayTurnNext()
        {
            for (int x = 0; x < PlayerBoard.BOARD_SIZE; x++)
            {
                for (int y = 0; y < PlayerBoard.BOARD_SIZE; y++)
                {
                    if (!IsSquareAlreadyHit(x, y))
                    {
                        Rounds.Add(new RoundMem(x, y, Hit(x, y)));
                        return;
                    }
                }
            }
        }
        private bool IsSquareAlreadyHit(int x, int y)
        {
            foreach (RoundMem round in Rounds)
            {
                if ((round.CoorX == x) && (round.CoorY == y)) { return true; }
            }
            return false;
        }
        private bool Hit(int x, int y) 
        {
            PlayerBoard.SquareType hitType = Board.Hit(x, y);
            if (hitType == PlayerBoard.SquareType.NewHit) { return true; }
            if (hitType == PlayerBoard.SquareType.NewMiss) { return false; }
            throw new Exception("Computer hit the same square twice!");
        }
    }
}
