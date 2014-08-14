namespace Luffarschacksprojekt
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
                  this.components = new System.ComponentModel.Container();
                  this.menuStrip1 = new System.Windows.Forms.MenuStrip();
                  this.inställningarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.spelare1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.typToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.aIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.monteCarloToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.originalgangsterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.människaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.tidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.oToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.spelare2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.aIToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
                  this.aIToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
                  this.monteCarloToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
                  this.originalgangsterToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
                  this.människaToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
                  this.timer1 = new System.Windows.Forms.Timer(this.components);
                  this.timer2 = new System.Windows.Forms.Timer(this.components);
                  this.menuStrip1.SuspendLayout();
                  this.SuspendLayout();
                  // 
                  // menuStrip1
                  // 
                  this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inställningarToolStripMenuItem});
                  this.menuStrip1.Location = new System.Drawing.Point(0, 0);
                  this.menuStrip1.Name = "menuStrip1";
                  this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
                  this.menuStrip1.Size = new System.Drawing.Size(500, 24);
                  this.menuStrip1.TabIndex = 0;
                  this.menuStrip1.Text = "menuStrip1";
                  // 
                  // inställningarToolStripMenuItem
                  // 
                  this.inställningarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spelare1ToolStripMenuItem,
            this.spelare2ToolStripMenuItem});
                  this.inställningarToolStripMenuItem.Name = "inställningarToolStripMenuItem";
                  this.inställningarToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
                  this.inställningarToolStripMenuItem.Text = "Inställningar";
                  // 
                  // spelare1ToolStripMenuItem
                  // 
                  this.spelare1ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.typToolStripMenuItem,
            this.tidToolStripMenuItem});
                  this.spelare1ToolStripMenuItem.Name = "spelare1ToolStripMenuItem";
                  this.spelare1ToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
                  this.spelare1ToolStripMenuItem.Text = "Spelare 1";
                  // 
                  // typToolStripMenuItem
                  // 
                  this.typToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aIToolStripMenuItem,
            this.människaToolStripMenuItem});
                  this.typToolStripMenuItem.Name = "typToolStripMenuItem";
                  this.typToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
                  this.typToolStripMenuItem.Text = "Typ";
                  // 
                  // aIToolStripMenuItem
                  // 
                  this.aIToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monteCarloToolStripMenuItem,
            this.originalgangsterToolStripMenuItem});
                  this.aIToolStripMenuItem.Name = "aIToolStripMenuItem";
                  this.aIToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
                  this.aIToolStripMenuItem.Text = "AI";
                  // 
                  // monteCarloToolStripMenuItem
                  // 
                  this.monteCarloToolStripMenuItem.Name = "monteCarloToolStripMenuItem";
                  this.monteCarloToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
                  this.monteCarloToolStripMenuItem.Text = "Monte Carlo";
                  this.monteCarloToolStripMenuItem.Click += new System.EventHandler(this.monteCarloToolStripMenuItem1_Click);
                  // 
                  // originalgangsterToolStripMenuItem
                  // 
                  this.originalgangsterToolStripMenuItem.Name = "originalgangsterToolStripMenuItem";
                  this.originalgangsterToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
                  this.originalgangsterToolStripMenuItem.Text = "Originalgangster";
                  this.originalgangsterToolStripMenuItem.Click += new System.EventHandler(this.originalgangsterToolStripMenuItem1_Click);
                  // 
                  // människaToolStripMenuItem
                  // 
                  this.människaToolStripMenuItem.Name = "människaToolStripMenuItem";
                  this.människaToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
                  this.människaToolStripMenuItem.Text = "Människa";
                  this.människaToolStripMenuItem.Click += new System.EventHandler(this.människaToolStripMenuItem_Click);
                  // 
                  // tidToolStripMenuItem
                  // 
                  this.tidToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oToolStripMenuItem,
            this.xToolStripMenuItem});
                  this.tidToolStripMenuItem.Name = "tidToolStripMenuItem";
                  this.tidToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
                  this.tidToolStripMenuItem.Text = "Tecken";
                  // 
                  // oToolStripMenuItem
                  // 
                  this.oToolStripMenuItem.Name = "oToolStripMenuItem";
                  this.oToolStripMenuItem.Size = new System.Drawing.Size(83, 22);
                  this.oToolStripMenuItem.Text = "O";
                  this.oToolStripMenuItem.Click += new System.EventHandler(this.oToolStripMenuItem_Click);
                  // 
                  // xToolStripMenuItem
                  // 
                  this.xToolStripMenuItem.Name = "xToolStripMenuItem";
                  this.xToolStripMenuItem.Size = new System.Drawing.Size(83, 22);
                  this.xToolStripMenuItem.Text = "X";
                  this.xToolStripMenuItem.Click += new System.EventHandler(this.xToolStripMenuItem_Click);
                  // 
                  // spelare2ToolStripMenuItem
                  // 
                  this.spelare2ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aIToolStripMenuItem1});
                  this.spelare2ToolStripMenuItem.Name = "spelare2ToolStripMenuItem";
                  this.spelare2ToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
                  this.spelare2ToolStripMenuItem.Text = "Spelare 2";
                  // 
                  // aIToolStripMenuItem1
                  // 
                  this.aIToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aIToolStripMenuItem2,
            this.människaToolStripMenuItem2});
                  this.aIToolStripMenuItem1.Name = "aIToolStripMenuItem1";
                  this.aIToolStripMenuItem1.Size = new System.Drawing.Size(94, 22);
                  this.aIToolStripMenuItem1.Text = "Typ";
                  // 
                  // aIToolStripMenuItem2
                  // 
                  this.aIToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monteCarloToolStripMenuItem1,
            this.originalgangsterToolStripMenuItem1});
                  this.aIToolStripMenuItem2.Name = "aIToolStripMenuItem2";
                  this.aIToolStripMenuItem2.Size = new System.Drawing.Size(125, 22);
                  this.aIToolStripMenuItem2.Text = "AI";
                  // 
                  // monteCarloToolStripMenuItem1
                  // 
                  this.monteCarloToolStripMenuItem1.Name = "monteCarloToolStripMenuItem1";
                  this.monteCarloToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
                  this.monteCarloToolStripMenuItem1.Text = "Monte Carlo";
                  this.monteCarloToolStripMenuItem1.Click += new System.EventHandler(this.monteCarloToolStripMenuItem2_Click);
                  // 
                  // originalgangsterToolStripMenuItem1
                  // 
                  this.originalgangsterToolStripMenuItem1.Name = "originalgangsterToolStripMenuItem1";
                  this.originalgangsterToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
                  this.originalgangsterToolStripMenuItem1.Text = "Originalgangster";
                  this.originalgangsterToolStripMenuItem1.Click += new System.EventHandler(this.originalgangsterToolStripMenuItem2_Click);
                  // 
                  // människaToolStripMenuItem2
                  // 
                  this.människaToolStripMenuItem2.Name = "människaToolStripMenuItem2";
                  this.människaToolStripMenuItem2.Size = new System.Drawing.Size(125, 22);
                  this.människaToolStripMenuItem2.Text = "Människa";
                  this.människaToolStripMenuItem2.Click += new System.EventHandler(this.människaToolStripMenuItem2_Click);
                  // 
                  // timer1
                  // 
                  this.timer1.Enabled = true;
                  this.timer1.Interval = 10;
                  // 
                  // timer2
                  // 
                  this.timer2.Enabled = true;
                  this.timer2.Interval = 10;
                  // 
                  // Form1
                  // 
                  this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                  this.ClientSize = new System.Drawing.Size(500, 480);
                  this.Controls.Add(this.menuStrip1);
                  this.DoubleBuffered = true;
                  this.KeyPreview = true;
                  this.MainMenuStrip = this.menuStrip1;
                  this.Name = "Form1";
                  this.ShowIcon = false;
                  this.Text = "Luffarschack";
                  this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
                  this.menuStrip1.ResumeLayout(false);
                  this.menuStrip1.PerformLayout();
                  this.ResumeLayout(false);
                  this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem inställningarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spelare1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spelare2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem typToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem människaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tidToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aIToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aIToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem människaToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem oToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monteCarloToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem originalgangsterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monteCarloToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem originalgangsterToolStripMenuItem1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;

    }
}

