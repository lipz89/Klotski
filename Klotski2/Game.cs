using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Game;
using Game.Auto;

namespace Klotski
{
    partial class Game : UserControl
    {
        public Game()
        {
            InitializeComponent();
        }

        #region  私有字段

        public static int Length = 36;
        private readonly Dictionary<int, Piece> lps = new Dictionary<int, Piece>();
        private readonly List<int> lis = new List<int>();
        private readonly List<string> lstData = new List<string>();

        private int aw = 6;
        private int ah = 6;
        private int level = 0;
        private int lastX, lastY;

        private string levelmap;

        private Piece curP, tarP;

        #endregion

        #region 属性事件
        /// <summary>
        /// 当前游戏进行的步数
        /// </summary>
        [Browsable(false)]
        public int Step { get; private set; } = 0;

        /// <summary>
        /// 当前关卡的ID
        /// </summary>
        [Browsable(false)]
        public int Level
        {
            get { return level + 1; }
        }
        /// <summary>
        /// 当前关卡的难度
        /// </summary>
        [Browsable(false)]
        public int Diff
        {
            get { return level / 20 + 1; }
        }
        /// <summary>
        /// 关卡总数
        /// </summary>
        [Browsable(false)]
        public int LevelCount
        {
            get { return lstData.Count; }
        }
        /// <summary>
        /// 游戏完成时触发的事件
        /// </summary>
        public event EventHandler<FinishArgs> Finished;
        /// <summary>
        /// 游戏进行中每一次移动触发的事件
        /// </summary>
        public event EventHandler Moved;

        #endregion

