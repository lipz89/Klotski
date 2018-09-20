using System.Collections.Generic;
using System.Linq;

namespace Game.Auto
{
    public class AutoBoard : Board
    {
        public AutoBoard(string map) : base(map)
        {
        }
        public AutoBoard(List<Block> blocks, Door door, int w, int h) : base(blocks, door, w, h)
        {
        }

        private AutoBoard LastBoard { get; set; }
        private Step LastStep { get; set; }


        //public List<Step> GetNextSteps()
        //{
        //    var steps = new List<Step>();
        //    foreach (var block in blocks)
        //    {
        //        if (block.Direction == Direction.Horizontal)
        //        {
        //            for (int i = Width - block.X - block.Length; i > 0; i--)
        //            {
        //                steps.Add(new Step(block.ID, MoveDir.Right, i));
        //            }
        //            for (int i = block.X; i > 0; i--)
        //            {
        //                steps.Add(new Step(block.ID, MoveDir.Left, i));
        //            }
        //        }
        //        else
        //        {
        //            for (int i = Height - block.Y - block.Length; i > 0; i--)
        //            {
        //                steps.Add(new Step(block.ID, MoveDir.Down, i));
        //            }
        //            for (int i = block.Y; i > 0; i--)
        //            {
        //                steps.Add(new Step(block.ID, MoveDir.Up, i));
        //            }
        //        }
        //    }
        //    return steps;
        //}
        internal List<Step> GetNextSteps()
        {
            var steps = new List<Step>();
            foreach (var block in Blocks)
            {
                if (block.Direction == Direction.Horizontal)
                {
                    for (int i = 1; i <= Width - block.X - block.Length; i++)
                    {
                        steps.Add(new Step(block.ID, MoveDir.Right, i));
                    }
                    for (int i = 1; i <= block.X; i++)
                    {
                        steps.Add(new Step(block.ID, MoveDir.Left, i));
                    }
                }
                else
                {
                    for (int i = 1; i <= Height - block.Y - block.Length; i++)
                    {
                        steps.Add(new Step(block.ID, MoveDir.Down, i));
                    }
                    for (int i = 1; i <= block.Y; i++)
                    {
                        steps.Add(new Step(block.ID, MoveDir.Up, i));
                    }
                }
            }
            return steps;
        }

        internal bool TryMove(Step step)
        {
            if (base.Move(step))
            {
                this.LastStep = step;
                return true;
            }

            return false;
        }

        internal AutoBoard Copy()
        {
            var newBlocks = this.Blocks.Select(x => x.Copy()).ToList();
            return new AutoBoard(newBlocks, Door, Width, Height) { LastBoard = this };
        }

        internal List<Step> GetSteps()
        {
            var list = new List<Step>();
            var game = this;
            while (game?.LastStep != null)
            {
                list.Insert(0, game.LastStep);
                game = game.LastBoard;
            }
            return list;
        }
    }
}