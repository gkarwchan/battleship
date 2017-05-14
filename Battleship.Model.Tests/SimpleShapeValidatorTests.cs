using NSpec;
using NSpec.Assertions;

namespace Battleship.Model.Tests
{
    public class SimpleShapeValidator_Tests : nspec
    {
        SimpleShapeValidator _subject;

        void before_each()
        {
            _subject = new SimpleShapeValidator();
        }
        private void when_validate_ship()
        {
            context["given start and end points have different column and row"] = () =>
            {
                it["should return Not Valid"] =
                    () =>
                    {
                        var valid = _subject.IsValidShapeForShip(new Coordinate(2, 2), new Coordinate(3, 3));
                        valid.ShouldBeFalse();
                    };
            };

            context["given ship size is bigger than 3"] = () =>
            {
                it["should return Not Valid"] =
                    () =>
                    {
                        var valid = _subject.IsValidShapeForShip(new Coordinate(2, 2), new Coordinate(2, 5));
                        valid.ShouldBeFalse();
                    };
            };

            context["given start and end on the same column and ship size is 3"] = () =>
            {
                it["should return Valid"] =
                    () =>
                    {
                        var valid = _subject.IsValidShapeForShip(new Coordinate(2, 2), new Coordinate(4, 2));
                        valid.ShouldBeTrue();
                    };
            };
            
        }
    }
}