namespace Nuae
{
    partial class MainPage
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuPanel = new System.Windows.Forms.Panel();
            this.sensorPageButton = new System.Windows.Forms.Button();
            this.LedPageButton = new System.Windows.Forms.Button();
            this.cameraPageButton = new System.Windows.Forms.Button();
            this.portConPageButton = new System.Windows.Forms.Button();
            this.saveTimer = new System.Windows.Forms.Timer(this.components);
            this.led_data_timer = new System.Windows.Forms.Timer(this.components);
            this.contentPanel = new System.Windows.Forms.Panel();
            this.sensor_req_timer = new System.Windows.Forms.Timer(this.components);
            this.settingsButton = new System.Windows.Forms.Button();
            this.menuPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuPanel
            // 
            this.menuPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.menuPanel.Controls.Add(this.settingsButton);
            this.menuPanel.Controls.Add(this.sensorPageButton);
            this.menuPanel.Controls.Add(this.LedPageButton);
            this.menuPanel.Controls.Add(this.cameraPageButton);
            this.menuPanel.Controls.Add(this.portConPageButton);
            this.menuPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.menuPanel.Location = new System.Drawing.Point(0, 0);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Size = new System.Drawing.Size(160, 767);
            this.menuPanel.TabIndex = 1;
            // 
            // sensorPageButton
            // 
            this.sensorPageButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.sensorPageButton.FlatAppearance.BorderSize = 0;
            this.sensorPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sensorPageButton.ForeColor = System.Drawing.SystemColors.Control;
            this.sensorPageButton.Location = new System.Drawing.Point(0, 189);
            this.sensorPageButton.Name = "sensorPageButton";
            this.sensorPageButton.Size = new System.Drawing.Size(160, 63);
            this.sensorPageButton.TabIndex = 1;
            this.sensorPageButton.Text = "센서 정보";
            this.sensorPageButton.UseVisualStyleBackColor = true;
            this.sensorPageButton.Click += new System.EventHandler(this.sensorPageButton_Click);
            // 
            // LedPageButton
            // 
            this.LedPageButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.LedPageButton.FlatAppearance.BorderSize = 0;
            this.LedPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LedPageButton.ForeColor = System.Drawing.SystemColors.Control;
            this.LedPageButton.Location = new System.Drawing.Point(0, 126);
            this.LedPageButton.Name = "LedPageButton";
            this.LedPageButton.Size = new System.Drawing.Size(160, 63);
            this.LedPageButton.TabIndex = 2;
            this.LedPageButton.Text = "LED On Off";
            this.LedPageButton.UseVisualStyleBackColor = true;
            this.LedPageButton.Click += new System.EventHandler(this.LedPageButton_Click);
            // 
            // cameraPageButton
            // 
            this.cameraPageButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.cameraPageButton.FlatAppearance.BorderSize = 0;
            this.cameraPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cameraPageButton.ForeColor = System.Drawing.SystemColors.Control;
            this.cameraPageButton.Location = new System.Drawing.Point(0, 63);
            this.cameraPageButton.Name = "cameraPageButton";
            this.cameraPageButton.Size = new System.Drawing.Size(160, 63);
            this.cameraPageButton.TabIndex = 0;
            this.cameraPageButton.Text = "카메라";
            this.cameraPageButton.UseVisualStyleBackColor = true;
            this.cameraPageButton.Click += new System.EventHandler(this.cameraPageButton_Click);
            // 
            // portConPageButton
            // 
            this.portConPageButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.portConPageButton.FlatAppearance.BorderSize = 0;
            this.portConPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.portConPageButton.ForeColor = System.Drawing.SystemColors.Control;
            this.portConPageButton.Location = new System.Drawing.Point(0, 0);
            this.portConPageButton.Name = "portConPageButton";
            this.portConPageButton.Size = new System.Drawing.Size(160, 63);
            this.portConPageButton.TabIndex = 1;
            this.portConPageButton.Text = "포트 연결";
            this.portConPageButton.UseVisualStyleBackColor = true;
            this.portConPageButton.Click += new System.EventHandler(this.portConPageButton_Click);
            // 
            // saveTimer
            // 
            this.saveTimer.Interval = 1200000;
            this.saveTimer.Tick += new System.EventHandler(this.saveTimer_Tick);
            // 
            // led_data_timer
            // 
            this.led_data_timer.Interval = 3000;
            this.led_data_timer.Tick += new System.EventHandler(this.led_data_timer_Tick);
            // 
            // contentPanel
            // 
            this.contentPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(160, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(1072, 767);
            this.contentPanel.TabIndex = 2;
            // 
            // sensor_req_timer
            // 
            this.sensor_req_timer.Interval = 5000;
            this.sensor_req_timer.Tick += new System.EventHandler(this.sensor_req_timer_tick);
            // 
            // settingsButton
            // 
            this.settingsButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.settingsButton.FlatAppearance.BorderSize = 0;
            this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsButton.ForeColor = System.Drawing.SystemColors.Control;
            this.settingsButton.Location = new System.Drawing.Point(0, 252);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(160, 63);
            this.settingsButton.TabIndex = 4;
            this.settingsButton.Text = "설정";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 767);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.menuPanel);
            this.MinimumSize = new System.Drawing.Size(1248, 806);
            this.Name = "MainPage";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainPage_FormClosing);
            this.menuPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel menuPanel;
        private System.Windows.Forms.Button sensorPageButton;
        private System.Windows.Forms.Button portConPageButton;
        public System.Windows.Forms.Timer saveTimer;
        private System.Windows.Forms.Button LedPageButton;
        private System.Windows.Forms.Button cameraPageButton;
        public System.Windows.Forms.Timer led_data_timer;
        public System.Windows.Forms.Panel contentPanel;
        public System.Windows.Forms.Timer sensor_req_timer;
        private System.Windows.Forms.Button settingsButton;
    }
}

