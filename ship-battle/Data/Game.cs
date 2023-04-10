using Microsoft.AspNetCore.Components.Web;
using static ship_battle.Data.PlayerBoard;
using static ship_battle.Data.Ship;
using ship_battle.Data.ComputerPlayerData;

namespace ship_battle.Data
{
    public class Game
    {
        ComputerPlayer computerPlayer;
        public int RoundNum { get; set; }
        public bool[,] BoardSetupHoverArray { get; private set; }
        public bool SetupRotate { get; set; }
        public GameStateType GameState { get; private set; }
        public enum GameStateType
        {
            Setup,
            Ready,
            GameOver,

        }
        public GameOverType GameOver { get; private set; }
        public enum GameOverType
        {
            Win,
            Lose,
            Draw,

        }
        public PlayerBoard EnemyBoard { get; set; }
        public PlayerBoard FriendlyBoard { get; set; }
        public Game()
        {
            BoardSetupHoverArray = new bool[BOARD_SIZE, BOARD_SIZE];
            SetupRotate = false;
            GameState = GameStateType.Setup;
            EnemyBoard = new PlayerBoard(PlayerType.Enemy);
            FriendlyBoard = new PlayerBoard(PlayerType.Friendly);
            RoundNum = 0;
            RandStringNum enemySeed = new(30);
            Console.WriteLine(enemySeed);
            EnemyBoard.Populate(enemySeed);
            computerPlayer = new(FriendlyBoard);
        }
        public void BoardClick(int col, int row, bool isEnemyGrid)
        {
            // game over mode
            if (GameState == GameStateType.GameOver) { return; }
            // setup mode
            if (GameState == GameStateType.Setup)
            {
                if (isEnemyGrid) { return; }
                foreach (ShipType shipType in Enum.GetValues(typeof(ShipType)))
                {
                    if (shipType == ShipType.None) continue;
                    if (FriendlyBoard.IsAlreadyAdded(shipType)) { continue; }
                    FriendlyBoard.AddShip(new Ship(shipType, col, row, SetupRotate));
                    return;
                }
                return;
            }
            // friendly turn
            if (RoundNum % 2 == 0 && isEnemyGrid) {
                SquareType currType = EnemyBoard.Hit(col, row);
                if (currType == SquareType.NewMiss
                    || currType == SquareType.NewHit)
                {
                    RoundNum++;
                }
            }
            // enemy turn
            if (RoundNum % 2 == 1)
            {
                computerPlayer.PlayTurn();
                RoundNum++;
                if (EnemyBoard.IsEveryShipSunk() || FriendlyBoard.IsEveryShipSunk()) { GameState = GameStateType.GameOver; }
            }
            
        }
        public void ClearBoard(PlayerBoard board)
        {
            GameState = GameStateType.Setup;
            board.ClearBoard();

        }
        public static bool RemoveShip(PlayerBoard board, Ship ship)
        {
            return board.RemoveShip(ship);
        }
        public void RemoveLastFriendlyShip()
        {
            if (GameState!= GameStateType.Setup) { return; }
            int ship_num = FriendlyBoard.Ships.Count;
            if (ship_num > 0)
            {
                FriendlyBoard.Ships.RemoveAt(ship_num - 1);
            }
        }
        public void RandomSetup()
        {
            if (GameState == GameStateType.Setup) {
                ClearBoard(FriendlyBoard);
                RandStringNum friendlySeed = new(30);
                FriendlyBoard.Populate(friendlySeed);
                Console.WriteLine(friendlySeed);
            }
        }
        public void StartGame()
        {
            if(IsEveryFriendlyShipAdded() != ShipType.None) { return; }
            if(GameState != GameStateType.Setup) { return; }
            GameState = GameStateType.Ready;
        }
        public ShipType IsEveryFriendlyShipAdded()
        {
            foreach (ShipType shipType in Enum.GetValues(typeof(ShipType)))
            {
                if (shipType == ShipType.None) continue;
                if (!FriendlyBoard.IsAlreadyAdded(shipType)) { return shipType; }
            }
            return ShipType.None;
        }
        public void ResetSetup()
        {
            if (GameState!= GameStateType.Setup) { return; }
            ClearBoard(FriendlyBoard);
        }
        public void ResetGame()
        {
            ClearBoard(FriendlyBoard);
            ClearBoard(EnemyBoard);
            GameState = GameStateType.Setup;
            EnemyBoard = new PlayerBoard(PlayerType.Enemy);
            FriendlyBoard = new PlayerBoard(PlayerType.Friendly);
            RoundNum = 0;
            RandStringNum enemySeed = new(30);
            Console.WriteLine(enemySeed);
            EnemyBoard.Populate(enemySeed);
            computerPlayer = new(FriendlyBoard);
        }
        public void BoardSetupHover(int col, int row)
        {
            if (GameState != GameStateType.Setup) { return; }
            ShipType nextShip = IsEveryFriendlyShipAdded();
            if (nextShip == ShipType.None) { return; }
            int nextShipLength = Length(nextShip);
            if (!SetupRotate) {
                // if (row + nextShipLength > BOARD_SIZE) { return;}
                for (int i = 0; i < nextShipLength; i++)
                {
                    if ((row + i) > BOARD_SIZE-1) { continue; }
                    BoardSetupHoverArray[col, i + row] = true;
                }
                return;
            }
            // if (col + nextShipLength > BOARD_SIZE) { return; }
            for (int i = 0; i < nextShipLength; i++)
            {
                if ((col + i) > BOARD_SIZE-1) { continue; }
                BoardSetupHoverArray[i + col, row] = true;
            }
        }
        public bool IsBoardSetupHover(int col, int row)
        {
            return BoardSetupHoverArray[col, row];
        }
        public void BoardSetupReset()
        {
            BoardSetupHoverArray = new bool[BOARD_SIZE, BOARD_SIZE];
        }
        public void EvaluateGameOver()
        {
            if (FriendlyBoard.IsEveryShipSunk() && EnemyBoard.IsEveryShipSunk()) { GameOver = GameOverType.Draw; return; }
            if (FriendlyBoard.IsEveryShipSunk()) { GameOver = GameOverType.Lose; return; }
            GameOver = GameOverType.Win;
        }
        public string GetGameOverString()
        {
            if (GameOver == GameOverType.Lose) { return "lose"; }
            if (GameOver == GameOverType.Win) { return "win"; }
            return "draw";
        }
    }
}
