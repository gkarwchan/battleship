namespace Battleship.Model.Tests.fakes
{
    // This stub class to facilitate testing the OpponentBoardStatus

    public class BoardFake : IBoard
    {
        private BoardCellStatus[,] _stubStatus;

        public BoardFake(string playerName)
        {
            _stubStatus = new BoardCellStatus[2,2];
            _stubStatus[0,0] = BoardCellStatus.Empty;
            _stubStatus[0, 1] = BoardCellStatus.Ship;
            _stubStatus[1, 0] = BoardCellStatus.Miss;
            _stubStatus[1, 1] = BoardCellStatus.Hit;
            this.PlayerDispalyName = playerName;
        }
      
        public BoardCellStatus[,] Cells
        {
            get { return _stubStatus; }
        }

        public void PlaceShip(Coordinate startCoordinate, Coordinate endCoordinate)
        {
            throw new System.NotImplementedException();
        }

        public void Hit(Coordinate hitPoint)
        {
            throw new System.NotImplementedException();
        }

        public string PlayerDispalyName { get; set; }

        public int BoardColumnSize
        {
            get { return 2; }
        }

        public int BoardRowSize
        {
            get { return 2; }
        }

        public bool AllShipsSunked
        {
            get { throw new System.NotImplementedException(); }
        }
    } 
}