        #region 开放的方法
        /// <summary>
        /// 初始化游戏，加载配置文件
        /// <param name="inStream">包含要加载的XML文件的流</param>
        /// </summary>
        public void InitLoad(Stream inStream)
        {
            StreamReader srData = new StreamReader(inStream);
            while (!srData.EndOfStream)
            {
                string d = srData.ReadLine();
                if (!string.IsNullOrEmpty(d))
                    lstData.Add(d);
            }
            srData.Close();
        }
        /// <summary>
        /// 加载指定关书的游戏
        /// </summary>
        /// <param name="levelId">关卡ID</param>
        public bool LoadLevel(int levelId)
        {
            if (levelId > 0 && levelId <= lstData.Count)
            {
                level = levelId - 1;
                return LoadLevel(lstData[level]);
            }
            return false;
        }
        /// <summary>
        /// 加载下一关游戏
        /// </summary>
        /// <returns></returns>
        public bool LoadNextLevel()
        {
            level++;
            return LoadLevel(level);
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 初始化游戏辅助变量
        /// </summary>
        private void InitGame()
        {
            this.Width = aw * Length + 24;
            this.Height = ah * Length + 24;
            Step = 0;
            lis.Clear();
            foreach (Piece p in lps.Values)
            {
                lis.AddRange(PlacesOfPiece(p));
            }
        }
        /// <summary>
        /// 返回指定的方块占据的位置
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private List<int> PlacesOfPiece(Piece p)
        {
            List<int> lst = new List<int>();
            for (int i = 0; i < p.H; i++)
            {
                for (int j = 0; j < p.W; j++)
                {
                    lst.Add((i + p.Y) * (aw + 1) + (j + p.X));
                }
            }
            return lst;
        }
        /// <summary>
        /// 根据配置文件内容片段读取关卡状态
        /// </summary>
        /// <param name="strData">关卡信息</param>
        /// <returns></returns>
        private bool LoadLevel(string strData)
        {
            try
            {
                this.levelmap = strData;
                lps.Clear();
                this.Controls.Clear();
                var infos = strData.Split(';');
                string[] bs = infos[2].Split(',');
                for (int i = 0; i < bs.Length; i++)
                {
                    //string id = ids[i].ToString();
                    Piece p = new Piece(bs[i], i);
                    if (p.Id == 0)
                    {
                        tarP = curP = p;
                    }
                    lps.Add(p.Id, p);
                    p.Left = p.X * Length + 12;
                    p.Top = p.Y * Length + 12;
                    //p.MouseEnter += p_Click;
                    p.OnMoveStarting += P_OnMoveStarting;
                    p.OnMoving += p_OnMove;
                    p.OnMoved += P_OnMoved;
                    this.Controls.Add(p);
                }
                InitGame();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void P_OnMoveStarting(object sender, EventArgs e)
        {
            curP = sender as Piece;
            ChangeCurPiece(curP);
            lastX = curP.X;
            lastY = curP.Y;
        }

        private void P_OnMoved(object sender, EventArgs e)
        {
            if (lastX != curP.X || lastY != curP.Y)
            {
                Step++;
                Moved?.Invoke(this, EventArgs.Empty);
                if (curP == tarP && tarP.X + tarP.W > aw)
                {
                    Finished?.Invoke(this, new FinishArgs(false));
                }
            }
        }

        void p_OnMove(object sender, BlockMoveArgs e)
        {
            Move(e.MoveDir);
        }

        /// <summary>
        /// 改变方块焦点
        /// </summary>
        /// <param name="p">获取焦点的方块</param>
        private void ChangeCurPiece(Piece p)
        {
            curP = p;
            p.Focus();
        }
        /// <summary>
        /// 方块移动
        /// </summary>
        private new void Move(MoveDir md)
        {
            switch (md)
            {
                case MoveDir.Left:
                    MoveLeft(curP);
                    break;
                case MoveDir.Up:
                    MoveUp(curP);
                    break;
                case MoveDir.Right:
                    MoveRight(curP);
                    break;
                case MoveDir.Down:
                    MoveDown(curP);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 方块左移
        /// </summary>
        /// <returns></returns>
        private void MoveLeft(Piece p)
        {
            int tmp = p.Y * (aw + 1) + p.X - 1;
            if (p.Dir != Direction.Vertical && p.X > 0 && !lis.Contains(tmp))
            {
                p.Left -= Length;
                p.X--;
                lis.Add(tmp);
                lis.Remove(tmp + p.W);
            }
        }
        /// <summary>
        /// 方块右移
        /// </summary>
        /// <returns></returns>
        private void MoveRight(Piece p)
        {
            int tmp = p.Y * (aw + 1) + p.X + p.W;
            if (p.Dir != Direction.Vertical && !lis.Contains(tmp))
            {
                if (p.X + p.W < aw || p == tarP)
                {
                    p.Left += Length;
                    p.X++;
                    lis.Add(tmp);
                    lis.Remove(tmp - p.W);
                }
            }
        }
        /// <summary>
        /// 方块上移
        /// </summary>
        /// <returns></returns>
        private void MoveUp(Piece p)
        {
            int tmp = (p.Y - 1) * (aw + 1) + p.X;
            if (p.Dir != Direction.Horizontal && p.Y > 0 && !lis.Contains(tmp))
            {
                p.Top -= Length;
                p.Y--;
                lis.Add(tmp);
                lis.Remove(tmp + p.H * (aw + 1));
            }
        }
        /// <summary>
        /// 方块下移
        /// </summary>
        /// <returns></returns>
        private void MoveDown(Piece p)
        {
            int tmp = (p.Y + p.H) * (aw + 1) + p.X;
            if (p.Dir != Direction.Horizontal && p.Y + p.H < aw && !lis.Contains(tmp))
            {
                p.Top += Length;
                p.Y++;
                lis.Add(tmp);
                lis.Remove(tmp - p.H * (aw + 1));
            }
        }

        #endregion

        #region 重写的方法

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (Enabled)
            {
                ChangeCurPiece(lps[0]);
            }
        }

        #endregion

        #region  自动求解

        /// <summary>
        /// 自动移动，如果已经发现解法
        /// </summary>
        private void AutoMove(object obj)
        {
            List<Step> lss = (List<Step>)obj;
            for (int i = 0; i < lss.Count; i++)
            {
                var stp = lss[i];
                for (int j = 0; j < stp.Stp; j++)
                {
                    switch (stp.Dir)
                    {
                        case MoveDir.Left:
                            MoveLeft(lps[stp.Id]);
                            break;
                        case MoveDir.Up:
                            MoveUp(lps[stp.Id]);
                            break;
                        case MoveDir.Right:
                            MoveRight(lps[stp.Id]);
                            break;
                        case MoveDir.Down:
                            MoveDown(lps[stp.Id]);
                            break;
                        default:
                            break;
                    }
                }
                Step++;
                Moved?.Invoke(this, EventArgs.Empty);

                if (i < lss.Count - 1)
                {
                    Thread.Sleep(200);
                }
            }
            if (curP.Id == tarP.Id && tarP.X + tarP.W >= aw)
            {
                Finished?.Invoke(this, new FinishArgs(true));
            }
            Thread.CurrentThread.Abort();
        }

        internal void FindBestAnswer()
        {
            var blocks = new List<Block>();
            foreach (var piece in lps.Values)
            {
                var block = new Block(piece.Id, piece.X, piece.Y, piece.Dir, piece.Dir == Direction.Horizontal ? piece.W : piece.H);
                blocks.Add(block);
            }
            var board = new AutoBoard(blocks, Door.Right(2), aw, ah);
            this.Enabled = false;
            var result = new Solve().Run(board);
            this.Enabled = true;
            if (result != null)
            {
                Thread th = new Thread(AutoMove);
                th.Start(result);
            }
        }

        #endregion
    }
}
