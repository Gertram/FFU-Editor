namespace FFU_Editor
{
    partial class GenerateFontWindow
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.FontFamilyLabel = new System.Windows.Forms.Label();
            this.FontFamilyComboBox = new System.Windows.Forms.ComboBox();
            this.FontSizeLabel = new System.Windows.Forms.Label();
            this.FontColorLabel = new System.Windows.Forms.Label();
            this.FontColorButton = new System.Windows.Forms.Button();
            this.FontBorderSizeLabel = new System.Windows.Forms.Label();
            this.FontBorderSIzeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.FontBorderColorButton = new System.Windows.Forms.Button();
            this.FontColorDialog = new System.Windows.Forms.ColorDialog();
            this.FontBorderColorDialog = new System.Windows.Forms.ColorDialog();
            this.FontSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.SymHeightLabel = new System.Windows.Forms.Label();
            this.SymHeightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.SaveButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.OthersRichTextBox = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontBorderSIzeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontSizeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SymHeightNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(734, 388);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // FontFamilyLabel
            // 
            this.FontFamilyLabel.AutoSize = true;
            this.FontFamilyLabel.Location = new System.Drawing.Point(802, 30);
            this.FontFamilyLabel.Name = "FontFamilyLabel";
            this.FontFamilyLabel.Size = new System.Drawing.Size(52, 16);
            this.FontFamilyLabel.TabIndex = 1;
            this.FontFamilyLabel.Text = "Шрифт";
            // 
            // FontFamilyComboBox
            // 
            this.FontFamilyComboBox.FormattingEnabled = true;
            this.FontFamilyComboBox.Location = new System.Drawing.Point(934, 27);
            this.FontFamilyComboBox.Name = "FontFamilyComboBox";
            this.FontFamilyComboBox.Size = new System.Drawing.Size(121, 24);
            this.FontFamilyComboBox.TabIndex = 2;
            this.FontFamilyComboBox.SelectedIndexChanged += new System.EventHandler(this.FontFamilyComboBox_SelectedIndexChanged);
            // 
            // FontSizeLabel
            // 
            this.FontSizeLabel.AutoSize = true;
            this.FontSizeLabel.Location = new System.Drawing.Point(802, 61);
            this.FontSizeLabel.Name = "FontSizeLabel";
            this.FontSizeLabel.Size = new System.Drawing.Size(111, 16);
            this.FontSizeLabel.TabIndex = 3;
            this.FontSizeLabel.Text = "Размер шрифта";
            // 
            // FontColorLabel
            // 
            this.FontColorLabel.AutoSize = true;
            this.FontColorLabel.Location = new System.Drawing.Point(802, 95);
            this.FontColorLabel.Name = "FontColorLabel";
            this.FontColorLabel.Size = new System.Drawing.Size(93, 16);
            this.FontColorLabel.TabIndex = 5;
            this.FontColorLabel.Text = "Цвет шрифта";
            // 
            // FontColorButton
            // 
            this.FontColorButton.BackColor = System.Drawing.Color.Black;
            this.FontColorButton.Location = new System.Drawing.Point(935, 87);
            this.FontColorButton.Name = "FontColorButton";
            this.FontColorButton.Size = new System.Drawing.Size(120, 24);
            this.FontColorButton.TabIndex = 6;
            this.FontColorButton.Text = "Выбрать";
            this.FontColorButton.UseVisualStyleBackColor = false;
            this.FontColorButton.Click += new System.EventHandler(this.FontColorButton_Click);
            // 
            // FontBorderSizeLabel
            // 
            this.FontBorderSizeLabel.AutoSize = true;
            this.FontBorderSizeLabel.Location = new System.Drawing.Point(802, 132);
            this.FontBorderSizeLabel.Name = "FontBorderSizeLabel";
            this.FontBorderSizeLabel.Size = new System.Drawing.Size(123, 16);
            this.FontBorderSizeLabel.TabIndex = 7;
            this.FontBorderSizeLabel.Text = "Толщина границы";
            // 
            // FontBorderSIzeNumericUpDown
            // 
            this.FontBorderSIzeNumericUpDown.Location = new System.Drawing.Point(935, 132);
            this.FontBorderSIzeNumericUpDown.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.FontBorderSIzeNumericUpDown.Name = "FontBorderSIzeNumericUpDown";
            this.FontBorderSIzeNumericUpDown.Size = new System.Drawing.Size(120, 22);
            this.FontBorderSIzeNumericUpDown.TabIndex = 8;
            this.FontBorderSIzeNumericUpDown.ValueChanged += new System.EventHandler(this.FontBorderSIzeNumericUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(805, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Цвет границы";
            // 
            // FontBorderColorButton
            // 
            this.FontBorderColorButton.BackColor = System.Drawing.Color.White;
            this.FontBorderColorButton.Location = new System.Drawing.Point(934, 162);
            this.FontBorderColorButton.Name = "FontBorderColorButton";
            this.FontBorderColorButton.Size = new System.Drawing.Size(120, 24);
            this.FontBorderColorButton.TabIndex = 10;
            this.FontBorderColorButton.Text = "Выбрать";
            this.FontBorderColorButton.UseVisualStyleBackColor = false;
            this.FontBorderColorButton.Click += new System.EventHandler(this.FontBorderColorButton_Click);
            // 
            // FontSizeNumericUpDown
            // 
            this.FontSizeNumericUpDown.Location = new System.Drawing.Point(935, 57);
            this.FontSizeNumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.FontSizeNumericUpDown.Name = "FontSizeNumericUpDown";
            this.FontSizeNumericUpDown.Size = new System.Drawing.Size(120, 22);
            this.FontSizeNumericUpDown.TabIndex = 11;
            this.FontSizeNumericUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.FontSizeNumericUpDown.ValueChanged += new System.EventHandler(this.FontBorderSIzeNumericUpDown_ValueChanged);
            // 
            // SymHeightLabel
            // 
            this.SymHeightLabel.AutoSize = true;
            this.SymHeightLabel.Location = new System.Drawing.Point(808, 198);
            this.SymHeightLabel.Name = "SymHeightLabel";
            this.SymHeightLabel.Size = new System.Drawing.Size(104, 16);
            this.SymHeightLabel.TabIndex = 12;
            this.SymHeightLabel.Text = "Высота ячейки";
            // 
            // SymHeightNumericUpDown
            // 
            this.SymHeightNumericUpDown.Location = new System.Drawing.Point(934, 198);
            this.SymHeightNumericUpDown.Name = "SymHeightNumericUpDown";
            this.SymHeightNumericUpDown.Size = new System.Drawing.Size(120, 22);
            this.SymHeightNumericUpDown.TabIndex = 13;
            this.SymHeightNumericUpDown.ValueChanged += new System.EventHandler(this.SymHeightNumericUpDown_ValueChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(935, 387);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(121, 23);
            this.SaveButton.TabIndex = 14;
            this.SaveButton.Text = "Сохранить";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 417);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(734, 96);
            this.richTextBox1.TabIndex = 15;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // OthersRichTextBox
            // 
            this.OthersRichTextBox.Location = new System.Drawing.Point(813, 271);
            this.OthersRichTextBox.Name = "OthersRichTextBox";
            this.OthersRichTextBox.Size = new System.Drawing.Size(243, 96);
            this.OthersRichTextBox.TabIndex = 16;
            this.OthersRichTextBox.Text = ".!?,-\\\"()";
            this.OthersRichTextBox.TextChanged += new System.EventHandler(this.OthersRichTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(813, 232);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "Доп символы";
            // 
            // GenerateFontWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1187, 536);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OthersRichTextBox);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.SymHeightNumericUpDown);
            this.Controls.Add(this.SymHeightLabel);
            this.Controls.Add(this.FontSizeNumericUpDown);
            this.Controls.Add(this.FontBorderColorButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FontBorderSIzeNumericUpDown);
            this.Controls.Add(this.FontBorderSizeLabel);
            this.Controls.Add(this.FontColorButton);
            this.Controls.Add(this.FontColorLabel);
            this.Controls.Add(this.FontSizeLabel);
            this.Controls.Add(this.FontFamilyComboBox);
            this.Controls.Add(this.FontFamilyLabel);
            this.Controls.Add(this.pictureBox1);
            this.Name = "GenerateFontWindow";
            this.Text = "GenerateFontWindow";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontBorderSIzeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontSizeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SymHeightNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label FontFamilyLabel;
        private System.Windows.Forms.ComboBox FontFamilyComboBox;
        private System.Windows.Forms.Label FontSizeLabel;
        private System.Windows.Forms.Label FontColorLabel;
        private System.Windows.Forms.Button FontColorButton;
        private System.Windows.Forms.Label FontBorderSizeLabel;
        private System.Windows.Forms.NumericUpDown FontBorderSIzeNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button FontBorderColorButton;
        private System.Windows.Forms.ColorDialog FontColorDialog;
        private System.Windows.Forms.ColorDialog FontBorderColorDialog;
        private System.Windows.Forms.NumericUpDown FontSizeNumericUpDown;
        private System.Windows.Forms.Label SymHeightLabel;
        private System.Windows.Forms.NumericUpDown SymHeightNumericUpDown;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox OthersRichTextBox;
        private System.Windows.Forms.Label label2;
    }
}