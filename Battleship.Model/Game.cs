
using System;

namespace Battleship.Model
{
    public class Game 
    {
        private readonly IBoard[] _boards;
        private bool _isFirstPlayerCurrentPlayer;
        private IUserInputToCoordinateConverter _userInputService;

        public bool IsGameOver { get; private set; }


        /// <summary>
        /// The game is responsible of managing the game.
        /// Switch the turn between players
        /// and specify when the game is over
        /// </summary>
        /// <param name="firstPlayerBoard">
        /// first player board is the first player that will play.
        /// </param>
        /// <param name="secondPlayerBoard">
        /// This is the second player board. this player will be next to the first
        /// </param>
        /// <param name="userInputConverter">
        /// user input Converter to Coordinate will be injected into the game. So in the future we can easily
        /// adapt a new input format
        /// </param>
        public Game(IBoard firstPlayerBoard, IBoard secondPlayerBoard, IUserInputToCoordinateConverter userInputConverter)
        {
            _boards = new IBoard[] {firstPlayerBoard, secondPlayerBoard};
            _userInputService = userInputConverter;
            _isFirstPlayerCurrentPlayer = true;
            IsGameOver = false;
        }

        public void PlaceShipInCurrentBoard(string shipInput)
        {
            var coords = _userInputService.ConvertUserInputToDoubleCoordinate(shipInput);
            if (coords == null) throw new ArgumentOutOfRangeException();
            _currentPlayerBoard.PlaceShip(coords.Item1, coords.Item2);
            switchPlayer();
        }

        public void PlayerPlayTurn(string hitPointInput)
        {
            var hitPoint = _userInputService.ConvertUserInputToCoordinate(hitPointInput);
            if (hitPoint == null) throw  new ArgumentOutOfRangeException();
            _opponentPlayerBoard.Hit(hitPoint);
            if (_opponentPlayerBoard.AllShipsSunked)
                IsGameOver = true;
            else
                switchPlayer();
        }

        public BoardCellStatus[,] GetPlayerBoard(bool isFirstPlayer)
        {
            var i = isFirstPlayer ? 0 : 1;
            return this._boards[i].Cells;
        }


        public string CurrentPlayerName
        {
            get { return _currentPlayerBoard.PlayerDispalyName; }
        }

        public string OppenentPlayerName
        {
            get
            {
                return _opponentPlayerBoard.PlayerDispalyName;
            }
        }

        

        private void switchPlayer()
        {
            _isFirstPlayerCurrentPlayer = !_isFirstPlayerCurrentPlayer;
        }

        private IBoard _currentPlayerBoard
        {
            get
            {
                return _isFirstPlayerCurrentPlayer ? _boards[0] : _boards[1];
            }
        }
        private IBoard _opponentPlayerBoard
        {
            get
            {
                return _isFirstPlayerCurrentPlayer ? _boards[1] : _boards[0];
            }
        }
        
    }
}