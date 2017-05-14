using System;

namespace Battleship.Model
{
    // this excepton will be thrown when the user enter  being ship and end ship coordinations not on the same row or column
    public class NotTheSameAxisException : ApplicationException
    {

    }

    /// <summary>
    /// This exception will be used when we use Board.PlaceShip and the ship doesn't fit on the board
    /// </summary>
    public class ShipCannotFitOnBoardException : ApplicationException
    {

    }

    /// <summary>
    /// This exception will be used when we use Board.PlaceShip and the target place is not empty
    /// </summary>
    public class ShipPlaceIsNotEmptyException : ApplicationException
    {

    }

    /// <summary>
    /// This exception will be used when we use Board.PlaceShip and the target place is not empty
    /// </summary>
    public class NotValidShapeForShipException : ApplicationException
    {

    }
}