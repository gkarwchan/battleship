namespace Battleship.Model
{
    public class GameFactory
    {
        public static Game CreateGame(int rowSize, int colSize, string firstPlayerName, string secondPlayerName)
        {
            var firstPlayerBoard = new Board(firstPlayerName, rowSize, colSize);
            var secondPlayerBoard = new Board(secondPlayerName, rowSize, colSize);

            var game = new Game(firstPlayerBoard, secondPlayerBoard, new AlphaNumericUserInputToCoordinateConverter());
            return game;
        }
    }
}