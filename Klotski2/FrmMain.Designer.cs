namespace Klotski
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.lblDiff = new System.Windows.Forms.Label();
            this.lblStep = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.pnlLevels = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblLevel = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblDiff
            // 
            this.lblDiff.AutoSize = true;
            this.lblDiff.Location = new System.Drawing.Point(76, 13);
            this.lblDiff.Name = "lblDiff";
            this.lblDiff.Size = new System.Drawing.Size(41, 12);
            this.lblDiff.TabIndex = 2;
            this.lblDiff.Text = "label2";
            // 
            // lblStep
            // 
            this.lblStep.AutoSize = true;
            this.lblStep.Location = new System.Drawing.Point(149, 13);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(41, 12);
            this.lblStep.TabIndex = 3;
            this.lblStep.Text = "label3";
            // 
            // lblTime
            // 
            this.lblTime.Location = new System.Drawing.Point(197, 13);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(86, 12);
            this.lblTime.TabIndex = 4;
            this.lblTime.Text = "label4";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pnlLevels
            // 
            this.pnlLevels.AutoScroll = true;
            this.pnlLevels.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlLevels.Location = new System.Drawing.Point(280, 0);
            this.pnlLevels.Name = "pnlLevels";
            this.pnlLevels.Size = new System.Drawing.Size(294, 262);
            this.pnlLevels.TabIndex = 5;
            this.pnlLevels.MouseEnter += new System.EventHandler(this.pnlLevels_MouseEnter);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(13, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 2);
            this.label1.TabIndex = 6;
            this.label1.Text = "label1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(-5, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "自动(&A)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(11, 13);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(65, 12);
            this.lblLevel.TabIndex = 9;
            this.lblLevel.TabStop = true;
            this.lblLevel.Text = "linkLabel1";
            this.lblLevel.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblLevel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblLevel_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(274, 18);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.Location = new System.Drawing.Point(0, 244);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(274, 18);
            this.panel2.TabIndex = 11;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 262);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlLevels);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.lblDiff);
            this.Controls.Add(this.lblStep);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "经典挪砖块";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDiff;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Panel pnlLevels;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.LinkLabel lblLevel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;

    }
}

