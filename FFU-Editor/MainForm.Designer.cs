namespace FFU_Editor
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
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportFromFFUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportPNGToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportXMLFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GenerateFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetPaddingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportPaddingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FontOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallIcon = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEnterSymCode = new System.Windows.Forms.Button();
            this.mtxtSymCode = new System.Windows.Forms.MaskedTextBox();
            this.smallIconWrap = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbSymbols = new System.Windows.Forms.PictureBox();
            this.SymImageAddressLabel = new System.Windows.Forms.Label();
            this.SymHeaderAddressLabel = new System.Windows.Forms.Label();
            this.SymHeaderAddressTextBox = new System.Windows.Forms.TextBox();
            this.SymImageAddressTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PalitteLabel = new System.Windows.Forms.Label();
            this.PalitteComboBox = new System.Windows.Forms.ComboBox();
            this.BackgroundLabel = new System.Windows.Forms.Label();
            this.BackgroundComboBox = new System.Windows.Forms.ComboBox();
            this.TemplateSymbolsBox = new System.Windows.Forms.GroupBox();
            this.SetSymbolsButton = new System.Windows.Forms.Button();
            this.ImportXMLFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.smallIcon)).BeginInit();
            this.smallIconWrap.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSymbols)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.TemplateSymbolsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.EditingToolStripMenuItem,
            this.ToolsToolStripMenuItem,
            this.FontOptionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1047, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.SaveAsToolStripMenuItem,
            this.ImportToolStripMenuItem,
            this.ExportToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(59, 24);
            this.FileToolStripMenuItem.Text = "Файл";
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.OpenToolStripMenuItem.Text = "Открыть";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.SaveToolStripMenuItem.Text = "Сохранить";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.SaveAsToolStripMenuItem.Text = "Сохранить как";
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // ImportToolStripMenuItem
            // 
            this.ImportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportFromFFUToolStripMenuItem,
            this.ImportXMLFontToolStripMenuItem});
            this.ImportToolStripMenuItem.Name = "ImportToolStripMenuItem";
            this.ImportToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.ImportToolStripMenuItem.Text = "Импортировать из";
            // 
            // ImportFromFFUToolStripMenuItem
            // 
            this.ImportFromFFUToolStripMenuItem.Name = "ImportFromFFUToolStripMenuItem";
            this.ImportFromFFUToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.ImportFromFFUToolStripMenuItem.Text = "FFU";
            this.ImportFromFFUToolStripMenuItem.Click += new System.EventHandler(this.ImportFromFFUToolStripMenuItem_Click);
            // 
            // ExportToolStripMenuItem
            // 
            this.ExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportPNGToolStripMenuItem1,
            this.ExportXMLFontToolStripMenuItem});
            this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            this.ExportToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.ExportToolStripMenuItem.Text = "Экспортировать в";
            // 
            // ExportPNGToolStripMenuItem1
            // 
            this.ExportPNGToolStripMenuItem1.Name = "ExportPNGToolStripMenuItem1";
            this.ExportPNGToolStripMenuItem1.Size = new System.Drawing.Size(224, 26);
            this.ExportPNGToolStripMenuItem1.Text = "PNG";
            this.ExportPNGToolStripMenuItem1.Click += new System.EventHandler(this.PNGToolStripMenuItem_Click);
            // 
            // ExportXMLFontToolStripMenuItem
            // 
            this.ExportXMLFontToolStripMenuItem.Name = "ExportXMLFontToolStripMenuItem";
            this.ExportXMLFontToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.ExportXMLFontToolStripMenuItem.Text = "XML Шрифт";
            this.ExportXMLFontToolStripMenuItem.Click += new System.EventHandler(this.PNGToolStripMenuItem_Click);
            // 
            // EditingToolStripMenuItem
            // 
            this.EditingToolStripMenuItem.Name = "EditingToolStripMenuItem";
            this.EditingToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.EditingToolStripMenuItem.Text = "Правка";
            // 
            // ToolsToolStripMenuItem
            // 
            this.ToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GenerateFontToolStripMenuItem,
            this.SetPaddingToolStripMenuItem,
            this.ImportPaddingToolStripMenuItem});
            this.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
            this.ToolsToolStripMenuItem.Size = new System.Drawing.Size(117, 24);
            this.ToolsToolStripMenuItem.Text = "Инструменты";
            // 
            // GenerateFontToolStripMenuItem
            // 
            this.GenerateFontToolStripMenuItem.Name = "GenerateFontToolStripMenuItem";
            this.GenerateFontToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.GenerateFontToolStripMenuItem.Text = "Сгенерировать шрифт";
            this.GenerateFontToolStripMenuItem.Click += new System.EventHandler(this.GenerateFontToolStripMenuItem_Click);
            // 
            // SetPaddingToolStripMenuItem
            // 
            this.SetPaddingToolStripMenuItem.Name = "SetPaddingToolStripMenuItem";
            this.SetPaddingToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.SetPaddingToolStripMenuItem.Text = "Установить отступы";
            this.SetPaddingToolStripMenuItem.Click += new System.EventHandler(this.SetPaddingToolStripMenuItem_Click);
            // 
            // ImportPaddingToolStripMenuItem
            // 
            this.ImportPaddingToolStripMenuItem.Name = "ImportPaddingToolStripMenuItem";
            this.ImportPaddingToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.ImportPaddingToolStripMenuItem.Text = "Импортировать отступы";
            this.ImportPaddingToolStripMenuItem.Click += new System.EventHandler(this.ImportPaddingToolStripMenuItem_Click);
            // 
            // FontOptionsToolStripMenuItem
            // 
            this.FontOptionsToolStripMenuItem.Name = "FontOptionsToolStripMenuItem";
            this.FontOptionsToolStripMenuItem.Size = new System.Drawing.Size(162, 24);
            this.FontOptionsToolStripMenuItem.Text = "Параметры шрифта";
            this.FontOptionsToolStripMenuItem.Click += new System.EventHandler(this.FontOptionsToolStripMenuItem_Click);
            // 
            // smallIcon
            // 
            this.smallIcon.BackColor = System.Drawing.Color.Black;
            this.smallIcon.Location = new System.Drawing.Point(19, 19);
            this.smallIcon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.smallIcon.Name = "smallIcon";
            this.smallIcon.Size = new System.Drawing.Size(125, 112);
            this.smallIcon.TabIndex = 1;
            this.smallIcon.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 34);
            this.label1.TabIndex = 2;
            this.label1.Text = "Шестнадцатиричный\r\n код символа";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnEnterSymCode
            // 
            this.btnEnterSymCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnterSymCode.Location = new System.Drawing.Point(25, 90);
            this.btnEnterSymCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEnterSymCode.Name = "btnEnterSymCode";
            this.btnEnterSymCode.Size = new System.Drawing.Size(125, 29);
            this.btnEnterSymCode.TabIndex = 4;
            this.btnEnterSymCode.TabStop = false;
            this.btnEnterSymCode.Text = "Показать";
            this.btnEnterSymCode.UseVisualStyleBackColor = true;
            this.btnEnterSymCode.Click += new System.EventHandler(this.ButtonEnterSymCode_Click);
            // 
            // mtxtSymCode
            // 
            this.mtxtSymCode.Location = new System.Drawing.Point(25, 64);
            this.mtxtSymCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mtxtSymCode.Mask = "AAAA";
            this.mtxtSymCode.Name = "mtxtSymCode";
            this.mtxtSymCode.Size = new System.Drawing.Size(125, 22);
            this.mtxtSymCode.TabIndex = 5;
            this.mtxtSymCode.TabStop = false;
            this.mtxtSymCode.TextChanged += new System.EventHandler(this.MtxtSymCode_TextChanged);
            this.mtxtSymCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MtxtSymCode_KeyDown);
            // 
            // smallIconWrap
            // 
            this.smallIconWrap.Controls.Add(this.smallIcon);
            this.smallIconWrap.Location = new System.Drawing.Point(32, 41);
            this.smallIconWrap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.smallIconWrap.Name = "smallIconWrap";
            this.smallIconWrap.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.smallIconWrap.Size = new System.Drawing.Size(187, 149);
            this.smallIconWrap.TabIndex = 3;
            this.smallIconWrap.TabStop = false;
            this.smallIconWrap.Text = "Миниатюра";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.btnAddNew);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnEnterSymCode);
            this.groupBox2.Controls.Add(this.mtxtSymCode);
            this.groupBox2.Location = new System.Drawing.Point(26, 194);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(171, 227);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Текущий символ";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(25, 192);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(122, 31);
            this.button2.TabIndex = 9;
            this.button2.Text = "Удалить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(25, 158);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 30);
            this.button1.TabIndex = 8;
            this.button1.TabStop = false;
            this.button1.Text = "Изменить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ButtonRedact_Click);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Location = new System.Drawing.Point(25, 123);
            this.btnAddNew.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(123, 31);
            this.btnAddNew.TabIndex = 7;
            this.btnAddNew.TabStop = false;
            this.btnAddNew.Text = "Добавить новый";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.ButtonAddNew_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 528);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1047, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 2);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(151, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // pbSymbols
            // 
            this.pbSymbols.BackColor = System.Drawing.Color.Black;
            this.pbSymbols.Location = new System.Drawing.Point(9, 61);
            this.pbSymbols.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pbSymbols.Name = "pbSymbols";
            this.pbSymbols.Size = new System.Drawing.Size(773, 397);
            this.pbSymbols.TabIndex = 8;
            this.pbSymbols.TabStop = false;
            // 
            // SymImageAddressLabel
            // 
            this.SymImageAddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SymImageAddressLabel.AutoSize = true;
            this.SymImageAddressLabel.Location = new System.Drawing.Point(3, 40);
            this.SymImageAddressLabel.Name = "SymImageAddressLabel";
            this.SymImageAddressLabel.Size = new System.Drawing.Size(76, 41);
            this.SymImageAddressLabel.TabIndex = 9;
            this.SymImageAddressLabel.Text = "Данные";
            this.SymImageAddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SymHeaderAddressLabel
            // 
            this.SymHeaderAddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SymHeaderAddressLabel.AutoSize = true;
            this.SymHeaderAddressLabel.Location = new System.Drawing.Point(3, 0);
            this.SymHeaderAddressLabel.Name = "SymHeaderAddressLabel";
            this.SymHeaderAddressLabel.Size = new System.Drawing.Size(76, 40);
            this.SymHeaderAddressLabel.TabIndex = 10;
            this.SymHeaderAddressLabel.Text = "Заголовок";
            this.SymHeaderAddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SymHeaderAddressTextBox
            // 
            this.SymHeaderAddressTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SymHeaderAddressTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.SymHeaderAddressTextBox.Location = new System.Drawing.Point(87, 9);
            this.SymHeaderAddressTextBox.Name = "SymHeaderAddressTextBox";
            this.SymHeaderAddressTextBox.ReadOnly = true;
            this.SymHeaderAddressTextBox.Size = new System.Drawing.Size(100, 22);
            this.SymHeaderAddressTextBox.TabIndex = 11;
            // 
            // SymImageAddressTextBox
            // 
            this.SymImageAddressTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SymImageAddressTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.SymImageAddressTextBox.Location = new System.Drawing.Point(87, 49);
            this.SymImageAddressTextBox.Name = "SymImageAddressTextBox";
            this.SymImageAddressTextBox.ReadOnly = true;
            this.SymImageAddressTextBox.Size = new System.Drawing.Size(100, 22);
            this.SymImageAddressTextBox.TabIndex = 12;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.SymHeaderAddressLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SymImageAddressLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.SymHeaderAddressTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.SymImageAddressTextBox, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(26, 426);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(193, 81);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // PalitteLabel
            // 
            this.PalitteLabel.AutoSize = true;
            this.PalitteLabel.Location = new System.Drawing.Point(6, 32);
            this.PalitteLabel.Name = "PalitteLabel";
            this.PalitteLabel.Size = new System.Drawing.Size(65, 17);
            this.PalitteLabel.TabIndex = 14;
            this.PalitteLabel.Text = "Палитра";
            // 
            // PalitteComboBox
            // 
            this.PalitteComboBox.FormattingEnabled = true;
            this.PalitteComboBox.Location = new System.Drawing.Point(77, 32);
            this.PalitteComboBox.Name = "PalitteComboBox";
            this.PalitteComboBox.Size = new System.Drawing.Size(121, 24);
            this.PalitteComboBox.TabIndex = 15;
            this.PalitteComboBox.SelectedIndexChanged += new System.EventHandler(this.PalitteComboBox_SelectedIndexChanged);
            // 
            // BackgroundLabel
            // 
            this.BackgroundLabel.AutoSize = true;
            this.BackgroundLabel.Location = new System.Drawing.Point(218, 32);
            this.BackgroundLabel.Name = "BackgroundLabel";
            this.BackgroundLabel.Size = new System.Drawing.Size(37, 17);
            this.BackgroundLabel.TabIndex = 16;
            this.BackgroundLabel.Text = "Фон";
            // 
            // BackgroundComboBox
            // 
            this.BackgroundComboBox.FormattingEnabled = true;
            this.BackgroundComboBox.Items.AddRange(new object[] {
            "Черный",
            "Белый"});
            this.BackgroundComboBox.Location = new System.Drawing.Point(261, 29);
            this.BackgroundComboBox.Name = "BackgroundComboBox";
            this.BackgroundComboBox.Size = new System.Drawing.Size(121, 24);
            this.BackgroundComboBox.TabIndex = 17;
            this.BackgroundComboBox.SelectedIndexChanged += new System.EventHandler(this.BackgroundComboBox_SelectedIndexChanged);
            // 
            // TemplateSymbolsBox
            // 
            this.TemplateSymbolsBox.Controls.Add(this.SetSymbolsButton);
            this.TemplateSymbolsBox.Controls.Add(this.PalitteLabel);
            this.TemplateSymbolsBox.Controls.Add(this.BackgroundComboBox);
            this.TemplateSymbolsBox.Controls.Add(this.pbSymbols);
            this.TemplateSymbolsBox.Controls.Add(this.PalitteComboBox);
            this.TemplateSymbolsBox.Controls.Add(this.BackgroundLabel);
            this.TemplateSymbolsBox.Location = new System.Drawing.Point(240, 44);
            this.TemplateSymbolsBox.Name = "TemplateSymbolsBox";
            this.TemplateSymbolsBox.Size = new System.Drawing.Size(795, 463);
            this.TemplateSymbolsBox.TabIndex = 18;
            this.TemplateSymbolsBox.TabStop = false;
            this.TemplateSymbolsBox.Text = "Пример";
            // 
            // SetSymbolsButton
            // 
            this.SetSymbolsButton.Location = new System.Drawing.Point(490, 21);
            this.SetSymbolsButton.Name = "SetSymbolsButton";
            this.SetSymbolsButton.Size = new System.Drawing.Size(292, 34);
            this.SetSymbolsButton.TabIndex = 18;
            this.SetSymbolsButton.Text = "Изменить модифицируемые символы";
            this.SetSymbolsButton.UseVisualStyleBackColor = true;
            this.SetSymbolsButton.Click += new System.EventHandler(this.SetSymbolsButton_Click);
            // 
            // ImportXMLFontToolStripMenuItem
            // 
            this.ImportXMLFontToolStripMenuItem.Name = "ImportXMLFontToolStripMenuItem";
            this.ImportXMLFontToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.ImportXMLFontToolStripMenuItem.Text = "XML Шрифта";
            this.ImportXMLFontToolStripMenuItem.Click += new System.EventHandler(this.ImportXMLFontToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 550);
            this.Controls.Add(this.TemplateSymbolsBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.smallIconWrap);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "FFU Редактор";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Click += new System.EventHandler(this.MainForm_Click);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.smallIcon)).EndInit();
            this.smallIconWrap.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSymbols)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.TemplateSymbolsBox.ResumeLayout(false);
            this.TemplateSymbolsBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.PictureBox smallIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEnterSymCode;
        private System.Windows.Forms.MaskedTextBox mtxtSymCode;
        private System.Windows.Forms.GroupBox smallIconWrap;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.PictureBox pbSymbols;
        private System.Windows.Forms.Label SymImageAddressLabel;
        private System.Windows.Forms.Label SymHeaderAddressLabel;
        private System.Windows.Forms.TextBox SymHeaderAddressTextBox;
        private System.Windows.Forms.TextBox SymImageAddressTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label PalitteLabel;
        private System.Windows.Forms.ComboBox PalitteComboBox;
        private System.Windows.Forms.Label BackgroundLabel;
        private System.Windows.Forms.ComboBox BackgroundComboBox;
        private System.Windows.Forms.GroupBox TemplateSymbolsBox;
        private System.Windows.Forms.Button SetSymbolsButton;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportPNGToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ExportXMLFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportFromFFUToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GenerateFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SetPaddingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportPaddingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FontOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportXMLFontToolStripMenuItem;
    }
}
