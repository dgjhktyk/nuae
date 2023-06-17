namespace Nuae
{
    partial class SettingPage
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.size_confrim_button = new System.Windows.Forms.Button();
            this.settin_location_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.save_location_label = new System.Windows.Forms.Label();
            this.size_limit_input_text_box = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(83, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "용량 제한 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(309, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "GB";
            // 
            // size_confrim_button
            // 
            this.size_confrim_button.Location = new System.Drawing.Point(203, 151);
            this.size_confrim_button.Name = "size_confrim_button";
            this.size_confrim_button.Size = new System.Drawing.Size(100, 30);
            this.size_confrim_button.TabIndex = 3;
            this.size_confrim_button.Text = "용량 제한 설정";
            this.size_confrim_button.UseVisualStyleBackColor = true;
            this.size_confrim_button.Click += new System.EventHandler(this.size_confrim_button_Click);
            // 
            // settin_location_button
            // 
            this.settin_location_button.Location = new System.Drawing.Point(203, 284);
            this.settin_location_button.Name = "settin_location_button";
            this.settin_location_button.Size = new System.Drawing.Size(100, 30);
            this.settin_location_button.TabIndex = 5;
            this.settin_location_button.Text = "저장 위치 설정";
            this.settin_location_button.UseVisualStyleBackColor = true;
            this.settin_location_button.Click += new System.EventHandler(this.setting_location_button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(83, 251);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "저장 위치 :";
            // 
            // save_location_label
            // 
            this.save_location_label.AutoSize = true;
            this.save_location_label.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.save_location_label.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.save_location_label.Location = new System.Drawing.Point(204, 251);
            this.save_location_label.Name = "save_location_label";
            this.save_location_label.Size = new System.Drawing.Size(0, 19);
            this.save_location_label.TabIndex = 9;
            // 
            // size_limit_input_text_box
            // 
            this.size_limit_input_text_box.Location = new System.Drawing.Point(204, 113);
            this.size_limit_input_text_box.Name = "size_limit_input_text_box";
            this.size_limit_input_text_box.Size = new System.Drawing.Size(99, 21);
            this.size_limit_input_text_box.TabIndex = 10;
            // 
            // SettingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(1036, 767);
            this.Controls.Add(this.size_limit_input_text_box);
            this.Controls.Add(this.save_location_label);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.settin_location_button);
            this.Controls.Add(this.size_confrim_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingPage";
            this.Text = "SettingPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button size_confrim_button;
        private System.Windows.Forms.Button settin_location_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label save_location_label;
        private System.Windows.Forms.TextBox size_limit_input_text_box;
    }
}