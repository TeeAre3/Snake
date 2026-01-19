namespace Snake
{
    public readonly record struct Position(int Row, int Column)
    {
        public Position Translate(Direction dir)
        {
            return new Position(Row + dir.RowOffset, Column + dir.ColumnOffset); 
        }
    }
}
