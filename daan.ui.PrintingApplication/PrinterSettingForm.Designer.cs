namespace daan.ui.PrintingApplication
{
    partial class PrinterSettingForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterSettingForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_Mac = new CCWin.SkinControl.SkinLabel();
            this.lbl_A4Printer = new CCWin.SkinControl.SkinLabel();
            this.lbl_BarcodePrinter = new CCWin.SkinControl.SkinLabel();
            this.lbl_hostname = new CCWin.SkinControl.SkinLabel();
            this.lbl_A5Printer = new CCWin.SkinControl.SkinLabel();
            this.lbl_PDFPrinter = new CCWin.SkinControl.SkinLabel();
            this.cb_A4Printer = new CCWin.SkinControl.SkinComboBox();
            this.cb_BarcodePrinter = new CCWin.SkinControl.SkinComboBox();
            this.cb_A5Printer = new CCWin.SkinControl.SkinComboBox();
            this.cb_PDFPrinter = new CCWin.SkinControl.SkinComboBox();
            this.mac = new CCWin.SkinControl.SkinLabel();
            this.hostname = new CCWin.SkinControl.SkinLabel();
            this.tlp_Buttons = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new CCWin.SkinControl.SkinButton();
            this.btnSave = new CCWin.SkinControl.SkinButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.tlp_Buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanel1.Controls.Add(this.lbl_Mac, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_A4Printer, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbl_BarcodePrinter, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbl_hostname, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_A5Printer, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbl_PDFPrinter, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.cb_A4Printer, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cb_BarcodePrinter, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cb_A5Printer, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.cb_PDFPrinter, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.mac, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.hostname, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.tlp_Buttons, 4, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 28);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(692, 168);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbl_Mac
            // 
            this.lbl_Mac.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_Mac.AutoSize = true;
            this.lbl_Mac.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Mac.BorderColor = System.Drawing.Color.White;
            this.lbl_Mac.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Mac.Location = new System.Drawing.Point(58, 6);
            this.lbl_Mac.Name = "lbl_Mac";
            this.lbl_Mac.Size = new System.Drawing.Size(59, 20);
            this.lbl_Mac.TabIndex = 0;
            this.lbl_Mac.Text = "Mac码:";
            // 
            // lbl_A4Printer
            // 
            this.lbl_A4Printer.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_A4Printer.AutoSize = true;
            this.lbl_A4Printer.BackColor = System.Drawing.Color.Transparent;
            this.lbl_A4Printer.BorderColor = System.Drawing.Color.White;
            this.lbl_A4Printer.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_A4Printer.Location = new System.Drawing.Point(9, 38);
            this.lbl_A4Printer.Name = "lbl_A4Printer";
            this.lbl_A4Printer.Size = new System.Drawing.Size(108, 20);
            this.lbl_A4Printer.TabIndex = 1;
            this.lbl_A4Printer.Text = "A4报告打印机:";
            // 
            // lbl_BarcodePrinter
            // 
            this.lbl_BarcodePrinter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_BarcodePrinter.AutoSize = true;
            this.lbl_BarcodePrinter.BackColor = System.Drawing.Color.Transparent;
            this.lbl_BarcodePrinter.BorderColor = System.Drawing.Color.White;
            this.lbl_BarcodePrinter.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_BarcodePrinter.Location = new System.Drawing.Point(29, 70);
            this.lbl_BarcodePrinter.Name = "lbl_BarcodePrinter";
            this.lbl_BarcodePrinter.Size = new System.Drawing.Size(88, 20);
            this.lbl_BarcodePrinter.TabIndex = 2;
            this.lbl_BarcodePrinter.Text = "条码打印机:";
            // 
            // lbl_hostname
            // 
            this.lbl_hostname.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_hostname.AutoSize = true;
            this.lbl_hostname.BackColor = System.Drawing.Color.Transparent;
            this.lbl_hostname.BorderColor = System.Drawing.Color.White;
            this.lbl_hostname.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_hostname.Location = new System.Drawing.Point(405, 6);
            this.lbl_hostname.Name = "lbl_hostname";
            this.lbl_hostname.Size = new System.Drawing.Size(58, 20);
            this.lbl_hostname.TabIndex = 3;
            this.lbl_hostname.Text = "主机名:";
            // 
            // lbl_A5Printer
            // 
            this.lbl_A5Printer.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_A5Printer.AutoSize = true;
            this.lbl_A5Printer.BackColor = System.Drawing.Color.Transparent;
            this.lbl_A5Printer.BorderColor = System.Drawing.Color.White;
            this.lbl_A5Printer.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_A5Printer.Location = new System.Drawing.Point(355, 38);
            this.lbl_A5Printer.Name = "lbl_A5Printer";
            this.lbl_A5Printer.Size = new System.Drawing.Size(108, 20);
            this.lbl_A5Printer.TabIndex = 4;
            this.lbl_A5Printer.Text = "A5报告打印机:";
            // 
            // lbl_PDFPrinter
            // 
            this.lbl_PDFPrinter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_PDFPrinter.AutoSize = true;
            this.lbl_PDFPrinter.BackColor = System.Drawing.Color.Transparent;
            this.lbl_PDFPrinter.BorderColor = System.Drawing.Color.White;
            this.lbl_PDFPrinter.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_PDFPrinter.Location = new System.Drawing.Point(377, 70);
            this.lbl_PDFPrinter.Name = "lbl_PDFPrinter";
            this.lbl_PDFPrinter.Size = new System.Drawing.Size(86, 20);
            this.lbl_PDFPrinter.TabIndex = 5;
            this.lbl_PDFPrinter.Text = "PDF打印机:";
            // 
            // cb_A4Printer
            // 
            this.cb_A4Printer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_A4Printer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_A4Printer.FormattingEnabled = true;
            this.cb_A4Printer.Location = new System.Drawing.Point(123, 35);
            this.cb_A4Printer.Name = "cb_A4Printer";
            this.cb_A4Printer.Size = new System.Drawing.Size(201, 25);
            this.cb_A4Printer.TabIndex = 6;
            this.cb_A4Printer.WaterText = "";
            // 
            // cb_BarcodePrinter
            // 
            this.cb_BarcodePrinter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_BarcodePrinter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_BarcodePrinter.FormattingEnabled = true;
            this.cb_BarcodePrinter.Location = new System.Drawing.Point(123, 67);
            this.cb_BarcodePrinter.Name = "cb_BarcodePrinter";
            this.cb_BarcodePrinter.Size = new System.Drawing.Size(201, 25);
            this.cb_BarcodePrinter.TabIndex = 7;
            this.cb_BarcodePrinter.WaterText = "";
            // 
            // cb_A5Printer
            // 
            this.cb_A5Printer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_A5Printer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_A5Printer.FormattingEnabled = true;
            this.cb_A5Printer.Location = new System.Drawing.Point(469, 35);
            this.cb_A5Printer.Name = "cb_A5Printer";
            this.cb_A5Printer.Size = new System.Drawing.Size(201, 25);
            this.cb_A5Printer.TabIndex = 8;
            this.cb_A5Printer.WaterText = "";
            // 
            // cb_PDFPrinter
            // 
            this.cb_PDFPrinter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_PDFPrinter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_PDFPrinter.FormattingEnabled = true;
            this.cb_PDFPrinter.Location = new System.Drawing.Point(469, 67);
            this.cb_PDFPrinter.Name = "cb_PDFPrinter";
            this.cb_PDFPrinter.Size = new System.Drawing.Size(201, 25);
            this.cb_PDFPrinter.TabIndex = 9;
            this.cb_PDFPrinter.WaterText = "";
            // 
            // mac
            // 
            this.mac.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mac.AutoSize = true;
            this.mac.BackColor = System.Drawing.Color.Transparent;
            this.mac.BorderColor = System.Drawing.Color.White;
            this.mac.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mac.Location = new System.Drawing.Point(123, 6);
            this.mac.Name = "mac";
            this.mac.Size = new System.Drawing.Size(147, 20);
            this.mac.TabIndex = 10;
            this.mac.Text = "00-00-00-00-00-00";
            // 
            // hostname
            // 
            this.hostname.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.hostname.AutoSize = true;
            this.hostname.BackColor = System.Drawing.Color.Transparent;
            this.hostname.BorderColor = System.Drawing.Color.White;
            this.hostname.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.hostname.Location = new System.Drawing.Point(469, 6);
            this.hostname.Name = "hostname";
            this.hostname.Size = new System.Drawing.Size(157, 20);
            this.hostname.TabIndex = 11;
            this.hostname.Text = "www.hostname.com";
            // 
            // tlp_Buttons
            // 
            this.tlp_Buttons.ColumnCount = 3;
            this.tlp_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlp_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlp_Buttons.Controls.Add(this.btnCancel, 2, 0);
            this.tlp_Buttons.Controls.Add(this.btnSave, 1, 0);
            this.tlp_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Buttons.Location = new System.Drawing.Point(466, 116);
            this.tlp_Buttons.Margin = new System.Windows.Forms.Padding(0);
            this.tlp_Buttons.Name = "tlp_Buttons";
            this.tlp_Buttons.RowCount = 1;
            this.tlp_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Buttons.Size = new System.Drawing.Size(207, 36);
            this.tlp_Buttons.TabIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.DownBack = null;
            this.btnCancel.Location = new System.Drawing.Point(130, 3);
            this.btnCancel.MouseBack = null;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NormlBack = null;
            this.btnCancel.Size = new System.Drawing.Size(74, 30);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.DownBack = null;
            this.btnSave.Location = new System.Drawing.Point(50, 3);
            this.btnSave.MouseBack = null;
            this.btnSave.Name = "btnSave";
            this.btnSave.NormlBack = null;
            this.btnSave.Size = new System.Drawing.Size(74, 30);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // PrinterSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 200);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrinterSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PrinterSettingForm";
            this.Load += new System.EventHandler(this.FromPrinterSetting_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tlp_Buttons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CCWin.SkinControl.SkinLabel lbl_Mac;
        private CCWin.SkinControl.SkinLabel lbl_A4Printer;
        private CCWin.SkinControl.SkinLabel lbl_BarcodePrinter;
        private CCWin.SkinControl.SkinLabel lbl_hostname;
        private CCWin.SkinControl.SkinLabel lbl_A5Printer;
        private CCWin.SkinControl.SkinLabel lbl_PDFPrinter;
        private CCWin.SkinControl.SkinComboBox cb_A4Printer;
        private CCWin.SkinControl.SkinComboBox cb_BarcodePrinter;
        private CCWin.SkinControl.SkinComboBox cb_A5Printer;
        private CCWin.SkinControl.SkinComboBox cb_PDFPrinter;
        private CCWin.SkinControl.SkinLabel mac;
        private CCWin.SkinControl.SkinLabel hostname;
        private System.Windows.Forms.TableLayoutPanel tlp_Buttons;
        private CCWin.SkinControl.SkinButton btnCancel;
        private CCWin.SkinControl.SkinButton btnSave;
    }
}