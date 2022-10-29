namespace DLFontViewer
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncompressedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.anotherUncompressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.anotherCompressedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обрезатьСимволToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GenerateFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEnterSymCode = new System.Windows.Forms.Button();
            this.mtxtSymCode = new System.Windows.Forms.MaskedTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbSymbols = new System.Windows.Forms.PictureBox();
            this.SymAddressLabel = new System.Windows.Forms.Label();
            this.SymHeaderAddress = new System.Windows.Forms.Label();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSymbols)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.обрезатьСимволToolStripMenuItem,
            this.GenerateFontToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compressedToolStripMenuItem,
            this.uncompressedToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // compressedToolStripMenuItem
            // 
            this.compressedToolStripMenuItem.Name = "compressedToolStripMenuItem";
            this.compressedToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
            this.compressedToolStripMenuItem.Text = "Compressed";
            this.compressedToolStripMenuItem.Click += new System.EventHandler(this.compressedToolStripMenuItem_Click);
            // 
            // uncompressedToolStripMenuItem
            // 
            this.uncompressedToolStripMenuItem.Name = "uncompressedToolStripMenuItem";
            this.uncompressedToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
            this.uncompressedToolStripMenuItem.Text = "Uncompressed";
            this.uncompressedToolStripMenuItem.Click += new System.EventHandler(this.uncompressedToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.anotherUncompressToolStripMenuItem,
            this.anotherCompressedToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // anotherUncompressToolStripMenuItem
            // 
            this.anotherUncompressToolStripMenuItem.Name = "anotherUncompressToolStripMenuItem";
            this.anotherUncompressToolStripMenuItem.Size = new System.Drawing.Size(229, 26);
            this.anotherUncompressToolStripMenuItem.Text = "Another uncompress";
            this.anotherUncompressToolStripMenuItem.Click += new System.EventHandler(this.anotherUncompressToolStripMenuItem_Click);
            // 
            // anotherCompressedToolStripMenuItem
            // 
            this.anotherCompressedToolStripMenuItem.Name = "anotherCompressedToolStripMenuItem";
            this.anotherCompressedToolStripMenuItem.Size = new System.Drawing.Size(229, 26);
            this.anotherCompressedToolStripMenuItem.Text = "Another compressed";
            this.anotherCompressedToolStripMenuItem.Click += new System.EventHandler(this.anotherCompressedToolStripMenuItem_Click);
            // 
            // обрезатьСимволToolStripMenuItem
            // 
            this.обрезатьСимволToolStripMenuItem.Name = "обрезатьСимволToolStripMenuItem";
            this.обрезатьСимволToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
            this.обрезатьСимволToolStripMenuItem.Text = "Обрезать символ";
            this.обрезатьСимволToolStripMenuItem.Click += new System.EventHandler(this.CutSymsToolStripMenuItem_Click);
            // 
            // GenerateFontToolStripMenuItem
            // 
            this.GenerateFontToolStripMenuItem.Name = "GenerateFontToolStripMenuItem";
            this.GenerateFontToolStripMenuItem.Size = new System.Drawing.Size(179, 24);
            this.GenerateFontToolStripMenuItem.Text = "Сгенерировать шрифт";
            this.GenerateFontToolStripMenuItem.Click += new System.EventHandler(this.GenerateFontToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(6, 30);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(95, 74);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Код символа hex";
            // 
            // btnEnterSymCode
            // 
            this.btnEnterSymCode.Location = new System.Drawing.Point(7, 62);
            this.btnEnterSymCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEnterSymCode.Name = "btnEnterSymCode";
            this.btnEnterSymCode.Size = new System.Drawing.Size(94, 23);
            this.btnEnterSymCode.TabIndex = 4;
            this.btnEnterSymCode.TabStop = false;
            this.btnEnterSymCode.Text = "Показать";
            this.btnEnterSymCode.UseVisualStyleBackColor = true;
            this.btnEnterSymCode.Click += new System.EventHandler(this.btnEnterSymCode_Click);
            // 
            // mtxtSymCode
            // 
            this.mtxtSymCode.Location = new System.Drawing.Point(7, 36);
            this.mtxtSymCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mtxtSymCode.Mask = "AAAA";
            this.mtxtSymCode.Name = "mtxtSymCode";
            this.mtxtSymCode.Size = new System.Drawing.Size(125, 22);
            this.mtxtSymCode.TabIndex = 5;
            this.mtxtSymCode.TabStop = false;
            this.mtxtSymCode.TextChanged += new System.EventHandler(this.mtxtSymCode_TextChanged);
            this.mtxtSymCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxtSymCode_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(20, 198);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(156, 141);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 113);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "ShowFull";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.btnAddNew);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnEnterSymCode);
            this.groupBox2.Controls.Add(this.mtxtSymCode);
            this.groupBox2.Location = new System.Drawing.Point(20, 25);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(156, 173);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(7, 146);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Удалить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 118);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 8;
            this.button1.TabStop = false;
            this.button1.Text = "Изменить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Location = new System.Drawing.Point(6, 90);
            this.btnAddNew.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(94, 23);
            this.btnAddNew.TabIndex = 7;
            this.btnAddNew.TabStop = false;
            this.btnAddNew.Text = "Добавить новый";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 334);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 26);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(151, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // pbSymbols
            // 
            this.pbSymbols.BackColor = System.Drawing.Color.Black;
            this.pbSymbols.Location = new System.Drawing.Point(208, 36);
            this.pbSymbols.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pbSymbols.Name = "pbSymbols";
            this.pbSymbols.Size = new System.Drawing.Size(569, 130);
            this.pbSymbols.TabIndex = 8;
            this.pbSymbols.TabStop = false;
            // 
            // SymAddressLabel
            // 
            this.SymAddressLabel.AutoSize = true;
            this.SymAddressLabel.Location = new System.Drawing.Point(195, 206);
            this.SymAddressLabel.Name = "SymAddressLabel";
            this.SymAddressLabel.Size = new System.Drawing.Size(46, 17);
            this.SymAddressLabel.TabIndex = 9;
            this.SymAddressLabel.Text = "label2";
            // 
            // SymHeaderAddress
            // 
            this.SymHeaderAddress.AutoSize = true;
            this.SymHeaderAddress.Location = new System.Drawing.Point(195, 190);
            this.SymHeaderAddress.Name = "SymHeaderAddress";
            this.SymHeaderAddress.Size = new System.Drawing.Size(46, 17);
            this.SymHeaderAddress.TabIndex = 10;
            this.SymHeaderAddress.Text = "label2";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pNGToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // pNGToolStripMenuItem
            // 
            this.pNGToolStripMenuItem.Name = "pNGToolStripMenuItem";
            this.pNGToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.pNGToolStripMenuItem.Text = "PNG";
            this.pNGToolStripMenuItem.Click += new System.EventHandler(this.pNGToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 360);
            this.Controls.Add(this.SymHeaderAddress);
            this.Controls.Add(this.SymAddressLabel);
            this.Controls.Add(this.pbSymbols);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "DLFontViewer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Click += new System.EventHandler(this.MainForm_Click);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSymbols)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEnterSymCode;
        private System.Windows.Forms.MaskedTextBox mtxtSymCode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pbSymbols;
        private System.Windows.Forms.Label SymAddressLabel;
        private System.Windows.Forms.Label SymHeaderAddress;
        private System.Windows.Forms.ToolStripMenuItem compressedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncompressedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem anotherUncompressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem anotherCompressedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обрезатьСимволToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GenerateFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pNGToolStripMenuItem;
    }
}
