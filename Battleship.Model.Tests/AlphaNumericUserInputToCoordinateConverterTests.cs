using System;
using FluentAssertions;
using NSpec;

namespace Battleship.Model.Tests
{
    public class AlphaNumericUserInputToCoordinateConverter_Tests : nspec
    {
        private AlphaNumericUserInputToCoordinateConverter _subject = null;

        private void before_each()
        {
            _subject = new AlphaNumericUserInputToCoordinateConverter();
        }

        private void when_convert_non_valid_input()
        {
            it["should return null"] = () =>
            {
                var coord = _subject.ConvertUserInputToCoordinate("any-invalid-value");
                coord.Should().BeNull();
            };
        }

        private void when_convert_valid_input()
        {
            Coordinate coord = null;
            before = () => coord = _subject.ConvertUserInputToCoordinate("D6");
            it["should return valid coordinate"] = () =>
            {
                coord.Should().NotBeNull();
                coord.Should().BeOfType<Coordinate>();
            };

            it["should convert alpha column to integer"] = () =>
            {
                coord.Column.ShouldBeEquivalentTo(3);
            };

            it["should set row to the number part in the string minus one"] = () =>
            {
                coord.Row.ShouldBeEquivalentTo(5);
            };

            it["should ignore the case"] = () =>
            {
                coord = _subject.ConvertUserInputToCoordinate("d6");
                coord.Row.ShouldBeEquivalentTo(5);
                coord.Column.ShouldBeEquivalentTo(3);
            };

            it["should ignore the spaces before and after"] = () =>
            {
                coord = _subject.ConvertUserInputToCoordinate("     N8     ");
                coord.Row.ShouldBeEquivalentTo(7);
                coord.Column.ShouldBeEquivalentTo(13);
            };
        }

        private void when_convert_double_coordinate_with_non_valid_input()
        {
            it["should return null"] = () =>
            {
                var coords = _subject.ConvertUserInputToDoubleCoordinate("any-invalid-value");
                coords.Should().BeNull();
            };
        }

        private void when_convert_double_coordinate_with_valid_input()
        {
            Tuple<Coordinate, Coordinate> coord = null;
            before = () => coord = _subject.ConvertUserInputToDoubleCoordinate("C6   N19");
            it["should return valid coordinate"] = () =>
            {
                coord.Should().NotBeNull();
                coord.Should().BeOfType<Tuple<Coordinate, Coordinate>>();
            };

            it["should convert the two coordinates correctly"] = () =>
            {
                coord.Item1.Column.ShouldBeEquivalentTo(2);
                coord.Item2.Column.ShouldBeEquivalentTo(13);
                coord.Item1.Row.ShouldBeEquivalentTo(5);
                coord.Item2.Row.ShouldBeEquivalentTo(18);
            };

            it["should ignore spaces before and after"] = () =>
            {
                coord = _subject.ConvertUserInputToDoubleCoordinate("A2   B9");
                coord.Item1.Column.ShouldBeEquivalentTo(0);
                coord.Item2.Column.ShouldBeEquivalentTo(1);
                coord.Item1.Row.ShouldBeEquivalentTo(1);
                coord.Item2.Row.ShouldBeEquivalentTo(8);
            };
        }

        private void when_one_entry_is_not_valid()
        {
            Tuple<Coordinate, Coordinate> coord = null;
            before = () => coord = _subject.ConvertUserInputToDoubleCoordinate("C6   JJ");
            it["should return null"] = () =>
            {
                coord.Should().BeNull();
            };

        }

        
    }
}