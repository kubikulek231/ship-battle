

using Microsoft.AspNetCore.Components.Web;
using static ship_battle.Data.PlayerBoard;
using static ship_battle.Data.Ship;

namespace ship_battle.Data
{
    public class Game
    {
        public bool[,] BoardSetupHoverArray { get; private set; }
        public bool SetupRotate { get; set; }
        public GameStateType GameState { get; private set; }
        public enum GameStateType
        {
            Setup,
            Ready,
        }
        public DifficultyType Difficulty { get; private set; }
        public enum DifficultyType
        {
            Easy,
            Hard,

        }
        public PlayerBoard EnemyBoard { get; set; }
        public PlayerBoard FriendlyBoard { get; set; }
        public Game(DifficultyType difficulty)
        {
            BoardSetupHoverArray = new bool[BOARD_SIZE, BOARD_SIZE];
            SetupRotate = false;
            GameState = GameStateType.Setup;
            Difficulty = difficulty;
            EnemyBoard = new PlayerBoard(PlayerBoard.PlayerType.Enemy);
            FriendlyBoard = new PlayerBoard(PlayerBoard.PlayerType.Friendly);
            EnemyBoard.Populate(123456, 456123);
            
        }
        public void BoardClick(MouseEventArgs eventArgs, int col, int row, PlayerBoard playerBoard)
        {
            if (GameState == GameStateType.Ready) {
                playerBoard.Hit(col, row);
            }
            if (GameState == GameStateType.Setup) 
            {
                foreach (ShipType shipType in Enum.GetValues(typeof(ShipType)))
                {
                    if (shipType == Ship.ShipType.None) continue;
                    if (playerBoard.IsAlreadyAdded(shipType)) { continue; }
                    playerBoard.AddShip(new Ship(shipType, col, row, SetupRotate));
                    return;
                }
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
                FriendlyBoard.Ships.RemoveAt(ship_num -1);
            }
        }
        public void RandomSetup()
        {
            if (GameState == GameStateType.Setup) {
                ClearBoard(FriendlyBoard);
                FriendlyBoard.Populate(8794531, 789452);
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
        public void BoardSetupHover(MouseEventArgs eventArgs, int col, int row)
        {
            BoardSetupReset();
            if (GameState != GameStateType.Setup) { return; }
            ShipType nextShip = IsEveryFriendlyShipAdded();
            if (nextShip == ShipType.None) { return; }
            int nextShipLength = Length(nextShip);
            if (!SetupRotate) {
                //if (row + nextShipLength > BOARD_SIZE) { return;}
                for (int i = 0; i < nextShipLength; i++)
                {
                    if ((i + nextShipLength) > BOARD_SIZE) { continue; }
                    BoardSetupHoverArray[col, i + row] = true;
                }
                return;
            }
            if (col + nextShipLength > BOARD_SIZE) { return; }
            for (int i = 0; i < nextShipLength; i++)
            {
                //if (i + nextShipLength > BOARD_SIZE) { continue; }
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
    }
}
