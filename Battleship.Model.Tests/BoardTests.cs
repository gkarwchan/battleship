using System;
using NSpec;
using FluentAssertions;
using Moq;
using NSpec.Assertions;


namespace Battleship.Model.Tests
{
    
    
    public class Board_tests : nspec
    {
        private Mock<IShipShapeValidator> _validatorMock;
        void before_each()
        {
            _validatorMock = new Mock<IShipShapeValidator>();
        }


        private void when_create_new_board()
        {
            context["given we assign negative RowSize value"] = () =>
            {
                it["should throw Out of range exception"] =
                    expect<ArgumentOutOfRangeException>(() => { var board = new Board("p1", -1, 3, _validatorMock.Object); });
            };

            context["given we assign negative ColumnSize value"] = () =>
            {
                it["should throw Out of range exception"] =
                    expect<ArgumentOutOfRangeException>(() => { var board = new Board("p1", 4, -13, _validatorMock.Object); });
            };

            it["should assign all board units as empty"] = () =>
            {
                var board = new Board("p1", 4, 4, _validatorMock.Object);
                for (var i = 0; i < 4; i++)
                    for (var j = 0; j < 4; j++)
                        board.Cells[i, j].ShouldBeEquivalentTo(BoardCellStatus.Empty);
            };
        }

        private void when_get_board_size()
        {
            Board _subject = null;
            before = () => _subject = new Board("p1", 5, 9);

            it["should get BoardRowSize Correctly"] = () => _subject.BoardRowSize.ShouldBeEquivalentTo(5);
            it["should get BoardColumnSize Correctly"] = () => _subject.BoardColumnSize.ShouldBeEquivalentTo(9);        

        }

        private void when_place_ship()
        {
            Board _subject = null;
            before = () =>
            {
                _subject = new Board("P1", 8, 7, _validatorMock.Object);
            };


           
            it["should call ShapeValidator with exact start and end coordinate"] = () =>
            {
                _validatorMock.Setup(
                   x => x.IsValidShapeForShip(
                       It.IsAny<Coordinate>(),
                       It.IsAny<Coordinate>()
                       )).Returns(true);
                _subject.PlaceShip(new Coordinate(3, 3), new Coordinate(3, 5) );
               _validatorMock.Verify(x => x.IsValidShapeForShip(
                   It.Is<Coordinate>(cor => cor.Row == 3 && cor.Column == 3),
                   It.Is<Coordinate>(cor => cor.Row == 3 && cor.Column == 5)
                   ));
            };

            context["given we passed start coordinate outside the board"] = () =>
            {
                it["should throw exception ArgumentOutOfRangeException"] = expect<ArgumentOutOfRangeException>(() =>
                {
                    _subject.PlaceShip(new Coordinate(10, 20), new Coordinate(3, 5));
                });
            };

            context["given the shapeValidator return false"] = () =>
            {
                before = () => _validatorMock.Setup(x => x.IsValidShapeForShip(It.IsAny<Coordinate>(), It.IsAny<Coordinate>()))
                            .Returns(false);
                it["should throw out of NotValidShapeForShip exception"] = expect<NotValidShapeForShipException>(() =>
                {
                    _subject.PlaceShip(new Coordinate(3, 3), new Coordinate(4, 4));
                });
            };

            context["given ship is valid and the board is empty"] = () =>
            {
                before = () =>
                {
                    _validatorMock.Setup(x => x.IsValidShapeForShip(It.IsAny<Coordinate>(), It.IsAny<Coordinate>()))
                        .Returns(true);
                    _subject.PlaceShip(new Coordinate(1, 3), new Coordinate(3, 3));
                };
                it["should mark all target board units as Ship"] = () =>
                {
                    for (var i = 1; i < 4; i++)
                        _subject.Cells[i, 3].ShouldBeEquivalentTo(BoardCellStatus.Ship);
                };

                it["and it should add a new ship in the ship list of the board"] = () =>
                {
                    _subject.Ships.Count.ShouldBeEquivalentTo(1);
                };
                it["and the coordinates of the added ship should match the end point and start point"] = () =>
                {
                    _subject.Ships[0].ShipUnits[0].Row.ShouldBeEquivalentTo(1);
                    _subject.Ships[0].ShipUnits[0].Column.ShouldBeEquivalentTo(3);
                    // end point of the ship
                    _subject.Ships[0].ShipUnits[2].Row.ShouldBeEquivalentTo(3);
                    _subject.Ships[0].ShipUnits[2].Column.ShouldBeEquivalentTo(3);
                };
            };

            context["given the target place is occupied by another ship"] = () =>
            {
                before = () => _validatorMock.Setup(x => x.IsValidShapeForShip(It.IsAny<Coordinate>(), It.IsAny<Coordinate>()))
                       .Returns(true);
                it["should throw PlaceNotEmpty exception"] = expect<ShipPlaceIsNotEmptyException>(() =>
                {
                    _subject.PlaceShip(new Coordinate(2, 3), new Coordinate(2, 5));
                    _subject.PlaceShip(new Coordinate(2, 4), new Coordinate(4, 4));
                });
            };

        }

        private void when_hit_board()
        {
            Board _subject = null;
            before = () =>
            {
                _subject = new Board("P1", 8, 7, _validatorMock.Object);
                _validatorMock.Setup(x => x.IsValidShapeForShip(It.IsAny<Coordinate>(), It.IsAny<Coordinate>()))
                        .Returns(true);
                _subject.PlaceShip(new Coordinate(4, 4), new Coordinate(4, 6));
            };

            context["given we passed hit point outside the board"] = () =>
            {
                it["should throw exception ArgumentOutOfRangeException"] = expect<ArgumentOutOfRangeException>(() =>
                {
                    _subject.Hit(new Coordinate(10, 20));
                });
            };

            context["given we hit empty cell"] = () =>
            {
                it["should update the status to Miss"] = () =>
                {
                    _subject.Hit(new Coordinate(2, 2));
                    _subject.Cells[2,2].ShouldBeEquivalentTo(BoardCellStatus.Miss);
                };
            };

            context["given we hit ship cell"] = () =>
            {
                it["should update the status to Hit"] = () =>
                {
                    _subject.Hit(new Coordinate(4, 5));
                    _subject.Cells[4, 5].ShouldBeEquivalentTo(BoardCellStatus.Hit);
                };

                it["and it should update the attribute WasHit of the ShipUnit that was hit to true"] = () =>
                {
                    _subject.Hit(new Coordinate(4, 5));
                    _subject.Ships[0].ShipUnits[1].WasHit.ShouldBeTrue();
                };
            };

            

            context["given we hit all ship units"] = () =>
            {
                it["should update AllShipsSunk to true"] = () =>
                {
                    _subject.Hit(new Coordinate(4, 4));
                    _subject.Hit(new Coordinate(4, 5));
                    _subject.Hit(new Coordinate(4, 6));
                    _subject.AllShipsSunked.ShouldBeTrue();
                };

            };

        }

    }

    
}