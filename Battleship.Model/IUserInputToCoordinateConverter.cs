using System;

namespace Battleship.Model
{
    /// <summary>
    /// This contract will be responsible of converting the user input to coordinate.
    /// The Board, Game deals only with coordinates.
    /// The user input like A3, will be handled here, 
    /// and converted to coordinate.
    /// This injection of this service into the game allows loose coupling, so in the future we can change the rules, 
    /// and have the user input in different format (maybe something like 4-5)
    /// </summary>
    public interface IUserInputToCoordinateConverter
    {
        Coordinate ConvertUserInputToCoordinate(string userInput);
        Tuple<Coordinate, Coordinate> ConvertUserInputToDoubleCoordinate(string shipInput);
    }
}