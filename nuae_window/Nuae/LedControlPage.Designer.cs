namespace Nuae
{
    partial class LedControlPage
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
            this.ledDataSendButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.blue_brightness_text_box = new System.Windows.Forms.TextBox();
            this.IDComboBox = new System.Windows.Forms.ComboBox();
            this.blue_radioButton = new System.Windows.Forms.RadioButton();
            this.red_radioButton = new System.Windows.Forms.RadioButton();
            this.uv_radioButton = new System.Windows.Forms.RadioButton();
            this.red_brightness_text_box = new System.Windows.Forms.TextBox();
            this.uv_brightness_text_box = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ledDataSendButton
            // 
            this.ledDataSendButton.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ledDataSendButton.Location = new System.Drawing.Point(287, 259);
            this.ledDataSendButton.Name = "ledDataSendButton";
            this.ledDataSendButton.Size = new System.Drawing.Size(121, 65);
            this.ledDataSendButton.TabIndex = 0;
            this.ledDataSendButton.Text = "전송";
            this.ledDataSendButton.UseVisualStyleBackColor = true;
            this.ledDataSendButton.Click += new System.EventHandler(this.ledDataSendButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(116, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "LED 색깔 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(116, 191);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "LED 밝기 :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(116, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "ID (제어기 번호) :";
            this.label3.Visible = false;
            // 
            // blue_brightness_text_box
            // 
            this.blue_brightness_text_box.Location = new System.Drawing.Point(287, 195);
            this.blue_brightness_text_box.Name = "blue_brightness_text_box";
            this.blue_brightness_text_box.Size = new System.Drawing.Size(60, 21);
            this.blue_brightness_text_box.TabIndex = 6;
            // 
            // IDComboBox
            // 
            this.IDComboBox.FormattingEnabled = true;
            this.IDComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.IDComboBox.Location = new System.Drawing.Point(287, 85);
            this.IDComboBox.Name = "IDComboBox";
            this.IDComboBox.Size = new System.Drawing.Size(121, 20);
            this.IDComboBox.TabIndex = 7;
            this.IDComboBox.Visible = false;
            this.IDComboBox.SelectedIndexChanged += new System.EventHandler(this.IDComboBox_SelectedIndexChanged);
            // 
            // blue_radioButton
            // 
            this.blue_radioButton.AutoSize = true;
            this.blue_radioButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.blue_radioButton.Location = new System.Drawing.Point(287, 136);
            this.blue_radioButton.Name = "blue_radioButton";
            this.blue_radioButton.Size = new System.Drawing.Size(47, 16);
            this.blue_radioButton.TabIndex = 9;
            this.blue_radioButton.TabStop = true;
            this.blue_radioButton.Text = "파랑";
            this.blue_radioButton.UseVisualStyleBackColor = true;
            this.blue_radioButton.CheckedChanged += new System.EventHandler(this.blue_radioButton_CheckedChanged);
            // 
            // red_radioButton
            // 
            this.red_radioButton.AutoSize = true;
            this.red_radioButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.red_radioButton.Location = new System.Drawing.Point(414, 136);
            this.red_radioButton.Name = "red_radioButton";
            this.red_radioButton.Size = new System.Drawing.Size(47, 16);
            this.red_radioButton.TabIndex = 10;
            this.red_radioButton.TabStop = true;
            this.red_radioButton.Text = "빨강";
            this.red_radioButton.UseVisualStyleBackColor = true;
            this.red_radioButton.CheckedChanged += new System.EventHandler(this.red_radioButton_CheckedChanged);
            // 
            // uv_radioButton
            // 
            this.uv_radioButton.AutoSize = true;
            this.uv_radioButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.uv_radioButton.Location = new System.Drawing.Point(541, 136);
            this.uv_radioButton.Name = "uv_radioButton";
            this.uv_radioButton.Size = new System.Drawing.Size(39, 16);
            this.uv_radioButton.TabIndex = 11;
            this.uv_radioButton.TabStop = true;
            this.uv_radioButton.Text = "UV";
            this.uv_radioButton.UseVisualStyleBackColor = true;
            this.uv_radioButton.CheckedChanged += new System.EventHandler(this.uv_radioButton_CheckedChanged);
            // 
            // red_brightness_text_box
            // 
            this.red_brightness_text_box.Location = new System.Drawing.Point(414, 195);
            this.red_brightness_text_box.Name = "red_brightness_text_box";
            this.red_brightness_text_box.Size = new System.Drawing.Size(60, 21);
            this.red_brightness_text_box.TabIndex = 12;
            // 
            // uv_brightness_text_box
            // 
            this.uv_brightness_text_box.Location = new System.Drawing.Point(541, 195);
            this.uv_brightness_text_box.Name = "uv_brightness_text_box";
            this.uv_brightness_text_box.Size = new System.Drawing.Size(60, 21);
            this.uv_brightness_text_box.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(668, 198);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "(0~100 사이의 값 입력)";
            // 
            // LedControlPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(1036, 767);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.uv_brightness_text_box);
            this.Controls.Add(this.red_brightness_text_box);
            this.Controls.Add(this.uv_radioButton);
            this.Controls.Add(this.red_radioButton);
            this.Controls.Add(this.blue_radioButton);
            this.Controls.Add(this.IDComboBox);
            this.Controls.Add(this.blue_brightness_text_box);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ledDataSendButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LedControlPage";
            this.Text = "LedOnOffPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ledDataSendButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox blue_brightness_text_box;
        private System.Windows.Forms.ComboBox IDComboBox;
        private System.Windows.Forms.RadioButton blue_radioButton;
        private System.Windows.Forms.RadioButton red_radioButton;
        private System.Windows.Forms.RadioButton uv_radioButton;
        private System.Windows.Forms.TextBox red_brightness_text_box;
        private System.Windows.Forms.TextBox uv_brightness_text_box;
        private System.Windows.Forms.Label label4;
    }
}