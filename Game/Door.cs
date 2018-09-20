using System;

namespace Game
{
    public class Door
    {
        public MoveDir Dir { get; }
        public int X { get; }
        public int Y { get; }
        private Door(MoveDir dir, int x, int y)
        {
            Dir = dir;
            X = x;
            Y = y;
        }

        internal bool IsSuccess(Block target, int width, int height)
        {
            if (this.Dir == MoveDir.Left)
            {
                return target.Y == this.Y && target.X == 0;
            }

            if (this.Dir == MoveDir.Right)
            {
                return target.Y == this.Y && target.X + target.Length == width;
            }
            if (this.Dir == MoveDir.Up)
            {
                return target.X == this.X && target.Y == 0;
            }

            if (this.Dir == MoveDir.Down)
            {
                return target.X == this.X && target.Y + target.Length == height;
            }

            return false;
        }

        public static Door Left(int y)
        {
            return new Door(MoveDir.Left, 0, y);
        }

        public static Door Right(int y)
        {
            return new Door(MoveDir.Right, 0, y);
        }

        public static Door Down(int x)
        {
            return new Door(MoveDir.Down, x, 0);
        }

        public static Door Up(int x)
        {
            return new Door(MoveDir.Up, x, 0);
        }
        public static Door New(string door)
        {
            var i = door[1] - 48;
            if (door.StartsWith("u", StringComparison.OrdinalIgnoreCase))
            {
                return new Door(MoveDir.Up, i, 0);
            }
            if (door.StartsWith("l", StringComparison.OrdinalIgnoreCase))
            {
                return new Door(MoveDir.Left, 0, i);
            }
            if (door.StartsWith("d", StringComparison.OrdinalIgnoreCase))
            {
                return new Door(MoveDir.Down, i, 0);
            }
            if (door.StartsWith("r", StringComparison.OrdinalIgnoreCase))
            {
                return new Door(MoveDir.Right, 0, i);
            }
            return null;
        }
    }
}