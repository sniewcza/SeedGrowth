namespace SeedGrowth
{
    partial class SetupView
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
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.BCcomboBox = new System.Windows.Forms.ComboBox();
            this.widthInputBox = new System.Windows.Forms.MaskedTextBox();
            this.heightInputBox = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.seedAmountInputBox = new System.Windows.Forms.MaskedTextBox();
            this.NHcomboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SDcomboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.xAxisSeedsInputBox = new System.Windows.Forms.MaskedTextBox();
            this.yAxisSeedsInputBox = new System.Windows.Forms.MaskedTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radiusInputBox = new System.Windows.Forms.MaskedTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.nextStepButton = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.inclusionMaxRadiusInputBox = new System.Windows.Forms.MaskedTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.inclusionMinRadiusInputBox = new System.Windows.Forms.MaskedTextBox();
            this.inclusionNumberInputBox = new System.Windows.Forms.MaskedTextBox();
            this.actionsGroupBox = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.actionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(112, 32);
            this.startButton.Margin = new System.Windows.Forms.Padding(4);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(100, 28);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(220, 32);
            this.stopButton.Margin = new System.Windows.Forms.Padding(4);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(100, 28);
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.button6_Click);
            // 
            // BCcomboBox
            // 
            this.BCcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BCcomboBox.FormattingEnabled = true;
            this.BCcomboBox.Location = new System.Drawing.Point(340, 69);
            this.BCcomboBox.Margin = new System.Windows.Forms.Padding(4);
            this.BCcomboBox.Name = "BCcomboBox";
            this.BCcomboBox.Size = new System.Drawing.Size(160, 24);
            this.BCcomboBox.TabIndex = 7;
            // 
            // widthInputBox
            // 
            this.widthInputBox.Location = new System.Drawing.Point(32, 29);
            this.widthInputBox.Margin = new System.Windows.Forms.Padding(4);
            this.widthInputBox.Mask = "0000";
            this.widthInputBox.Name = "widthInputBox";
            this.widthInputBox.Size = new System.Drawing.Size(132, 22);
            this.widthInputBox.TabIndex = 8;
            // 
            // heightInputBox
            // 
            this.heightInputBox.Location = new System.Drawing.Point(32, 69);
            this.heightInputBox.Margin = new System.Windows.Forms.Padding(4);
            this.heightInputBox.Mask = "0000";
            this.heightInputBox.Name = "heightInputBox";
            this.heightInputBox.Size = new System.Drawing.Size(132, 22);
            this.heightInputBox.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 72);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Y";
            // 
            // seedAmountInputBox
            // 
            this.seedAmountInputBox.Location = new System.Drawing.Point(114, 167);
            this.seedAmountInputBox.Margin = new System.Windows.Forms.Padding(4);
            this.seedAmountInputBox.Mask = "00000";
            this.seedAmountInputBox.Name = "seedAmountInputBox";
            this.seedAmountInputBox.Size = new System.Drawing.Size(132, 22);
            this.seedAmountInputBox.TabIndex = 14;
            this.seedAmountInputBox.ValidatingType = typeof(int);
            // 
            // NHcomboBox
            // 
            this.NHcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NHcomboBox.FormattingEnabled = true;
            this.NHcomboBox.Location = new System.Drawing.Point(368, 29);
            this.NHcomboBox.Margin = new System.Windows.Forms.Padding(4);
            this.NHcomboBox.Name = "NHcomboBox";
            this.NHcomboBox.Size = new System.Drawing.Size(132, 24);
            this.NHcomboBox.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(254, 34);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "Neighbourhood";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(195, 72);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 17);
            this.label5.TabIndex = 18;
            this.label5.Text = "Boundary conditions";
            // 
            // SDcomboBox
            // 
            this.SDcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SDcomboBox.FormattingEnabled = true;
            this.SDcomboBox.Location = new System.Drawing.Point(91, 116);
            this.SDcomboBox.Margin = new System.Windows.Forms.Padding(4);
            this.SDcomboBox.Name = "SDcomboBox";
            this.SDcomboBox.Size = new System.Drawing.Size(160, 24);
            this.SDcomboBox.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 119);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 17);
            this.label6.TabIndex = 20;
            this.label6.Text = "Seed draw";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 170);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 17);
            this.label7.TabIndex = 21;
            this.label7.Text = "Seeds amount";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(328, 32);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(100, 28);
            this.saveButton.TabIndex = 22;
            this.saveButton.Text = "Save BMP";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // xAxisSeedsInputBox
            // 
            this.xAxisSeedsInputBox.Location = new System.Drawing.Point(17, 42);
            this.xAxisSeedsInputBox.Margin = new System.Windows.Forms.Padding(4);
            this.xAxisSeedsInputBox.Mask = "0000";
            this.xAxisSeedsInputBox.Name = "xAxisSeedsInputBox";
            this.xAxisSeedsInputBox.Size = new System.Drawing.Size(132, 22);
            this.xAxisSeedsInputBox.TabIndex = 23;
            this.xAxisSeedsInputBox.ValidatingType = typeof(int);
            // 
            // yAxisSeedsInputBox
            // 
            this.yAxisSeedsInputBox.Location = new System.Drawing.Point(173, 42);
            this.yAxisSeedsInputBox.Margin = new System.Windows.Forms.Padding(4);
            this.yAxisSeedsInputBox.Mask = "0000";
            this.yAxisSeedsInputBox.Name = "yAxisSeedsInputBox";
            this.yAxisSeedsInputBox.Size = new System.Drawing.Size(132, 22);
            this.yAxisSeedsInputBox.TabIndex = 24;
            this.yAxisSeedsInputBox.ValidatingType = typeof(int);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.yAxisSeedsInputBox);
            this.groupBox1.Controls.Add(this.xAxisSeedsInputBox);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(7, 205);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(333, 80);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(194, 19);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 17);
            this.label8.TabIndex = 26;
            this.label8.Text = "Y axis seeds";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 17);
            this.label3.TabIndex = 25;
            this.label3.Text = "X axis seeds";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radiusInputBox);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(7, 293);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(267, 48);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            // 
            // radiusInputBox
            // 
            this.radiusInputBox.Location = new System.Drawing.Point(83, 16);
            this.radiusInputBox.Margin = new System.Windows.Forms.Padding(4);
            this.radiusInputBox.Mask = "000";
            this.radiusInputBox.Name = "radiusInputBox";
            this.radiusInputBox.Size = new System.Drawing.Size(132, 22);
            this.radiusInputBox.TabIndex = 1;
            this.radiusInputBox.ValidatingType = typeof(int);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 20);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 17);
            this.label9.TabIndex = 0;
            this.label9.Text = "Radius";
            // 
            // nextStepButton
            // 
            this.nextStepButton.Location = new System.Drawing.Point(5, 32);
            this.nextStepButton.Name = "nextStepButton";
            this.nextStepButton.Size = new System.Drawing.Size(100, 28);
            this.nextStepButton.TabIndex = 27;
            this.nextStepButton.Text = "Next step";
            this.nextStepButton.UseVisualStyleBackColor = true;
            this.nextStepButton.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(6, 361);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 28);
            this.button4.TabIndex = 28;
            this.button4.Text = "Initialize";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.widthInputBox);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.SDcomboBox);
            this.groupBox3.Controls.Add(this.heightInputBox);
            this.groupBox3.Controls.Add(this.NHcomboBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.BCcomboBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.seedAmountInputBox);
            this.groupBox3.Location = new System.Drawing.Point(12, 15);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(655, 408);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Setup";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.inclusionMaxRadiusInputBox);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.inclusionMinRadiusInputBox);
            this.groupBox4.Controls.Add(this.inclusionNumberInputBox);
            this.groupBox4.Location = new System.Drawing.Point(347, 116);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(302, 153);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Inclusions";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 105);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 17);
            this.label12.TabIndex = 5;
            this.label12.Text = "max radius";
            // 
            // inclusionMaxRadiusInputBox
            // 
            this.inclusionMaxRadiusInputBox.Location = new System.Drawing.Point(97, 102);
            this.inclusionMaxRadiusInputBox.Mask = "00000";
            this.inclusionMaxRadiusInputBox.Name = "inclusionMaxRadiusInputBox";
            this.inclusionMaxRadiusInputBox.Size = new System.Drawing.Size(100, 22);
            this.inclusionMaxRadiusInputBox.TabIndex = 4;
            this.inclusionMaxRadiusInputBox.ValidatingType = typeof(int);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 66);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(73, 17);
            this.label11.TabIndex = 3;
            this.label11.Text = "min radius";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 32);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 17);
            this.label10.TabIndex = 2;
            this.label10.Text = "amount";
            // 
            // inclusionMinRadiusInputBox
            // 
            this.inclusionMinRadiusInputBox.Location = new System.Drawing.Point(97, 66);
            this.inclusionMinRadiusInputBox.Mask = "00000";
            this.inclusionMinRadiusInputBox.Name = "inclusionMinRadiusInputBox";
            this.inclusionMinRadiusInputBox.Size = new System.Drawing.Size(100, 22);
            this.inclusionMinRadiusInputBox.TabIndex = 1;
            this.inclusionMinRadiusInputBox.ValidatingType = typeof(int);
            // 
            // inclusionNumberInputBox
            // 
            this.inclusionNumberInputBox.Location = new System.Drawing.Point(79, 32);
            this.inclusionNumberInputBox.Mask = "00000";
            this.inclusionNumberInputBox.Name = "inclusionNumberInputBox";
            this.inclusionNumberInputBox.Size = new System.Drawing.Size(100, 22);
            this.inclusionNumberInputBox.TabIndex = 0;
            this.inclusionNumberInputBox.ValidatingType = typeof(int);
            // 
            // actionsGroupBox
            // 
            this.actionsGroupBox.Controls.Add(this.button1);
            this.actionsGroupBox.Controls.Add(this.stopButton);
            this.actionsGroupBox.Controls.Add(this.startButton);
            this.actionsGroupBox.Controls.Add(this.saveButton);
            this.actionsGroupBox.Controls.Add(this.nextStepButton);
            this.actionsGroupBox.Enabled = false;
            this.actionsGroupBox.Location = new System.Drawing.Point(13, 429);
            this.actionsGroupBox.Name = "actionsGroupBox";
            this.actionsGroupBox.Size = new System.Drawing.Size(569, 84);
            this.actionsGroupBox.TabIndex = 30;
            this.actionsGroupBox.TabStop = false;
            this.actionsGroupBox.Text = "Actions";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(435, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 28;
            this.button1.Text = "Load MBP";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // SetupView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(679, 619);
            this.Controls.Add(this.actionsGroupBox);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "SetupView";
            this.Text = "Seed growth simulator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.actionsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox BCcomboBox;
        private System.Windows.Forms.MaskedTextBox widthInputBox;
        private System.Windows.Forms.MaskedTextBox heightInputBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox seedAmountInputBox;
        private System.Windows.Forms.ComboBox NHcomboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox SDcomboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.MaskedTextBox xAxisSeedsInputBox;
        private System.Windows.Forms.MaskedTextBox yAxisSeedsInputBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MaskedTextBox radiusInputBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button nextStepButton;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox actionsGroupBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.MaskedTextBox inclusionMinRadiusInputBox;
        private System.Windows.Forms.MaskedTextBox inclusionNumberInputBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.MaskedTextBox inclusionMaxRadiusInputBox;
        private System.Windows.Forms.Button button1;
    }
}

