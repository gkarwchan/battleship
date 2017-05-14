using System;

namespace Battleship.Model
{
    public class Coordinate
    {
        private int _row, _column;

        public int Row
        {
            get { return _row; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Row");
                _row = value;
            }
        }

        public int Column
        {
            get { return _column; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Column");
                _column = value;
            }
        }

        public Coordinate(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Coordinate;
            if (item == null) return false;
            return this.Column == item.Column && this.Row == item.Row;
        }
    }
}