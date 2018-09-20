using System.Collections.Generic;

namespace Klotski
{
    class Auto
    {
        /// <summary>
        /// 木块的集合
        /// </summary>
        List<P> lstTargets = new List<P>();
        /// <summary>
        /// 木块的位置，一维数字表示
        /// </summary>
        List<int> lstTargetPlaces = new List<int>();
        int aw = 6, ah = 6;
        /// <summary>
        /// 移动的动作集合，用于记录回溯
        /// </summary>
        Stack<Step> stkTrySteps = new Stack<Step>();
        /// <summary>
        /// 移动的动作集合，当找到解决方案后将记录回溯的集合复制到本集合，即为解。
        /// </summary>
        List<Step> lstFinallySteps = new List<Step>();
        /// <summary>
        /// 状态集合，用于记录某一状态在解题步骤中是否已经出现过
        /// </summary>
        Stack<string> historyStatus = new Stack<string>();

        public Auto(Dictionary<string, Piece> lps, int aw, int ah)
        {
            this.aw = aw;
            this.ah = ah;
            foreach (Piece piece in lps.Values)
            {
                for (int i = 0; i < piece.H; i++)
                {
                    for (int j = 0; j < piece.W; j++)
                    {
                        lstTargetPlaces.Add((i + piece.Y) * ah + (j + piece.X));
                    }
                }
                lstTargets.Add(new P(piece));
            }
        }
        #region 自动求解的方法（还在尝试当中）
        /// <summary>
        /// 自动求解
        /// </summary>
        public List<Step> FindBestAnswer()
        {
            string statu = GetStatu();
            historyStatus.Push(statu);
            TryMove();
            historyStatus.Pop();
            return lstFinallySteps;
        }
        /// <summary>
        /// 获取游戏全盘状态
        /// </summary>
        /// <returns></returns>
        private string GetStatu()
        {
            string s = string.Empty;
            foreach (P p in lstTargets)
            {
                s += p.Id + p.X + p.Y ;
            }
            return s;
        }

        /// <summary>
        /// 递归求解
        /// </summary>
        private void TryMove()
        {
            foreach (P p in lstTargets)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (p.Dir == Direction.Horizontal)
                    {
                        if (i == 0)
                        {
                            if (ALeft(p))
                            {
                                stkTrySteps.Push(new Step(p.Id, MDir.Left));
                                Check(p, i);
                                ARight(p);
                                stkTrySteps.Pop();
                            }
                        }
                        else if (i == 1)
                        {
                            if (ARight(p))
                            {
                                stkTrySteps.Push(new Step(p.Id, MDir.Right));
                                Check(p, i);
                                ALeft(p);
                                stkTrySteps.Pop();
                            }
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            if (AUp(p))
                            {
                                stkTrySteps.Push(new Step(p.Id, MDir.Up));
                                Check(p, i);
                                ADown(p);
                                stkTrySteps.Pop();
                            }
                        }
                        else if (i == 1)
                        {
                            if (ADown(p))
                            {
                                stkTrySteps.Push(new Step(p.Id, MDir.Down));
                                Check(p, i);
                                AUp(p);
                                stkTrySteps.Pop();
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 递归过程中检查目标方块是否到达出口位置
        /// </summary>
        /// <param name="p">移动的方块</param>
        /// <param name="i"></param>
        private void Check(P p, int i)
        {
            if (p.Id == "T" && p.X + p.W >= aw)
            {
                lstFinallySteps.Clear();
                lstFinallySteps.AddRange(stkTrySteps);
            }
            else
            {
                string statu = GetStatu();
                if (!historyStatus.Contains(statu))
                {
                    historyStatus.Push(statu);
                    TryMove();
                    //historyStatus.Pop();
                }
            }
        }

        /// <summary>
        /// 方块左移
        /// </summary>
        /// <param name="p">要移动的方块</param>
        /// <returns></returns>
        private bool ALeft(P p)
        {
            int tmp = p.Y * aw + p.X - 1;
            if (p.Dir != Direction.Vertical && p.X > 0 && !lstTargetPlaces.Contains(tmp))
            {
                p.X--;
                lstTargetPlaces.Add(tmp);
                lstTargetPlaces.Remove(tmp + p.W);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 方块右移
        /// </summary>
        /// <param name="p">要移动的方块</param>
        /// <returns></returns>
        private bool ARight(P p)
        {
            int tmp = p.Y * aw + p.X + p.W;
            if (p.Dir != Direction.Vertical && p.X + p.W < aw && !lstTargetPlaces.Contains(tmp))
            {
                p.X++;
                lstTargetPlaces.Add(tmp);
                lstTargetPlaces.Remove(tmp - p.W);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 方块上移
        /// </summary>
        /// <param name="p">要移动的方块</param>
        /// <returns></returns>
        private bool AUp(P p)
        {
            int tmp = (p.Y - 1) * aw + p.X;
            if (p.Dir != Direction.Horizontal && p.Y > 0 && !lstTargetPlaces.Contains(tmp))
            {
                p.Y--;
                lstTargetPlaces.Add(tmp);
                lstTargetPlaces.Remove(tmp + p.H * aw);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 方块下移
        /// </summary>
        /// <param name="p">要移动的方块</param>
        /// <returns></returns>
        private bool ADown(P p)
        {
            int tmp = (p.Y + p.H) * aw + p.X;
            if (p.Dir != Direction.Horizontal && p.Y + p.H < aw && !lstTargetPlaces.Contains(tmp))
            {
                p.Y++;
                lstTargetPlaces.Add(tmp);
                lstTargetPlaces.Remove(tmp - p.H * aw);
                return true;
            }
            return false;
        }

        #endregion

        class P
        {
            public P(Piece piece)
            {
                this.Id = piece.Id;
                this.X = piece.X;
                this.Y = piece.Y;
                this.W = piece.W;
                this.H = piece.H;
                this.Dir = piece.Dir;
            }
            public Direction Dir;
            public string Id;
            public int X;
            public int Y;
            public int W;
            public int H;
        }
    }
}
