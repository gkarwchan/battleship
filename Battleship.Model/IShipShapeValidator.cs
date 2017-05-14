namespace Battleship.Model
{
    /// <summary>
    /// IShipShapeValidator is the contract that is responsible of applying the validation rules on Ship's Shape.
    /// We will inject the implementation of this into Board
    /// We can adapt to any rule change in the game easily if it is related to the shape of the ship, 
    /// by changing the implementation of this contract without effecting the whole code
    /// </summary>
    public interface IShipShapeValidator
    {
        bool IsValidShapeForShip(Coordinate startCoordinate, Coordinate endCoordinate);
    }
}