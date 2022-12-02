﻿
namespace FFU_Editor
{
    partial class FontInfoForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.PlusScaleTextBox = new System.Windows.Forms.TextBox();
            this.LittleEndianLabel = new System.Windows.Forms.Label();
            this.EndingTextBox = new System.Windows.Forms.TextBox();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.HeightTextBox = new System.Windows.Forms.TextBox();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.WidthTextBox = new System.Windows.Forms.TextBox();
            this.CodekLabel = new System.Windows.Forms.Label();
            this.MinusScaleTextBox = new System.Windows.Forms.NumericUpDown();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CodekComboBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinusScaleTextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.PlusScaleTextBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.LittleEndianLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.EndingTextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.HeightLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.HeightTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.WidthLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.WidthTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.CodekLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.MinusScaleTextBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.CodekComboBox, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(38, 23);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(248, 205);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 35);
            this.label7.TabIndex = 12;
            this.label7.Text = "Уменьшение?";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 136);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 34);
            this.label6.TabIndex = 10;
            this.label6.Text = "Увеличение?";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PlusScaleTextBox
            // 
            this.PlusScaleTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PlusScaleTextBox.Location = new System.Drawing.Point(139, 142);
            this.PlusScaleTextBox.Name = "PlusScaleTextBox";
            this.PlusScaleTextBox.ReadOnly = true;
            this.PlusScaleTextBox.Size = new System.Drawing.Size(94, 22);
            this.PlusScaleTextBox.TabIndex = 11;
            // 
            // LittleEndianLabel
            // 
            this.LittleEndianLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LittleEndianLabel.AutoSize = true;
            this.LittleEndianLabel.Location = new System.Drawing.Point(3, 102);
            this.LittleEndianLabel.Name = "LittleEndianLabel";
            this.LittleEndianLabel.Size = new System.Drawing.Size(118, 34);
            this.LittleEndianLabel.TabIndex = 6;
            this.LittleEndianLabel.Text = "Порядок байтов";
            this.LittleEndianLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EndingTextBox
            // 
            this.EndingTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.EndingTextBox.Location = new System.Drawing.Point(139, 108);
            this.EndingTextBox.Name = "EndingTextBox";
            this.EndingTextBox.ReadOnly = true;
            this.EndingTextBox.Size = new System.Drawing.Size(94, 22);
            this.EndingTextBox.TabIndex = 7;
            // 
            // HeightLabel
            // 
            this.HeightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeightLabel.AutoSize = true;
            this.HeightLabel.Location = new System.Drawing.Point(3, 68);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(118, 34);
            this.HeightLabel.TabIndex = 4;
            this.HeightLabel.Text = "Высота символа";
            this.HeightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HeightTextBox
            // 
            this.HeightTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.HeightTextBox.Location = new System.Drawing.Point(139, 74);
            this.HeightTextBox.Name = "HeightTextBox";
            this.HeightTextBox.ReadOnly = true;
            this.HeightTextBox.Size = new System.Drawing.Size(94, 22);
            this.HeightTextBox.TabIndex = 5;
            // 
            // WidthLabel
            // 
            this.WidthLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.Location = new System.Drawing.Point(3, 34);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(118, 34);
            this.WidthLabel.TabIndex = 2;
            this.WidthLabel.Text = "Ширина символа";
            this.WidthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WidthTextBox
            // 
            this.WidthTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.WidthTextBox.Location = new System.Drawing.Point(139, 40);
            this.WidthTextBox.Name = "WidthTextBox";
            this.WidthTextBox.ReadOnly = true;
            this.WidthTextBox.Size = new System.Drawing.Size(94, 22);
            this.WidthTextBox.TabIndex = 3;
            // 
            // CodekLabel
            // 
            this.CodekLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CodekLabel.AutoSize = true;
            this.CodekLabel.Location = new System.Drawing.Point(3, 0);
            this.CodekLabel.Name = "CodekLabel";
            this.CodekLabel.Size = new System.Drawing.Size(118, 34);
            this.CodekLabel.TabIndex = 0;
            this.CodekLabel.Text = "Кодирование";
            this.CodekLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MinusScaleTextBox
            // 
            this.MinusScaleTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MinusScaleTextBox.Location = new System.Drawing.Point(144, 176);
            this.MinusScaleTextBox.Name = "MinusScaleTextBox";
            this.MinusScaleTextBox.Size = new System.Drawing.Size(84, 22);
            this.MinusScaleTextBox.TabIndex = 2;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(143, 234);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(143, 39);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Сохранить";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CodekComboBox
            // 
            this.CodekComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CodekComboBox.FormattingEnabled = true;
            this.CodekComboBox.Items.AddRange(new object[] {
            "3bpp",
            "4bpp"});
            this.CodekComboBox.Location = new System.Drawing.Point(141, 4);
            this.CodekComboBox.Name = "CodekComboBox";
            this.CodekComboBox.Size = new System.Drawing.Size(89, 24);
            this.CodekComboBox.TabIndex = 13;
            // 
            // FontInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 319);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FontInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Параметры шрифта";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinusScaleTextBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox PlusScaleTextBox;
        private System.Windows.Forms.Label LittleEndianLabel;
        private System.Windows.Forms.TextBox EndingTextBox;
        private System.Windows.Forms.Label HeightLabel;
        private System.Windows.Forms.TextBox HeightTextBox;
        private System.Windows.Forms.Label WidthLabel;
        private System.Windows.Forms.TextBox WidthTextBox;
        private System.Windows.Forms.Label CodekLabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.NumericUpDown MinusScaleTextBox;
        private System.Windows.Forms.ComboBox CodekComboBox;
    }
}