namespace Game
{
    public class Block
    {
        public int ID { get; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction Direction { get; }
        public int Length { get; }

        public Block(int id, string map)
        {
            ID = id;
            X = map[1] - 48;
            Y = map[2] - 48;
            var type = map[0];
            if (type == '0' || type == '1')
            {
                Direction = Direction.Horizontal;
            }
            else if (type == '2' || type == '3')
            {
                Direction = Direction.Vertical;
            }
            if (type == '0' || type == '2')
            {
                Length = 2;
            }
            else if (type == '1' || type == '3')
            {
                Length = 3;
            }
        }

        public Block(int id, int x, int y, Direction direction, int length)
        {
            ID = id;
            X = x;
            Y = y;
            Direction = direction;
            Length = length;
        }

        public override string ToString()
        {
            return $"{ID}-{X}{Y}";
        }

        internal void Move(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        internal Block Copy()
        {
            return new Block(this.ID, this.X, this.Y, this.Direction, this.Length);
        }
    }
}