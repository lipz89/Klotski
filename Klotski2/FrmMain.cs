using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Klotski
{
    partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon;
            CheckForIllegalCrossThreadCalls = false;
            this.Size = new Size(310, 336);
            tmr.Interval = 1000;
            tmr.AutoReset = true;
            tmr.Elapsed += tmr_Elapsed;
            this.button1.Left = -100;
            InitGame();

            userFile = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\klotski.sav";
        }

        private void pnlLevels_MouseEnter(object sender, EventArgs e)
        {
            pnlLevels.Focus();
        }

        private Game game;
        private System.Timers.Timer tmr = new System.Timers.Timer();
        private int timeCount = 0;
        private readonly string userFile = "klotski.sav";
        private readonly List<LvInfo> LstLv = new List<LvInfo>();
        private int curLv = 1, maxCanLv;
        private readonly List<Label> lbls = new List<Label>();

        private void InitGame()
        {
            this.game = new Game();
            this.Controls.Add(game);
            Assembly asm = Assembly.GetEntryAssembly();
            Stream xs = asm.GetManifestResourceStream("Klotski.maps.txt");

            this.game.InitLoad(xs);

            this.game.Location = new Point(27, 45);
            this.game.Name = "game";
            this.game.Size = new Size(240, 240);
            this.game.TabIndex = 7;
            this.game.Finished += this.game_Finished;
            this.game.Moved += this.game_Moved;

            for (int i = 0; i < game.LevelCount; i++)
            {
                LstLv.Add(new LvInfo(i + 1));
                lbls.Add(new Label());
            }
            game.Enabled = false;
        }

        void tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timeCount++;
            lblTime.Text = string.Format("用时：{0:00}:{1:00}", timeCount / 60, timeCount % 60);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            maxCanLv = curLv = GetUserInfo();
            AddLevels();
            ScrollPnlLevels(maxCanLv);
        }

        private void ScrollPnlLevels(int lv)
        {
            if (lv < 1 || lv > 100)
                return;
            int m = pnlLevels.VerticalScroll.Maximum * lv / 100;
            m = Math.Max(m, pnlLevels.Height / 2);
            pnlLevels.VerticalScroll.Value = m - pnlLevels.Height / 2;
        }

        private void Init()
        {
            game.Enabled = true;
            lblStep.Text = "第0步";
            lblTime.Text = "用时：00:00";
            lblDiff.Text = "难度：" + game.Diff;
            lblLevel.Text = "第" + game.Level + "关";
            timeCount = 0;
            tmr.Start();
        }

        private void game_Finished(object sender, FinishArgs e)
        {
            tmr.Stop();
            if (!e.IsAuto)
            {
                LvInfo li = LstLv[curLv - 1];
                if (li.Empty || li.Step > game.Step)
                {
                    MessageBox.Show(this, "找到了更优解。");
                    li.SetInfo(game.Step, timeCount, DateTime.Now);
                    lbls[curLv - 1].Text = li.ToLabelString();
                    lbls[curLv - 1].ForeColor = Color.Black;
                }

                if (lbls.Count > curLv)
                {
                    MessageBox.Show(this, "恭喜通过关卡，点击确定进入下一关。");
                    if (!lbls[curLv].Enabled)
                    {
                        lbls[curLv].Enabled = true;
                        if (curLv > maxCanLv)
                        {
                            maxCanLv = curLv;
                        }
                    }

                    curLv++;
                }
                else
                {
                    MessageBox.Show(this, "恭喜,你已经全部通关。");
                }

                SaveUserInfo();

                game.LoadLevel(curLv);
                Init();
                pnlLevels.Visible = false;
                panel1.Visible = panel2.Visible = false;
            }
        }

        private void game_Moved(object sender, EventArgs e)
        {
            lblStep.Text = "第" + game.Step + "步";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            game.FindBestAnswer();
        }

        private int GetUserInfo()
        {
            int level = 1;
            StreamReader sr = new StreamReader(File.Open(userFile, FileMode.OpenOrCreate));
            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                if (!string.IsNullOrEmpty(s))
                {
                    LvInfo li = new LvInfo(s);
                    LstLv[li.LvId - 1] = li;
                    level = li.LvId;
                }
            }
            sr.Close();
            return level;
        }

        private void AddLevels()
        {
            bool canDo = true;
            int h = 18, w = 18;
            for (int i = 0; i < game.LevelCount; i++)
            {
                LvInfo lv = LstLv[i];
                Label lbl = lbls[i];
                lbl.BorderStyle = BorderStyle.FixedSingle;
                lbl.Location = new Point(w, h);
                h += 24;
                lbl.Size = new Size(240, 16);
                lbl.TextAlign = ContentAlignment.MiddleLeft;
                lbl.Tag = i + 1;
                lbl.Click += lbl_Click;
                lbl.Text = lv.ToLabelString();
                lbl.Cursor = Cursors.Hand;

                if (!lv.Empty)
                {
                    canDo = true;
                }
                else
                {
                    lbl.ForeColor = Color.Red;
                    if (canDo)
                    {
                        canDo = false;
                    }
                    else
                    {
                        lbl.Cursor = Cursors.Arrow;
                        lbl.Enabled = false;
                    }
                }
                this.pnlLevels.Controls.Add(lbl);
                lbls[i] = lbl;
            }
            Label lbd = new Label();
            lbd.Text = " ";
            lbd.Location = new Point(w, h - 12);
            this.pnlLevels.Controls.Add(lbd);
        }

        void lbl_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null)
            {
                curLv = Convert.ToInt32(lbl.Tag);
                game.LoadLevel(curLv);
                Init();
                pnlLevels.Visible = false;
                panel1.Visible = panel2.Visible = false;
            }
        }

        private void SaveUserInfo()
        {
            StreamWriter sr = new StreamWriter(File.Open(userFile, FileMode.Create));
            foreach (LvInfo lv in LstLv)
            {
                if (!string.IsNullOrEmpty(lv.ToString()))
                {
                    sr.WriteLine(lv.ToString());
                }
            }
            sr.Close();
        }

        private void lblLevel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pnlLevels.Visible = true;
            panel1.Visible = panel2.Visible = true;
            game.Enabled = false;
            ScrollPnlLevels(maxCanLv);
        }

        private class LvInfo
        {
            public LvInfo(int lvid)
            {
                this.LvId = lvid;
            }

            public void SetInfo(int step, int tm, DateTime dt)
            {
                this.Step = step;
                this.Tm = tm.ToString();
                this.Dt = dt.ToString("yyyy-MM-dd HH:mm");
                Empty = false;
            }

            public LvInfo(string lii)
            {
                string[] ss = lii.Split(',');
                this.LvId = int.Parse(ss[0]);
                this.Step = int.Parse(ss[1]);
                this.Tm = ss[2];
                this.Dt = ss[3];
                Empty = false;
            }

            public bool Empty { get; private set; } = true;

            public int LvId { get; }

            public string Dt { get; private set; }

            public int Step { get; private set; } = 0;

            public string Tm { get; private set; }

            public override string ToString()
            {
                if (Empty)
                    return string.Empty;

                return string.Format("{0},{1},{2},{3}", LvId, Step, Tm, Dt);
            }

            public string ToLabelString()
            {
                if (Empty)
                    return string.Format("Lv:{0}  未挑战", LvId.ToString().PadLeft(3));

                return string.Format("Lv:{0} Sp:{1} Tm:{2}  {3}", LvId.ToString().PadLeft(3), Step.ToString().PadLeft(3), Tm.PadLeft(3), Dt);
            }
        }
    }
}
