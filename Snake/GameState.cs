using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class GameState
    {
        public int Rows { get; }
        public int Columns { get; }
        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }
        public GameMode Mode { get; private set; }

        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();
        public readonly LinkedList<Position> snakePositions = new LinkedList<Position>();
        private readonly Random random = new Random();

        public GameState(int rows, int columns, GameMode mode)
        {
            Rows = rows;
            Columns = columns;
            Mode = mode;
            Grid = new GridValue[rows, columns];
            Dir = Direction.Right;

            SpawnSnake();
            SpawnFood();
        }

        private void SpawnSnake()
        {
            int r = Rows / 2;
            for (int c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.Snake;
                snakePositions.AddFirst(new Position(r, c));
            }
        }

        private IEnumerable<Position> EmptyPositions()
        {
            for(int r = 0; r < Rows; r++)
            {
                for(int c = 0; c < Columns; c++)
                {
                    if (Grid[r,c] == GridValue.Empty)
                    {
                        yield return new Position(r, c);
                    }
                }
            }
        }

        private void SpawnFood()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if(empty.Count == 0) return;

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Column] = GridValue.Food;
        }

        public Position HeadPosition()
        {
            return snakePositions.First.Value;
        }

        public Position TailPosition()
        {
            return snakePositions.Last.Value;
        }

        public IEnumerable<Position> SnakePositions()
        {
            return snakePositions;
        }

        private void SpawnHead(Position pos)
        {
            snakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Column] = GridValue.Snake;
        }

        private void DespawnTail()
        {
            Position tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Column] = GridValue.Empty;
            snakePositions.RemoveLast();
        }

        private Direction GetLastDirection()
        {
            if (dirChanges.Count == 0) return Dir;

            return dirChanges.Last.Value;
        }

        private bool CanChangeDirection(Direction newDir)
        {
            if (dirChanges.Count == 2) return false;

            Direction lastDir = GetLastDirection();
            return newDir != lastDir && newDir != lastDir.Opposite();
        }

        public void ChangeDirection(Direction direction)
        {
            if(CanChangeDirection(direction))
            {
                dirChanges.AddLast(direction);
            }
        }

        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Column < 0 || pos.Column >= Columns;
        }

        private GridValue WillHit(Position newHeadPos)
        {
            if(OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            if(newHeadPos == TailPosition())
            {
                return GridValue.Empty; 
            }

            return Grid[newHeadPos.Row, newHeadPos.Column];
        }

        public void Move()
        {
            if(dirChanges.Count > 0)
            {
                Dir = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }

            Position head = HeadPosition();
            Position newHeadPos = HeadPosition().Translate(Dir);

            if(Mode == GameMode.WallsWrap)
            {
                int r = newHeadPos.Row;
                int c = newHeadPos.Column;

                if (r < 0) r = Rows - 1;
                else if (r >= Rows)  r = 0;
                if (c < 0) c = Columns - 1;
                else if (c >= Columns) c = 0;

                newHeadPos = new Position(r, c);
            }

            GridValue hit = WillHit(newHeadPos);
            

            if (hit == GridValue.Outside || hit == GridValue.Snake)
            {
                GameOver = true;
            }
            else if (hit == GridValue.Empty)
            {
                DespawnTail();
                SpawnHead(newHeadPos);
            }
            else if(hit == GridValue.Food)
            {
                SpawnHead(newHeadPos);
                Score += 10;
                SpawnFood();
            }
        }
    }
}
