
namespace Snake
{
    public record Direction(int RowOffset, int ColumnOffset)
    {
        public readonly static Direction Left = new (0, -1);
        public readonly static Direction Right = new (0, 1);
        public readonly static Direction Up = new (-1, 0);
        public readonly static Direction Down = new (1, 0);

        public Direction Opposite()
        {
            return new Direction(-RowOffset, -ColumnOffset);
        }

    }
}
