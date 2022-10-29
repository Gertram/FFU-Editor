namespace DLFontViewer
{
    partial class AddSymForm
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
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.LeftSizeTextBox = new System.Windows.Forms.TextBox();
            this.RightCutSize = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.Color0 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Color15 = new System.Windows.Forms.Button();
            this.Color7 = new System.Windows.Forms.Button();
            this.Color6 = new System.Windows.Forms.Button();
            this.Color5 = new System.Windows.Forms.Button();
            this.Color3 = new System.Windows.Forms.Button();
            this.Color2 = new System.Windows.Forms.Button();
            this.Color1 = new System.Windows.Forms.Button();
            this.Color4 = new System.Windows.Forms.Button();
            this.Color8 = new System.Windows.Forms.Button();
            this.Color9 = new System.Windows.Forms.Button();
            this.Color10 = new System.Windows.Forms.Button();
            this.Color11 = new System.Windows.Forms.Button();
            this.Color12 = new System.Windows.Forms.Button();
            this.Color13 = new System.Windows.Forms.Button();
            this.Color14 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.BorderWidth = new System.Windows.Forms.TextBox();
            this.LeftPadding = new System.Windows.Forms.NumericUpDown();
            this.button3 = new System.Windows.Forms.Button();
            this.RightPadding = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LeftPadding)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightPadding)).BeginInit();
            this.SuspendLayout();
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.Location = new System.Drawing.Point(23, 43);
            this.maskedTextBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.maskedTextBox1.Mask = "AAAA";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(125, 22);
            this.maskedTextBox1.TabIndex = 0;
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(23, 176);
            this.AddButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(94, 23);
            this.AddButton.TabIndex = 1;
            this.AddButton.Text = "Добавить";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(188, 10);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(458, 301);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(23, 86);
            this.txtWidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(125, 22);
            this.txtWidth.TabIndex = 3;
            this.txtWidth.Text = "14";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(23, 137);
            this.txtHeight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(125, 22);
            this.txtHeight.TabIndex = 4;
            this.txtHeight.Text = "14";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Ширина символа";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Высота символа";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Код символа";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(21, 224);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(127, 86);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Black;
            this.pictureBox2.Location = new System.Drawing.Point(681, 14);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(411, 296);
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseDown);
            this.pictureBox2.MouseLeave += new System.EventHandler(this.pictureBox2_MouseLeave);
            this.pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseMove);
            this.pictureBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseUp);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1237, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Cut";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LeftSizeTextBox
            // 
            this.LeftSizeTextBox.Location = new System.Drawing.Point(1176, 43);
            this.LeftSizeTextBox.Name = "LeftSizeTextBox";
            this.LeftSizeTextBox.Size = new System.Drawing.Size(58, 22);
            this.LeftSizeTextBox.TabIndex = 11;
            this.LeftSizeTextBox.Text = "0";
            // 
            // RightCutSize
            // 
            this.RightCutSize.Location = new System.Drawing.Point(1257, 43);
            this.RightCutSize.Name = "RightCutSize";
            this.RightCutSize.Size = new System.Drawing.Size(65, 22);
            this.RightCutSize.TabIndex = 12;
            this.RightCutSize.Text = "0";
            // 
            // Color0
            // 
            this.Color0.Location = new System.Drawing.Point(3, 3);
            this.Color0.Name = "Color0";
            this.Color0.Size = new System.Drawing.Size(75, 25);
            this.Color0.TabIndex = 13;
            this.Color0.Text = "Цвет 0";
            this.Color0.UseVisualStyleBackColor = true;
            this.Color0.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.Controls.Add(this.Color15, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.Color7, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.Color6, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.Color5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.Color3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Color2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Color1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Color0, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Color4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.Color8, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Color9, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Color10, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.Color11, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.Color12, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.Color13, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.Color14, 1, 6);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1122, 193);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 254);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // Color15
            // 
            this.Color15.Location = new System.Drawing.Point(113, 220);
            this.Color15.Name = "Color15";
            this.Color15.Size = new System.Drawing.Size(75, 25);
            this.Color15.TabIndex = 28;
            this.Color15.Text = "Цвет 15";
            this.Color15.UseVisualStyleBackColor = true;
            this.Color15.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color7
            // 
            this.Color7.Location = new System.Drawing.Point(3, 220);
            this.Color7.Name = "Color7";
            this.Color7.Size = new System.Drawing.Size(75, 23);
            this.Color7.TabIndex = 20;
            this.Color7.Text = "Цвет 7";
            this.Color7.UseVisualStyleBackColor = true;
            this.Color7.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color6
            // 
            this.Color6.Location = new System.Drawing.Point(3, 189);
            this.Color6.Name = "Color6";
            this.Color6.Size = new System.Drawing.Size(75, 23);
            this.Color6.TabIndex = 19;
            this.Color6.Text = "Цвет 6";
            this.Color6.UseVisualStyleBackColor = true;
            this.Color6.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color5
            // 
            this.Color5.Location = new System.Drawing.Point(3, 158);
            this.Color5.Name = "Color5";
            this.Color5.Size = new System.Drawing.Size(75, 23);
            this.Color5.TabIndex = 18;
            this.Color5.Text = "Цвет 5";
            this.Color5.UseVisualStyleBackColor = true;
            this.Color5.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color3
            // 
            this.Color3.Location = new System.Drawing.Point(3, 96);
            this.Color3.Name = "Color3";
            this.Color3.Size = new System.Drawing.Size(75, 23);
            this.Color3.TabIndex = 16;
            this.Color3.Text = "Цвет 3";
            this.Color3.UseVisualStyleBackColor = true;
            this.Color3.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color2
            // 
            this.Color2.Location = new System.Drawing.Point(3, 65);
            this.Color2.Name = "Color2";
            this.Color2.Size = new System.Drawing.Size(75, 23);
            this.Color2.TabIndex = 15;
            this.Color2.Text = "Цвет 2";
            this.Color2.UseVisualStyleBackColor = true;
            this.Color2.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color1
            // 
            this.Color1.Location = new System.Drawing.Point(3, 34);
            this.Color1.Name = "Color1";
            this.Color1.Size = new System.Drawing.Size(75, 23);
            this.Color1.TabIndex = 14;
            this.Color1.Text = "Цвет 1";
            this.Color1.UseVisualStyleBackColor = true;
            this.Color1.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color4
            // 
            this.Color4.Location = new System.Drawing.Point(3, 127);
            this.Color4.Name = "Color4";
            this.Color4.Size = new System.Drawing.Size(75, 23);
            this.Color4.TabIndex = 17;
            this.Color4.Text = "Цвет 4";
            this.Color4.UseVisualStyleBackColor = true;
            this.Color4.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color8
            // 
            this.Color8.Location = new System.Drawing.Point(113, 3);
            this.Color8.Name = "Color8";
            this.Color8.Size = new System.Drawing.Size(75, 25);
            this.Color8.TabIndex = 21;
            this.Color8.Text = "Цвет 8";
            this.Color8.UseVisualStyleBackColor = true;
            this.Color8.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color9
            // 
            this.Color9.Location = new System.Drawing.Point(113, 34);
            this.Color9.Name = "Color9";
            this.Color9.Size = new System.Drawing.Size(75, 25);
            this.Color9.TabIndex = 22;
            this.Color9.Text = "Цвет 9";
            this.Color9.UseVisualStyleBackColor = true;
            this.Color9.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color10
            // 
            this.Color10.Location = new System.Drawing.Point(113, 65);
            this.Color10.Name = "Color10";
            this.Color10.Size = new System.Drawing.Size(75, 25);
            this.Color10.TabIndex = 23;
            this.Color10.Text = "Цвет 10";
            this.Color10.UseVisualStyleBackColor = true;
            this.Color10.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color11
            // 
            this.Color11.Location = new System.Drawing.Point(113, 96);
            this.Color11.Name = "Color11";
            this.Color11.Size = new System.Drawing.Size(75, 25);
            this.Color11.TabIndex = 24;
            this.Color11.Text = "Цвет 11";
            this.Color11.UseVisualStyleBackColor = true;
            this.Color11.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color12
            // 
            this.Color12.Location = new System.Drawing.Point(113, 127);
            this.Color12.Name = "Color12";
            this.Color12.Size = new System.Drawing.Size(75, 25);
            this.Color12.TabIndex = 25;
            this.Color12.Text = "Цвет 12";
            this.Color12.UseVisualStyleBackColor = true;
            this.Color12.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color13
            // 
            this.Color13.Location = new System.Drawing.Point(113, 158);
            this.Color13.Name = "Color13";
            this.Color13.Size = new System.Drawing.Size(75, 25);
            this.Color13.TabIndex = 26;
            this.Color13.Text = "Цвет 13";
            this.Color13.UseVisualStyleBackColor = true;
            this.Color13.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // Color14
            // 
            this.Color14.Location = new System.Drawing.Point(113, 189);
            this.Color14.Name = "Color14";
            this.Color14.Size = new System.Drawing.Size(75, 25);
            this.Color14.TabIndex = 27;
            this.Color14.Text = "Цвет 14";
            this.Color14.UseVisualStyleBackColor = true;
            this.Color14.Click += new System.EventHandler(this.SelectColor_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1125, 453);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(169, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Отобразить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // BorderWidth
            // 
            this.BorderWidth.Location = new System.Drawing.Point(880, 425);
            this.BorderWidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BorderWidth.Name = "BorderWidth";
            this.BorderWidth.Size = new System.Drawing.Size(125, 22);
            this.BorderWidth.TabIndex = 16;
            this.BorderWidth.Text = "4";
            // 
            // LeftPadding
            // 
            this.LeftPadding.Location = new System.Drawing.Point(1114, 127);
            this.LeftPadding.Name = "LeftPadding";
            this.LeftPadding.Size = new System.Drawing.Size(120, 22);
            this.LeftPadding.TabIndex = 17;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1237, 164);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 18;
            this.button3.Text = "padding";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // RightPadding
            // 
            this.RightPadding.Location = new System.Drawing.Point(1241, 127);
            this.RightPadding.Name = "RightPadding";
            this.RightPadding.Size = new System.Drawing.Size(120, 22);
            this.RightPadding.TabIndex = 19;
            // 
            // AddSymForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1324, 600);
            this.Controls.Add(this.RightPadding);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.LeftPadding);
            this.Controls.Add(this.BorderWidth);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.RightCutSize);
            this.Controls.Add(this.LeftSizeTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.maskedTextBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AddSymForm";
            this.Text = "AddSymForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LeftPadding)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightPadding)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox maskedTextBox1;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox LeftSizeTextBox;
        private System.Windows.Forms.TextBox RightCutSize;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button Color0;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button Color7;
        private System.Windows.Forms.Button Color6;
        private System.Windows.Forms.Button Color5;
        private System.Windows.Forms.Button Color3;
        private System.Windows.Forms.Button Color2;
        private System.Windows.Forms.Button Color1;
        private System.Windows.Forms.Button Color4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button Color15;
        private System.Windows.Forms.Button Color8;
        private System.Windows.Forms.Button Color9;
        private System.Windows.Forms.Button Color10;
        private System.Windows.Forms.Button Color11;
        private System.Windows.Forms.Button Color12;
        private System.Windows.Forms.Button Color13;
        private System.Windows.Forms.Button Color14;
        private System.Windows.Forms.TextBox BorderWidth;
        private System.Windows.Forms.NumericUpDown LeftPadding;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.NumericUpDown RightPadding;
    }
}