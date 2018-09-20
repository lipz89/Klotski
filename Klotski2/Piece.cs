using System;
using System.Drawing;
using System.Windows.Forms;
using Game;

namespace Klotski
{
    partial class Piece : UserControl
    {
        private Piece() { }

        /// <summary>
        /// 游戏移动的方块对象
        /// </summary>
        /// <param name="bkData">方块信息</param>
        /// <param name="id">方块的ID</param>
        public Piece(string bkData, int id)
        {
            InitializeComponent();

            X = bkData[1] - 48;
            Y = bkData[2] - 48;
            H = 1;
            W = 1;
            this.Id = id;
            var type = bkData[0];
            if (type == '0')
            {
                Dir = Direction.Horizontal;
                W = 2;
                img = Properties.Resources.rect1;
            }
            else if (type == '1')
            {
                Dir = Direction.Horizontal;
                W = 3;
                img = Properties.Resources.rect2;
            }
            else if (type == '2')
            {
                Dir = Direction.Vertical;
                H = 2;
                img = Properties.Resources.rect3;
            }
            else if (type == '3')
            {
                Dir = Direction.Vertical;
                H = 3;
                img = Properties.Resources.rect4;
            }

            if (this.Id == 0)
            {
                img = Properties.Resources.rect0;
            }

            this.Width = this.W * Game.Length;
            this.Height = this.H * Game.Length;
        }

        #region 字段

        private Image img;

        private Point downPoint;
        private bool isDown;

        #endregion

        #region 属性
        /// <summary>
        /// 方块的ID
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 宽度
        /// </summary>
        public int W { get; }

        /// <summary>
        /// 高度
        /// </summary>
        public int H { get; }

        /// <summary>
        /// 方块相对游戏区域左上角的横坐标
        /// </summary>
        public int X { get; internal set; }

        /// <summary>
        /// 方块相对游戏区域左上角的纵坐标
        /// </summary>
        public int Y { get; internal set; }

        /// <summary>
        /// 方块允许移动的方向限制
        /// </summary>
        public Direction Dir { get; }

        internal event EventHandler<BlockMoveArgs> OnMoving;
        internal event EventHandler<EventArgs> OnMoved;
        internal event EventHandler<EventArgs> OnMoveStarting;

        #endregion

        #region 方法
        /// <summary>
        /// 重写绘制方法，主要绘制3D效果的边框，和方块的ID
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(img, 0, 0, this.Width, this.Height);
            if (this.Focused)
            {
                if (Dir == Direction.Horizontal)
                {
                    e.Graphics.DrawImage(Properties.Resources.left0, 0, 0, this.Height, this.Height);
                    e.Graphics.DrawImage(Properties.Resources.right0, this.Width - this.Height, 0, this.Height, this.Height);
                }
                else
                {
                    e.Graphics.DrawImage(Properties.Resources.up0, 0, 0, this.Width, this.Width);
                    e.Graphics.DrawImage(Properties.Resources.down0, 0, this.Height - this.Width, this.Width, this.Width);
                }
            }
            //e.Graphics.DrawString(ID, this.Font, new SolidBrush(this.ForeColor), Width * Game.Length / 2 - 5, Height * Game.Length / 2 - 6);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            downPoint = e.Location;
            isDown = true;
            OnMoveStarting?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isDown)
            {
                if (Dir == Direction.Horizontal)
                {
                    if (e.X - downPoint.X > this.Height)
                        OnMoving?.Invoke(this, new BlockMoveArgs(MoveDir.Right));
                    else if (downPoint.X - e.X > this.Height)
                        OnMoving?.Invoke(this, new BlockMoveArgs(MoveDir.Left));
                }
                else
                {
                    if (e.Y - downPoint.Y > this.Width)
                        OnMoving?.Invoke(this, new BlockMoveArgs(MoveDir.Down));
                    else if (downPoint.Y - e.Y > this.Width)
                        OnMoving?.Invoke(this, new BlockMoveArgs(MoveDir.Up));
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (isDown)
            {
                OnMoved?.Invoke(this, EventArgs.Empty);
            }
            isDown = false;
        }

        #endregion
    }
}
