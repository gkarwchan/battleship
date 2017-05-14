using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Battleship.Model
{
    public class Board : IBoard
    {
        private readonly IShipShapeValidator _shapeValidator;
        private readonly BoardCellStatus[,] _board;


        

        public string PlayerDispalyName { get; set; }
        public IList<Ship> Ships { get; private set; }
        

        /// <summary>
        /// Board is the board where player place his / her ships, and hit opponent board
        /// </summary>
        /// <param name="playerName">We didn't define a player class, because it will has only one property which is a name, 
        /// so instead I we added this property to the board</param>
        /// <param name="rowSize">
        /// we specify the rowSize (in our simple exercies it is 8)
        /// </param>
        /// <param name="columnSize">
        /// we specify the rowSize (in our simple exercies it is 8)
        /// </param>
        /// <param name="shapeValidator">
        /// shapeValidator is IShipShapeValidator will validate the ship. By passing this as an injected dependency
        ///  we can adapt to any rules about ship shape in the future, by changing only the implementation of this
        ///  class without effecting the whole code
        /// </param>
        public Board(string playerName, int rowSize, int columnSize, IShipShapeValidator shapeValidator)
        {
            _shapeValidator = shapeValidator;

            if (rowSize < 0 || columnSize < 0)
                throw new ArgumentOutOfRangeException();
            _board = new BoardCellStatus[rowSize, columnSize];
            PlayerDispalyName = playerName;
            Ships = new List<Ship>();
        }

        // the default shape validator is the simpleShapeValidator
        public Board(string playerName, int rowSize, int columnSize)
            : this(playerName, rowSize, columnSize, new SimpleShapeValidator())
        {
        }


        public BoardCellStatus[,] Cells
        {
            get { return _board; }
        }

        public void PlaceShip(Coordinate startCoordinate, Coordinate endCoordinate)
        {
            // Validate ship location and shape
            if (IsCoordinateOutsideBoard(startCoordinate) || IsCoordinateOutsideBoard(endCoordinate)) throw new ArgumentOutOfRangeException();
            if (!_shapeValidator.IsValidShapeForShip(startCoordinate, endCoordinate)) throw new NotValidShapeForShipException();
            var coords = GetShipCoordinates(startCoordinate, endCoordinate);
            
            // Validate if location is not empty. Not required in our simple exercise, but for a real solution
            if (!IsShipPlaceEmpty(coords)) throw new ShipPlaceIsNotEmptyException();

            /* all validation passes then 
            1 - mark all board cells as BoardCellStatus.Ship
            2 - and add it to the ship list */
            Array.ForEach(coords, x => _board[x.Row, x.Column] = BoardCellStatus.Ship);
            Ships.Add(new Ship(coords));
        }

        public void Hit(Coordinate hitPoint)
        {
            if (IsCoordinateOutsideBoard(hitPoint)) throw new ArgumentOutOfRangeException();
            if (_board[hitPoint.Row, hitPoint.Column] == BoardCellStatus.Empty)
                _board[hitPoint.Row, hitPoint.Column] = BoardCellStatus.Miss;

            // if the player hit a cell previously hit, then no need to do anything. No need to break his heart and tell him he lost his turn
            if (_board[hitPoint.Row, hitPoint.Column] != BoardCellStatus.Ship) return;
            
            // otherwise , BINGO, we hit the ship
            _board[hitPoint.Row, hitPoint.Column] = BoardCellStatus.Hit;
            // find the ship which has a shipUnit with the same coordinate and mark its unit's WasHit to true
            var ship = this.Ships.FirstOrDefault(x => x.ShipUnits.Contains(hitPoint));
            if (ship == null) throw new  ArgumentOutOfRangeException();
            ship.ShipUnits.First(x => x.Equals(hitPoint)).WasHit = true;
        }

        private bool IsCoordinateOutsideBoard(Coordinate startCoordinate)
        {
            return startCoordinate.Row > BoardRowSize || startCoordinate.Column > BoardColumnSize;
        }


        public int BoardColumnSize
        {
            get { return _board.GetLength(1); }
        }

        public int BoardRowSize
        {
            get { return _board.GetLength(0); }
        }

        public bool AllShipsSunked
        {
            get { return this.Ships.ToList().TrueForAll(x => x.IsSunk); }
        }

        private bool IsShipPlaceEmpty(Coordinate[] coords)
        {
            return Array.TrueForAll(coords, x => _board[x.Row, x.Column] == BoardCellStatus.Empty );
        }

        private Coordinate[] GetShipCoordinates(Coordinate startCoordinate, Coordinate endCoordinate)
        {
            var coords = new List<Coordinate>();
            // check if it is vertical or horizontal
            if (startCoordinate.Column == endCoordinate.Column)
            {
                // it is vertical
                var startRow = endCoordinate.Row > startCoordinate.Row ? startCoordinate.Row : endCoordinate.Row;
                var length = Math.Abs(endCoordinate.Row - startCoordinate.Row);
                for (var i = 0; i <= length; i++)
                    coords.Add(new Coordinate(startRow + i, startCoordinate.Column));
            }
            else
            {
                // it is horizontal
                var startColumn = endCoordinate.Column > startCoordinate.Column
                    ? startCoordinate.Column
                    : endCoordinate.Column;
                var length = Math.Abs(endCoordinate.Column - startCoordinate.Column);
                for (var i = 0; i <= length; i++)
                    coords.Add(new Coordinate(startCoordinate.Row, startColumn + i));
            }
            return coords.ToArray();
        }
        
    }
}