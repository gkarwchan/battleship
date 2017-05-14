using System;
using NSpec;
using NSpec.Assertions;

namespace Battleship.Model.Tests
{
    public class Ship_Tests : nspec
    {
        private Ship _subject;
        private Coordinate[] _coord;

        void before_each()
        {
            _coord = new Coordinate[] { new Coordinate(3, 4), new Coordinate(3, 5), new Coordinate(3, 6) };
            
        }
        private void when_create_ship()
        {
            before = () => _subject = new Ship(_coord);
            it["should mark all WasHit as false"] = () =>
            {
                Array.ForEach(_subject.ShipUnits, x => x.WasHit.ShouldBeFalse());
            };
        }

        private void when_hit_ship()
        {
            before = () => _subject = new Ship(_coord);
            context["given we assign valid coordinate"] = () =>
            {
                it["should mark WasHit as true"] = () =>
                {

                    _subject.Hit(new Coordinate(3, 5));
                    _subject.ShipUnits[1].WasHit.ShouldBeTrue();
                    _subject.ShipUnits[0].WasHit.ShouldBeFalse();
                    _subject.ShipUnits[2].WasHit.ShouldBeFalse();
                };
            };

            context["given we hit one unit only"] = () =>
            {
                it["should keep IsSunk as false"] = () =>
                {

                    _subject.Hit(new Coordinate(3, 5));
                    _subject.IsSunk.ShouldBeFalse();
                };
            };

            context["given we hit all units"] = () =>
            {
                it["should IsSunk return true"] = () =>
                {
                    _subject.Hit(new Coordinate(3, 4));
                    _subject.Hit(new Coordinate(3, 5));
                    _subject.Hit(new Coordinate(3, 6));
                    _subject.IsSunk.ShouldBeTrue();
                };
            };
        }
    }
}