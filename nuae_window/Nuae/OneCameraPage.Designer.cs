namespace Nuae
{
    partial class OneCameraPage
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
            this.SoloShotExitButton = new System.Windows.Forms.Button();
            this.SoloShotBackGroundPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SoloShotVideoPlayer = new AForge.Controls.VideoSourcePlayer();
            this.SoloShotBackGroundPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // SoloShotExitButton
            // 
            this.SoloShotExitButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.SoloShotExitButton.Location = new System.Drawing.Point(958, 3);
            this.SoloShotExitButton.Name = "SoloShotExitButton";
            this.SoloShotExitButton.Size = new System.Drawing.Size(75, 40);
            this.SoloShotExitButton.TabIndex = 0;
            this.SoloShotExitButton.Text = "닫기";
            this.SoloShotExitButton.UseVisualStyleBackColor = true;
            this.SoloShotExitButton.Click += new System.EventHandler(this.SoloShotExitButton_Click);
            // 
            // SoloShotBackGroundPanel
            // 
            this.SoloShotBackGroundPanel.ColumnCount = 1;
            this.SoloShotBackGroundPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SoloShotBackGroundPanel.Controls.Add(this.SoloShotExitButton, 0, 0);
            this.SoloShotBackGroundPanel.Controls.Add(this.SoloShotVideoPlayer, 0, 1);
            this.SoloShotBackGroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SoloShotBackGroundPanel.Location = new System.Drawing.Point(0, 0);
            this.SoloShotBackGroundPanel.Name = "SoloShotBackGroundPanel";
            this.SoloShotBackGroundPanel.RowCount = 2;
            this.SoloShotBackGroundPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.122449F));
            this.SoloShotBackGroundPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.87755F));
            this.SoloShotBackGroundPanel.Size = new System.Drawing.Size(1036, 767);
            this.SoloShotBackGroundPanel.TabIndex = 1;
            // 
            // SoloShotVideoPlayer
            // 
            this.SoloShotVideoPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SoloShotVideoPlayer.Location = new System.Drawing.Point(3, 49);
            this.SoloShotVideoPlayer.Name = "SoloShotVideoPlayer";
            this.SoloShotVideoPlayer.Size = new System.Drawing.Size(1030, 715);
            this.SoloShotVideoPlayer.TabIndex = 1;
            this.SoloShotVideoPlayer.Text = "videoSourcePlayer1";
            this.SoloShotVideoPlayer.VideoSource = null;
            // 
            // OneCameraPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(1036, 767);
            this.Controls.Add(this.SoloShotBackGroundPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OneCameraPage";
            this.Text = "OneCameraPage";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OneCameraPage_FormClosing);
            this.Load += new System.EventHandler(this.OneCameraPage_Load);
            this.SoloShotBackGroundPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SoloShotExitButton;
        private System.Windows.Forms.TableLayoutPanel SoloShotBackGroundPanel;
        public AForge.Controls.VideoSourcePlayer SoloShotVideoPlayer;
    }
}