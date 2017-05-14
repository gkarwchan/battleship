using System;

namespace Battleship.Model
{

    /// <summary>
    /// This simple shape validator is for the purpose of the exercise
    /// it only support one dimensional shape (row, or column), and 3 length of size ship
    /// </summary>
    public class SimpleShapeValidator : IShipShapeValidator
    {
        public bool IsValidShapeForShip(Coordinate startCoordinate, Coordinate endCoordinate)
        {

            var isOnTheSameRowOrColumn = startCoordinate.Column == endCoordinate.Column || startCoordinate.Row == endCoordinate.Row;
            if (!isOnTheSameRowOrColumn)
                return false;
            var shipLength = startCoordinate.Column == endCoordinate.Column
                    ? Math.Abs(startCoordinate.Row - endCoordinate.Row) + 1
                    : Math.Abs(startCoordinate.Column - endCoordinate.Column) + 1;
            return shipLength == 3;

        }
    }
}