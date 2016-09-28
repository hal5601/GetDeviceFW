namespace GetDeviceFW
{
    partial class Config
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
            this.button_OK = new System.Windows.Forms.Button();
            this.label_VID = new System.Windows.Forms.Label();
            this.textBox_VID = new System.Windows.Forms.TextBox();
            this.textBox_PID = new System.Windows.Forms.TextBox();
            this.label_PID = new System.Windows.Forms.Label();
            this.textBox_TargetFW = new System.Windows.Forms.TextBox();
            this.label_FW = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(140, 126);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 13;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // label_VID
            // 
            this.label_VID.AutoSize = true;
            this.label_VID.Location = new System.Drawing.Point(34, 27);
            this.label_VID.Name = "label_VID";
            this.label_VID.Size = new System.Drawing.Size(29, 12);
            this.label_VID.TabIndex = 14;
            this.label_VID.Text = "VID:";
            // 
            // textBox_VID
            // 
            this.textBox_VID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_VID.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_VID.Location = new System.Drawing.Point(115, 24);
            this.textBox_VID.Name = "textBox_VID";
            this.textBox_VID.Size = new System.Drawing.Size(100, 21);
            this.textBox_VID.TabIndex = 15;
            // 
            // textBox_PID
            // 
            this.textBox_PID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_PID.Location = new System.Drawing.Point(115, 51);
            this.textBox_PID.Name = "textBox_PID";
            this.textBox_PID.Size = new System.Drawing.Size(100, 21);
            this.textBox_PID.TabIndex = 17;
            // 
            // label_PID
            // 
            this.label_PID.AutoSize = true;
            this.label_PID.Location = new System.Drawing.Point(34, 54);
            this.label_PID.Name = "label_PID";
            this.label_PID.Size = new System.Drawing.Size(29, 12);
            this.label_PID.TabIndex = 16;
            this.label_PID.Text = "PID:";
            // 
            // textBox_TargetFW
            // 
            this.textBox_TargetFW.Location = new System.Drawing.Point(115, 78);
            this.textBox_TargetFW.Name = "textBox_TargetFW";
            this.textBox_TargetFW.Size = new System.Drawing.Size(100, 21);
            this.textBox_TargetFW.TabIndex = 19;
            // 
            // label_FW
            // 
            this.label_FW.AutoSize = true;
            this.label_FW.Location = new System.Drawing.Point(34, 81);
            this.label_FW.Name = "label_FW";
            this.label_FW.Size = new System.Drawing.Size(65, 12);
            this.label_FW.TabIndex = 18;
            this.label_FW.Text = "Target FW:";
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 170);
            this.Controls.Add(this.textBox_TargetFW);
            this.Controls.Add(this.label_FW);
            this.Controls.Add(this.textBox_PID);
            this.Controls.Add(this.label_PID);
            this.Controls.Add(this.textBox_VID);
            this.Controls.Add(this.label_VID);
            this.Controls.Add(this.button_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Config";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Config";
            this.Load += new System.EventHandler(this.Config_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Label label_VID;
        private System.Windows.Forms.TextBox textBox_VID;
        private System.Windows.Forms.TextBox textBox_PID;
        private System.Windows.Forms.Label label_PID;
        private System.Windows.Forms.TextBox textBox_TargetFW;
        private System.Windows.Forms.Label label_FW;

    }
}