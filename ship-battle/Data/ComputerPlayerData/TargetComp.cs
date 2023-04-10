using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ship_battle.Data.ComputerPlayerData
{
    public class TargetComp
    {
        private int LastSunkNum;
        private bool IsTargetHorizontal;
        private int RegisteredHitPointerInit;
        private int RegisteredHitPointerProg;
        private PlayerBoard TargetBoard;
        private List<RoundMem> Rounds;

        private TargetStatus targetStatus;
        public enum TargetStatus
        {
            FindDirection,
            AttemptForward,
            AttemptBackward,
        }
        public TargetComp(List<RoundMem> rounds, PlayerBoard playerBoard)
        {
            targetStatus = TargetStatus.FindDirection;
            TargetBoard = playerBoard;
            Rounds = rounds;
            RegisteredHitPointerInit = rounds.Count - 1;
            RegisteredHitPointerProg = rounds.Count - 1;
            LastSunkNum = playerBoard.GetNumSunk();
        }
        public bool AttemptHit()
        {
            if (LastSunkNum < TargetBoard.GetNumSunk()) { return false; }
            if (Rounds.Count > 1) { Console.WriteLine(Rounds[Rounds.Count() - 1].ToString()); }

            if (targetStatus == TargetStatus.FindDirection) 
            {
                int initX = Rounds[RegisteredHitPointerInit].CoorX;
                int initY = Rounds[RegisteredHitPointerInit].CoorY;
                (int newX, int newY) = FindTarget(Rounds[RegisteredHitPointerInit].CoorX, Rounds[RegisteredHitPointerInit].CoorY);
                if (newX == initX && newY == initY) return false;
                HitAdd(newX, newY);
                if (Rounds[Rounds.Count - 1].IsHit == true)
                {
                    if (newX == initX) IsTargetHorizontal = false;
                    if (newY == initY) IsTargetHorizontal = true;
                    RegisteredHitPointerProg = Rounds.Count - 1;
                    if (IsTargetHorizontal && Rounds[RegisteredHitPointerInit].CoorX > Rounds[RegisteredHitPointerProg].CoorX) { swapPointers(); }
                    if (!IsTargetHorizontal && Rounds[RegisteredHitPointerInit].CoorY > Rounds[RegisteredHitPointerProg].CoorY) { swapPointers(); }
                    targetStatus = TargetStatus.AttemptForward;
                }
                return true;
            }
            int TTL = 0;
            while (true)
            {
                if (TTL > 4) { return false; }
                if (targetStatus == TargetStatus.AttemptForward)
                {
                    int progX = Rounds[RegisteredHitPointerProg].CoorX;
                    int progY = Rounds[RegisteredHitPointerProg].CoorY;
                    if (IsTargetHorizontal)
                    {
                        if (IsValidNextCoor(progX + 1, progY))
                        {
                            HitAdd(progX + 1, progY);
                            if (Rounds[Rounds.Count - 1].IsHit == true)
                            {
                                RegisteredHitPointerProg = Rounds.Count - 1;
                            }
                            targetStatus = TargetStatus.AttemptBackward;
                            return true;
                        }
                    }
                    else
                    {
                        if (IsValidNextCoor(progX, progY + 1))
                        {
                            HitAdd(progX, progY + 1);
                            if (Rounds[Rounds.Count - 1].IsHit == true)
                            {
                                RegisteredHitPointerProg = Rounds.Count - 1;
                            }
                            targetStatus = TargetStatus.AttemptBackward;
                            return true;
                        }
                    }
                    targetStatus = TargetStatus.AttemptBackward;
                    TTL++;

                }
                if (targetStatus == TargetStatus.AttemptBackward)
                {
                    int progX = Rounds[RegisteredHitPointerInit].CoorX;
                    int progY = Rounds[RegisteredHitPointerInit].CoorY;
                    if (IsTargetHorizontal)
                    {
                        if (IsValidNextCoor(progX - 1, progY))
                        {
                            HitAdd(progX - 1, progY);
                            if (Rounds[Rounds.Count - 1].IsHit == true)
                            {
                                RegisteredHitPointerInit = Rounds.Count - 1;
                            }
                            targetStatus = TargetStatus.AttemptForward;
                            return true;
                        }
                    }
                    else
                    {
                        if (IsValidNextCoor(progX, progY - 1))
                        {
                            HitAdd(progX, progY - 1);
                            if (Rounds[Rounds.Count - 1].IsHit == true)
                            {
                                RegisteredHitPointerInit = Rounds.Count - 1;
                            }
                            targetStatus = TargetStatus.AttemptForward;
                            return true;
                        }
                    }
                    targetStatus = TargetStatus.AttemptForward;
                    TTL++;
                }
            }
        }
        private void swapPointers()
        {
            (RegisteredHitPointerInit, RegisteredHitPointerProg) = (RegisteredHitPointerProg, RegisteredHitPointerInit);
        }
        private void HitAdd(int x, int y)
        {
            Rounds.Add(new RoundMem(x, y, Hit(x, y)));
        }
        private bool Hit(int x, int y)
        {
            return (TargetBoard.Hit(x, y) == PlayerBoard.SquareType.NewHit);
        }
        private bool CoorOutOfBoudsCheck(int x, int y)
        {   
            if (x < 0 || y < 0) return false;
            if (x > PlayerBoard.BOARD_SIZE - 1 ||  y > PlayerBoard.BOARD_SIZE - 1) return false; 
            return true;
        }
        private (int, int) FindTarget(int x, int y)
        {
            int targetX = x + 1;
            int targetY = y;
            if (CoorOutOfBoudsCheck(targetX, targetY)) 
            {
                if (!IsAlreadyHit(targetX, targetY))
                {
                    return (targetX, targetY);
                }
            }
            targetX = x - 1;
            targetY = y;
            if (CoorOutOfBoudsCheck(targetX, targetY))
            {
                if (!IsAlreadyHit(targetX, targetY))
                {
                    return (targetX, targetY);
                }
            }
            targetX = x;
            targetY = y + 1;
            if (CoorOutOfBoudsCheck(targetX, targetY))
            {
                if (!IsAlreadyHit(targetX, targetY))
                {
                    return (targetX, targetY);
                }
            }
            targetX = x;
            targetY = y - 1;
            if (CoorOutOfBoudsCheck(targetX, targetY))
            {
                if (!IsAlreadyHit(targetX, targetY))
                {
                    return (targetX, targetY);
                }
            }
            return (x, y);
        }
        private bool IsValidNextCoor(int x, int y)
        {
            int targetX = x;
            int targetY = y;
            if (CoorOutOfBoudsCheck(targetX, targetY))
            {
                if (!IsAlreadyHit(targetX, targetY))
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsAlreadyHit(int x, int y)
        {
            foreach (RoundMem Round in Rounds)
            {
                if (Round.CoorX == x && Round.CoorY == y) { return true; }
            }
            return false;
        }
    }
}
