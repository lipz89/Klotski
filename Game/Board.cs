using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class Board
    {
        public Board(string map)
        {
            var infos = map.Split(';');
            var strXy = infos[0].Split('*');
            int w, h;
            if (int.TryParse(strXy[0], out w))
            {
                Width = w;
            }
            if (int.TryParse(strXy[1], out h))
            {
                Height = h;
            }

            this.Door = Door.New(infos[1]);
            this.Blocks = infos[2].Split(',').Select((x, i) => new Block(i, x)).ToList();
            this.Positions = this.Blocks.SelectMany(GetBlockPositions).ToList();
        }
        public Board(List<Block> blocks, Door door, int width, int height)
        {
            this.Blocks = blocks;
            this.Width = width;
            this.Height = height;
            this.Door = door;
            this.Positions = this.Blocks.SelectMany(GetBlockPositions).ToList();
        }
        //public Board LastBoard { get; private set; }
        //public Step LastStep { get; private set; }
        internal List<Block> Blocks { get; }
        internal List<int> Positions { get; private set; }
        public int Width { get; }
        public int Height { get; }
        public Door Door { get; }

        public Block this[int id]
        {
            get { return this.Blocks.FirstOrDefault(x => x.ID == id); }
        }

        private List<int> GetBlockPositions(Block block)
        {
            var position = block.Y * Width + block.X;
            var list = new List<int>() { position };
            if (block.Direction == Direction.Horizontal)
            {
                for (int i = 1; i < block.Length; i++)
                {
                    list.Add(position + i);
                }
            }
            else if (block.Direction == Direction.Vertical)
            {
                for (int i = 1; i < block.Length; i++)
                {
                    list.Add(position + Width * i);
                }
            }
            return list;
        }

        public bool Move(Step step)
        {
            var b = this[step.Id];
            int x = b.X, y = b.Y;
            int np;
            for (int i = 1; i <= step.Stp; i++)
            {
                switch (step.Dir)
                {
                    case MoveDir.Up:
                        y--;
                        np = y * Width + x;
                        break;
                    case MoveDir.Left:
                        x--;
                        np = y * Width + x;
                        break;
                    case MoveDir.Down:
                        y++;
                        np = (y + b.Length - 1) * Width + x;
                        break;
                    case MoveDir.Right:
                        x++;
                        np = y * Width + x + b.Length - 1;
                        break;
                    default:
                        return false;
                }

                if (x < 0 || y < 0)
                {
                    return false;
                }

                if (b.Direction == Direction.Horizontal && x + b.Length > Width)
                {
                    return false;
                }
                if (b.Direction == Direction.Vertical && y + b.Length > Height)
                {
                    return false;
                }

                if (this.Positions.Contains(np))
                {
                    return false;
                }
            }
            b.Move(x, y);
            this.Positions = this.Blocks.SelectMany(GetBlockPositions).ToList();
            return true;
        }

        public bool IsSuccess()
        {
            return this.Door.IsSuccess(this[0], Width, Height);
        }

        public override string ToString()
        {
            var strs = Blocks.OrderBy(x => x.ID).Select(x => x.ToString());
            return string.Join(",", strs);
        }
    }
}