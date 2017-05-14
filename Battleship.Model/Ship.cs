using System;
using System.Linq;

namespace Battleship.Model
{
    public class Ship
    {
        public ShipUnit[] ShipUnits { get; private set; }

        public Ship(Coordinate[] locations)
        {
            ShipUnits = locations.Select(x => new ShipUnit(x)).ToArray();
        }

        public void Hit(Coordinate coord)
        {
            var shipUnit = Array.IndexOf(ShipUnits, coord);
            this.ShipUnits[shipUnit].WasHit = true;
        }

        public bool IsSunk
        {
            get
            {
                var allHit = true;
                Array.ForEach(this.ShipUnits, unit => allHit &= unit.WasHit);
                return allHit;
            }
        }
    }
}