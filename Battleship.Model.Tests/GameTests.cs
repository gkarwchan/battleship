using System;
using Battleship.Model.Tests.fakes;
using FluentAssertions;
using Moq;
using NSpec;
using NSpec.Assertions;

namespace Battleship.Model.Tests
{
    public class Game_Tests : nspec
    {
        private Mock<IBoard> _firstPlayerBoard;
        private Mock<IBoard> _secondPlayerBoard;
        private Mock<IUserInputToCoordinateConverter> _userInputConverter;
        void before_each()
        {
            _firstPlayerBoard = new Mock<IBoard>();
            _secondPlayerBoard = new Mock<IBoard>();
            _userInputConverter = new Mock<IUserInputToCoordinateConverter>();
        }


        private void when_create_new_game()
        {
            Game _subject = null;
            before = () =>
            {
                _firstPlayerBoard.Setup(x => x.PlayerDispalyName).Returns("p1");
                _secondPlayerBoard.Setup(x => x.PlayerDispalyName).Returns("p2");
                _subject = new Game(_firstPlayerBoard.Object, _secondPlayerBoard.Object, _userInputConverter.Object);
            };
            it["current board should be first player board"] = () =>
            {
                _subject.CurrentPlayerName.ShouldBeEquivalentTo("p1");
            };

            it["current player name should be first player name"] = () =>
            {
                _subject.CurrentPlayerName.ShouldBeEquivalentTo("p1");
            };
            it["current opponent name should be second player name"] = () =>
            {
                _subject.OppenentPlayerName.ShouldBeEquivalentTo("p2");
            };

        }

        private void when_player_try_to_get_the_game_boards()
        {
            Game _game = null;
            BoardCellStatus[,] _subject = null;
            before = () =>
            {
                var stubOne = new BoardFake("p1");
                var stubTwo = new BoardFake("p2");
                _game = new Game(stubOne, stubTwo, _userInputConverter.Object);
                
            };

            it["should be able to see all Cell Status of the first player board"] = () =>
            {
                var currentPlayerBoard = _game.GetPlayerBoard(true);
                currentPlayerBoard[0,0].ShouldBeEquivalentTo(BoardCellStatus.Empty);
                currentPlayerBoard[0,1].ShouldBeEquivalentTo(BoardCellStatus.Ship);
                currentPlayerBoard[1,0].ShouldBeEquivalentTo(BoardCellStatus.Miss);
                currentPlayerBoard[1,1].ShouldBeEquivalentTo(BoardCellStatus.Hit);
            };

            it["should be able to see all Cell Status of the second player board"] = () =>
            {
                var currentPlayerBoard = _game.GetPlayerBoard(false);
                currentPlayerBoard[0, 0].ShouldBeEquivalentTo(BoardCellStatus.Empty);
                currentPlayerBoard[0, 1].ShouldBeEquivalentTo(BoardCellStatus.Ship);
                currentPlayerBoard[1, 0].ShouldBeEquivalentTo(BoardCellStatus.Miss);
                currentPlayerBoard[1, 1].ShouldBeEquivalentTo(BoardCellStatus.Hit);
            };
            
        }

        private void when_player_place_ship()
        {
            Game _subject = null;
            Tuple<Coordinate, Coordinate> coords = new Tuple<Coordinate, Coordinate>(new Coordinate(1, 1), new Coordinate(1, 3));
            before = () =>
            {
                _firstPlayerBoard.Setup(x => x.PlayerDispalyName).Returns("p1");
                _secondPlayerBoard.Setup(x => x.PlayerDispalyName).Returns("p2");

                _userInputConverter.Setup(x => x.ConvertUserInputToDoubleCoordinate("B2 D2")).Returns(coords);
                _subject = new Game(_firstPlayerBoard.Object, _secondPlayerBoard.Object, _userInputConverter.Object);

            };

            it["should call place ship for current board"] = () =>
            {
                _subject.PlaceShipInCurrentBoard("B2 D2");


                _firstPlayerBoard.Verify(x => x.PlaceShip(
                    It.Is<Coordinate>(cor => cor.Row == 1 && cor.Column == 1),
                    It.Is<Coordinate>(cor => cor.Row == 1 && cor.Column == 3)
                    ));

            };

            it["should switch the player"] = () =>
            {
                _subject.PlaceShipInCurrentBoard("B2 D2");
                _subject.CurrentPlayerName.ShouldBeEquivalentTo("p2");
            };

            context["given the user input is invalid"] = () =>
            {
                before = () =>
                {
                    _userInputConverter.Setup(x => x.ConvertUserInputToDoubleCoordinate("something-stupid")).Returns((Tuple<Coordinate, Coordinate>)null);
                };

                it["should throw Argument exception"] = expect<ArgumentOutOfRangeException>(() =>
                {
                    _subject.PlaceShipInCurrentBoard("something-stupid");
                });
            };


        }

        private void when_player_play_turn()
        {
            Game _subject = null;
            Coordinate cord = new Coordinate(1,1);
            before = () =>
            {
                _firstPlayerBoard.Setup(x => x.PlayerDispalyName).Returns("p1");
                _secondPlayerBoard.Setup(x => x.PlayerDispalyName).Returns("p2");
                _userInputConverter.Setup(x => x.ConvertUserInputToCoordinate("B2")).Returns(cord);
                _subject = new Game(_firstPlayerBoard.Object, _secondPlayerBoard.Object, _userInputConverter.Object);
            };

            it["should switch the player"] = () =>
            {
                _subject.PlayerPlayTurn("B2");
                _subject.CurrentPlayerName.ShouldBeEquivalentTo("p2");
            };

            context["given the hit result in all ships sunk"] = () =>
            {
                before = () => { _secondPlayerBoard.Setup(x => x.AllShipsSunked).Returns(true); };
                it["should set isGameOver to true"] = () =>
                {
                    _subject.PlayerPlayTurn("B2");
                    _subject.IsGameOver.ShouldBeTrue();
                };

            };


            context["given the user input is invalid"] = () =>
            {
                before = () =>
                {
                    _userInputConverter.Setup(x => x.ConvertUserInputToCoordinate("something-stupid")).Returns((Coordinate) null);
                };

                it["should throw Argument exception"] = expect<ArgumentOutOfRangeException>(() =>
                {
                    _subject.PlayerPlayTurn("something-stupid");
                });
            };

        }
        
    }


    
}