namespace Battleship.Model
{
    public class ShipUnit : Coordinate
    {
        public bool WasHit { get; set; }

        public ShipUnit(Coordinate coord) : base(coord.Row, coord.Column)
        {
            this.WasHit = false;
        }

        
    }
}