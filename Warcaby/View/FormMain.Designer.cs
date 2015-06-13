namespace Warcaby
{
    partial class FormMain
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panelBoardBackground = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.nowaGraToolStripMenuItemNewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.cofnijToolStripMenuItemBack = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemEndTurn = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // panelBoardBackground
            // 
            this.panelBoardBackground.BackgroundImage = global::Warcaby.Properties.Resources.chessboard;
            this.panelBoardBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBoardBackground.Location = new System.Drawing.Point(0, 27);
            this.panelBoardBackground.Name = "panelBoardBackground";
            this.panelBoardBackground.Size = new System.Drawing.Size(600, 600);
            this.panelBoardBackground.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nowaGraToolStripMenuItemNewGame,
            this.cofnijToolStripMenuItemBack,
            this.ToolStripMenuItemEndTurn});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(600, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // nowaGraToolStripMenuItemNewGame
            // 
            this.nowaGraToolStripMenuItemNewGame.Name = "nowaGraToolStripMenuItemNewGame";
            this.nowaGraToolStripMenuItemNewGame.Size = new System.Drawing.Size(71, 20);
            this.nowaGraToolStripMenuItemNewGame.Text = "Nowa Gra";
            this.nowaGraToolStripMenuItemNewGame.Click += new System.EventHandler(this.nowaGraToolStripMenuItemNewGame_Click);
            // 
            // cofnijToolStripMenuItemBack
            // 
            this.cofnijToolStripMenuItemBack.Name = "cofnijToolStripMenuItemBack";
            this.cofnijToolStripMenuItemBack.Size = new System.Drawing.Size(51, 20);
            this.cofnijToolStripMenuItemBack.Text = "Cofnij";
            this.cofnijToolStripMenuItemBack.Click += new System.EventHandler(this.cofnijToolStripMenuItemBack_Click);
            // 
            // ToolStripMenuItemEndTurn
            // 
            this.ToolStripMenuItemEndTurn.Name = "ToolStripMenuItemEndTurn";
            this.ToolStripMenuItemEndTurn.Size = new System.Drawing.Size(222, 20);
            this.ToolStripMenuItemEndTurn.Text = "Zakończ ruch pomimo kolejnego bicia";
            this.ToolStripMenuItemEndTurn.Visible = false;
            this.ToolStripMenuItemEndTurn.Click += new System.EventHandler(this.ToolStripMenuItemEndTurn_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(600, 627);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panelBoardBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "Warcaby";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Panel panelBoardBackground;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem nowaGraToolStripMenuItemNewGame;
        private System.Windows.Forms.ToolStripMenuItem cofnijToolStripMenuItemBack;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemEndTurn;
    }
}

