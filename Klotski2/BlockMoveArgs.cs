using System;
using Game;

namespace Klotski
{
    class BlockMoveArgs : EventArgs
    {
        public readonly MoveDir MoveDir;

        public BlockMoveArgs(MoveDir moveDir)
        {
            this.MoveDir = moveDir;
        }
    }
}