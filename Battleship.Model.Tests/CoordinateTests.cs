using System;
using NSpec;
using NSpec.Assertions;

namespace Battleship.Model.Tests
{
    public class Coordinate_tests : nspec
    {
        void when_create_new_coordinate()
        {
            context["given we assign negative Row value"] = () =>
            {
                it["should throw Out of range exception"] =
                    expect<ArgumentOutOfRangeException>(() => { var unit = new Coordinate(-1, 3); });
            };
            context["given we assign negative Column value"] = () =>
            {
                it["should throw Out of range exception"] =
                    expect<ArgumentOutOfRangeException>(() => { var unit = new Coordinate(5, -1); });
            };
        }

        void when_check_coordinate_equality()
        {
            context["given both column and row are equal"] = () =>
            {
                var coord1 = new Coordinate(3, 4);
                var coord2 = new Coordinate(3, 4);
                it["should return true"] = () =>
                {
                    coord1.Equals(coord2).ShouldBeTrue();
                };

            };

            context["given column or row different"] = () =>
            {
                var coord1 = new Coordinate(3, 4);
                var coord2 = new Coordinate(5, 9);
                it["should return false"] = () =>
                {
                    coord1.Equals(coord2).ShouldBeFalse();
                };

            };

        }

    }
}