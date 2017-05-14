using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Battleship.Model
{
    /// <summary>
    /// This is responsible of converting user input of pattern A3 into Coordinate: column 0, row 2
    /// </summary>
    public class AlphaNumericUserInputToCoordinateConverter : IUserInputToCoordinateConverter
    {
        private const string CoordinateInputPattern = @"^(?<alpha>[A-Z])(?<numeric>\d+)$";  // similar to :  "A10"
        private const string DoubleCoordinateInputPattern = @"^(?<firstPoint>[A-z]\d+)\s+(?<secondPoint>[A-z]\d+)$"; // double input pattern is : "A1  A5"
        const int LowerColumn = (int) 'A'; // the ascii of A

        public Coordinate ConvertUserInputToCoordinate(string userInput)
        {
            Regex rgx = new Regex(CoordinateInputPattern, RegexOptions.IgnoreCase);
            Match m = rgx.Match(userInput.Trim().ToUpper());
            if (m.Groups.Count != 3)
                return null;
            int row = int.Parse(m.Groups["numeric"].Value);
            int column = Encoding.ASCII.GetBytes(m.Groups["alpha"].Value)[0];
            var coord = new Coordinate(row - 1, column - LowerColumn);
            return coord;
        }

        public Tuple<Coordinate, Coordinate> ConvertUserInputToDoubleCoordinate(string userInput)
        {
            Regex rgx = new Regex(DoubleCoordinateInputPattern, RegexOptions.IgnoreCase);
            Match m = rgx.Match(userInput.Trim().ToUpper());
            if (m.Groups.Count != 3)
                return null;
            string firstEntry = m.Groups["firstPoint"].Value;
            string secondEntry = m.Groups["secondPoint"].Value;
            Coordinate firstCoord = ConvertUserInputToCoordinate(firstEntry);
            Coordinate secondCoord = ConvertUserInputToCoordinate(secondEntry);
            return new Tuple<Coordinate, Coordinate>(firstCoord, secondCoord);
        }
    }
}