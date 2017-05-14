namespace Battleship.Model
{
    public enum BoardCellStatus
    {
        Empty,
        Ship,
        Hit,
        Miss
    };

    public interface IBoard
    {
        
        BoardCellStatus[,] Cells { get; }
        void PlaceShip(Coordinate startCoordinate, Coordinate endCoordinate);
        void Hit(Coordinate hitPoint);
        string PlayerDispalyName { get; set; }
        int BoardColumnSize { get;  }
        int BoardRowSize { get; }
        bool AllShipsSunked { get; }
    }
}