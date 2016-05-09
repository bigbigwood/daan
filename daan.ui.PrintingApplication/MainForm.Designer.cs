namespace daan.ui.PrintingApplication
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MainTabControl = new CCWin.SkinControl.SkinTabControl();
            this.skinTabPage1 = new CCWin.SkinControl.SkinTabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ReportToolBar = new CCWin.SkinControl.SkinToolStrip();
            this.btnQueryOrder = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.tlp_queryConditions = new System.Windows.Forms.TableLayoutPanel();
            this.dpSTo = new System.Windows.Forms.DateTimePicker();
            this.dpTo = new System.Windows.Forms.DateTimePicker();
            this.dpFrom = new System.Windows.Forms.DateTimePicker();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel4 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel5 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel6 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel7 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel8 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel9 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel10 = new CCWin.SkinControl.SkinLabel();
            this.dropDictLab = new CCWin.SkinControl.SkinComboBox();
            this.dropStatus = new CCWin.SkinControl.SkinComboBox();
            this.dropDictcustomer = new CCWin.SkinControl.SkinComboBox();
            this.dropReportStatus = new CCWin.SkinControl.SkinComboBox();
            this.tbxName = new CCWin.SkinControl.SkinTextBox();
            this.tbxOrderNum = new CCWin.SkinControl.SkinTextBox();
            this.dpSFrom = new System.Windows.Forms.DateTimePicker();
            this.dgv_orders = new System.Windows.Forms.DataGridView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.OrderStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_OrderNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_Sex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_Age = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_Mobile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_CreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_OrganizationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_OrderPackageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_Section = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_SamplingDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_PostAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_Recipient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell_ContractNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainTableLayoutPanel.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.skinTabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.ReportToolBar.SuspendLayout();
            this.tlp_queryConditions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_orders)).BeginInit();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.MainTabControl, 0, 1);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(4, 28);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 3;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(992, 618);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // MainTabControl
            // 
            this.MainTabControl.AnimatorType = CCWin.SkinControl.AnimationType.HorizSlide;
            this.MainTabControl.CloseRect = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.MainTabControl.Controls.Add(this.skinTabPage1);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.HeadBack = null;
            this.MainTabControl.ImageList = this.imageList;
            this.MainTabControl.ImgSize = new System.Drawing.Size(48, 48);
            this.MainTabControl.ImgTxtOffset = new System.Drawing.Point(0, 0);
            this.MainTabControl.ItemSize = new System.Drawing.Size(120, 75);
            this.MainTabControl.Location = new System.Drawing.Point(3, 6);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.PageArrowDown = ((System.Drawing.Image)(resources.GetObject("MainTabControl.PageArrowDown")));
            this.MainTabControl.PageArrowHover = ((System.Drawing.Image)(resources.GetObject("MainTabControl.PageArrowHover")));
            this.MainTabControl.PageCloseHover = ((System.Drawing.Image)(resources.GetObject("MainTabControl.PageCloseHover")));
            this.MainTabControl.PageCloseNormal = ((System.Drawing.Image)(resources.GetObject("MainTabControl.PageCloseNormal")));
            this.MainTabControl.PageDown = ((System.Drawing.Image)(resources.GetObject("MainTabControl.PageDown")));
            this.MainTabControl.PageHover = ((System.Drawing.Image)(resources.GetObject("MainTabControl.PageHover")));
            this.MainTabControl.PageImagePosition = CCWin.SkinControl.SkinTabControl.ePageImagePosition.Top;
            this.MainTabControl.PageNorml = null;
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(986, 579);
            this.MainTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.MainTabControl.TabIndex = 0;
            // 
            // skinTabPage1
            // 
            this.skinTabPage1.BackColor = System.Drawing.Color.White;
            this.skinTabPage1.Controls.Add(this.tableLayoutPanel1);
            this.skinTabPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTabPage1.Font = new System.Drawing.Font("Baiduan Number", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skinTabPage1.ImageIndex = 1;
            this.skinTabPage1.Location = new System.Drawing.Point(0, 75);
            this.skinTabPage1.Name = "skinTabPage1";
            this.skinTabPage1.Size = new System.Drawing.Size(986, 504);
            this.skinTabPage1.TabIndex = 0;
            this.skinTabPage1.TabItemImage = null;
            this.skinTabPage1.Text = "体检报告";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ReportToolBar, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tlp_queryConditions, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgv_orders, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(986, 504);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ReportToolBar
            // 
            this.ReportToolBar.Arrow = System.Drawing.Color.Black;
            this.ReportToolBar.Back = System.Drawing.Color.White;
            this.ReportToolBar.BackRadius = 4;
            this.ReportToolBar.BackRectangle = new System.Drawing.Rectangle(10, 10, 10, 10);
            this.ReportToolBar.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.ReportToolBar.BaseFore = System.Drawing.Color.Black;
            this.ReportToolBar.BaseForeAnamorphosis = false;
            this.ReportToolBar.BaseForeAnamorphosisBorder = 4;
            this.ReportToolBar.BaseForeAnamorphosisColor = System.Drawing.Color.White;
            this.ReportToolBar.BaseForeOffset = new System.Drawing.Point(0, 0);
            this.ReportToolBar.BaseHoverFore = System.Drawing.Color.White;
            this.ReportToolBar.BaseItemAnamorphosis = true;
            this.ReportToolBar.BaseItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.ReportToolBar.BaseItemBorderShow = true;
            this.ReportToolBar.BaseItemDown = ((System.Drawing.Image)(resources.GetObject("ReportToolBar.BaseItemDown")));
            this.ReportToolBar.BaseItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.ReportToolBar.BaseItemMouse = ((System.Drawing.Image)(resources.GetObject("ReportToolBar.BaseItemMouse")));
            this.ReportToolBar.BaseItemNorml = null;
            this.ReportToolBar.BaseItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.ReportToolBar.BaseItemRadius = 4;
            this.ReportToolBar.BaseItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.ReportToolBar.BaseItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.ReportToolBar.BindTabControl = null;
            this.ReportToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReportToolBar.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.ReportToolBar.Fore = System.Drawing.Color.Black;
            this.ReportToolBar.GripMargin = new System.Windows.Forms.Padding(2, 2, 4, 2);
            this.ReportToolBar.HoverFore = System.Drawing.Color.White;
            this.ReportToolBar.ItemAnamorphosis = true;
            this.ReportToolBar.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.ReportToolBar.ItemBorderShow = true;
            this.ReportToolBar.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.ReportToolBar.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.ReportToolBar.ItemRadius = 4;
            this.ReportToolBar.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.ReportToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnQueryOrder,
            this.btnPrint});
            this.ReportToolBar.Location = new System.Drawing.Point(0, 0);
            this.ReportToolBar.Name = "ReportToolBar";
            this.ReportToolBar.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.ReportToolBar.Size = new System.Drawing.Size(986, 30);
            this.ReportToolBar.SkinAllColor = true;
            this.ReportToolBar.TabIndex = 3;
            this.ReportToolBar.Text = "skinToolStrip1";
            this.ReportToolBar.TitleAnamorphosis = true;
            this.ReportToolBar.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.ReportToolBar.TitleRadius = 4;
            this.ReportToolBar.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // btnQueryOrder
            // 
            this.btnQueryOrder.Image = ((System.Drawing.Image)(resources.GetObject("btnQueryOrder.Image")));
            this.btnQueryOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnQueryOrder.Name = "btnQueryOrder";
            this.btnQueryOrder.Size = new System.Drawing.Size(59, 27);
            this.btnQueryOrder.Text = "查询";
            this.btnQueryOrder.Click += new System.EventHandler(this.btnQueryOrder_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(59, 27);
            this.btnPrint.Text = "打印";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // tlp_queryConditions
            // 
            this.tlp_queryConditions.ColumnCount = 8;
            this.tlp_queryConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlp_queryConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp_queryConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlp_queryConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp_queryConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlp_queryConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp_queryConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlp_queryConditions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp_queryConditions.Controls.Add(this.dpSTo, 7, 1);
            this.tlp_queryConditions.Controls.Add(this.dpTo, 7, 0);
            this.tlp_queryConditions.Controls.Add(this.dpFrom, 5, 0);
            this.tlp_queryConditions.Controls.Add(this.skinLabel1, 0, 0);
            this.tlp_queryConditions.Controls.Add(this.skinLabel2, 0, 1);
            this.tlp_queryConditions.Controls.Add(this.skinLabel3, 0, 2);
            this.tlp_queryConditions.Controls.Add(this.skinLabel4, 2, 0);
            this.tlp_queryConditions.Controls.Add(this.skinLabel5, 2, 1);
            this.tlp_queryConditions.Controls.Add(this.skinLabel6, 2, 2);
            this.tlp_queryConditions.Controls.Add(this.skinLabel7, 4, 0);
            this.tlp_queryConditions.Controls.Add(this.skinLabel8, 4, 1);
            this.tlp_queryConditions.Controls.Add(this.skinLabel9, 6, 0);
            this.tlp_queryConditions.Controls.Add(this.skinLabel10, 6, 1);
            this.tlp_queryConditions.Controls.Add(this.dropDictLab, 1, 0);
            this.tlp_queryConditions.Controls.Add(this.dropStatus, 1, 1);
            this.tlp_queryConditions.Controls.Add(this.dropDictcustomer, 3, 0);
            this.tlp_queryConditions.Controls.Add(this.dropReportStatus, 3, 2);
            this.tlp_queryConditions.Controls.Add(this.tbxName, 1, 2);
            this.tlp_queryConditions.Controls.Add(this.tbxOrderNum, 3, 1);
            this.tlp_queryConditions.Controls.Add(this.dpSFrom, 5, 1);
            this.tlp_queryConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_queryConditions.Location = new System.Drawing.Point(3, 33);
            this.tlp_queryConditions.Name = "tlp_queryConditions";
            this.tlp_queryConditions.RowCount = 4;
            this.tlp_queryConditions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.tlp_queryConditions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlp_queryConditions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlp_queryConditions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tlp_queryConditions.Size = new System.Drawing.Size(980, 114);
            this.tlp_queryConditions.TabIndex = 4;
            // 
            // dpSTo
            // 
            this.dpSTo.CustomFormat = "yyyy-MM-dd";
            this.dpSTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpSTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpSTo.Location = new System.Drawing.Point(816, 39);
            this.dpSTo.Name = "dpSTo";
            this.dpSTo.Size = new System.Drawing.Size(161, 28);
            this.dpSTo.TabIndex = 23;
            this.dpSTo.Value = new System.DateTime(2016, 5, 7, 0, 0, 0, 0);
            // 
            // dpTo
            // 
            this.dpTo.CustomFormat = "yyyy-MM-dd";
            this.dpTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpTo.Location = new System.Drawing.Point(816, 3);
            this.dpTo.Name = "dpTo";
            this.dpTo.Size = new System.Drawing.Size(161, 28);
            this.dpTo.TabIndex = 22;
            this.dpTo.Value = new System.DateTime(2016, 5, 7, 0, 0, 0, 0);
            // 
            // dpFrom
            // 
            this.dpFrom.CustomFormat = "yyyy-MM-dd";
            this.dpFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpFrom.Location = new System.Drawing.Point(615, 3);
            this.dpFrom.Name = "dpFrom";
            this.dpFrom.Size = new System.Drawing.Size(160, 28);
            this.dpFrom.TabIndex = 21;
            this.dpFrom.Value = new System.DateTime(2016, 4, 1, 0, 0, 0, 0);
            // 
            // skinLabel1
            // 
            this.skinLabel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.skinLabel1.Location = new System.Drawing.Point(35, 8);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(42, 20);
            this.skinLabel1.TabIndex = 0;
            this.skinLabel1.Text = "分点:";
            // 
            // skinLabel2
            // 
            this.skinLabel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.skinLabel2.Location = new System.Drawing.Point(35, 44);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(42, 20);
            this.skinLabel2.TabIndex = 1;
            this.skinLabel2.Text = "状态:";
            // 
            // skinLabel3
            // 
            this.skinLabel3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.skinLabel3.AutoSize = true;
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.Color.White;
            this.skinLabel3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.skinLabel3.Location = new System.Drawing.Point(20, 80);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new System.Drawing.Size(57, 20);
            this.skinLabel3.TabIndex = 2;
            this.skinLabel3.Text = "关键词:";
            // 
            // skinLabel4
            // 
            this.skinLabel4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.skinLabel4.AutoSize = true;
            this.skinLabel4.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel4.BorderColor = System.Drawing.Color.White;
            this.skinLabel4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.skinLabel4.Location = new System.Drawing.Point(291, 8);
            this.skinLabel4.Name = "skinLabel4";
            this.skinLabel4.Size = new System.Drawing.Size(72, 20);
            this.skinLabel4.TabIndex = 3;
            this.skinLabel4.Text = "体检单位:";
            // 
            // skinLabel5
            // 
            this.skinLabel5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.skinLabel5.AutoSize = true;
            this.skinLabel5.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel5.BorderColor = System.Drawing.Color.White;
            this.skinLabel5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.skinLabel5.Location = new System.Drawing.Point(255, 44);
            this.skinLabel5.Name = "skinLabel5";
            this.skinLabel5.Size = new System.Drawing.Size(108, 20);
            this.skinLabel5.TabIndex = 4;
            this.skinLabel5.Text = "体检号/条形码:";
            // 
            // skinLabel6
            // 
            this.skinLabel6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.skinLabel6.AutoSize = true;
            this.skinLabel6.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel6.BorderColor = System.Drawing.Color.White;
            this.skinLabel6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.skinLabel6.Location = new System.Drawing.Point(291, 80);
            this.skinLabel6.Name = "skinLabel6";
            this.skinLabel6.Size = new System.Drawing.Size(72, 20);
            this.skinLabel6.TabIndex = 5;
            this.skinLabel6.Text = "报告状态:";
            // 
            // skinLabel7
            // 
            this.skinLabel7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.skinLabel7.AutoSize = true;
            this.skinLabel7.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel7.BorderColor = System.Drawing.Color.White;
            this.skinLabel7.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.skinLabel7.Location = new System.Drawing.Point(537, 8);
            this.skinLabel7.Name = "skinLabel7";
            this.skinLabel7.Size = new System.Drawing.Size(72, 20);
            this.skinLabel7.TabIndex = 6;
            this.skinLabel7.Text = "登记时间:";
            // 
            // skinLabel8
            // 
            this.skinLabel8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.skinLabel8.AutoSize = true;
            this.skinLabel8.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel8.BorderColor = System.Drawing.Color.White;
            this.skinLabel8.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.skinLabel8.Location = new System.Drawing.Point(537, 44);
            this.skinLabel8.Name = "skinLabel8";
            this.skinLabel8.Size = new System.Drawing.Size(72, 20);
            this.skinLabel8.TabIndex = 7;
            this.skinLabel8.Text = "采样时间:";
            // 
            // skinLabel9
            // 
            this.skinLabel9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.skinLabel9.AutoSize = true;
            this.skinLabel9.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel9.BorderColor = System.Drawing.Color.White;
            this.skinLabel9.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.skinLabel9.Location = new System.Drawing.Point(783, 8);
            this.skinLabel9.Name = "skinLabel9";
            this.skinLabel9.Size = new System.Drawing.Size(27, 20);
            this.skinLabel9.TabIndex = 8;
            this.skinLabel9.Text = "到:";
            // 
            // skinLabel10
            // 
            this.skinLabel10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.skinLabel10.AutoSize = true;
            this.skinLabel10.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel10.BorderColor = System.Drawing.Color.White;
            this.skinLabel10.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.skinLabel10.Location = new System.Drawing.Point(783, 44);
            this.skinLabel10.Name = "skinLabel10";
            this.skinLabel10.Size = new System.Drawing.Size(27, 20);
            this.skinLabel10.TabIndex = 9;
            this.skinLabel10.Text = "到:";
            // 
            // dropDictLab
            // 
            this.dropDictLab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dropDictLab.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropDictLab.FormattingEnabled = true;
            this.dropDictLab.Location = new System.Drawing.Point(83, 3);
            this.dropDictLab.Name = "dropDictLab";
            this.dropDictLab.Size = new System.Drawing.Size(160, 29);
            this.dropDictLab.TabIndex = 10;
            this.dropDictLab.WaterText = "";
            // 
            // dropStatus
            // 
            this.dropStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dropStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropStatus.FormattingEnabled = true;
            this.dropStatus.Location = new System.Drawing.Point(83, 39);
            this.dropStatus.Name = "dropStatus";
            this.dropStatus.Size = new System.Drawing.Size(160, 29);
            this.dropStatus.TabIndex = 11;
            this.dropStatus.WaterText = "";
            // 
            // dropDictcustomer
            // 
            this.dropDictcustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dropDictcustomer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropDictcustomer.FormattingEnabled = true;
            this.dropDictcustomer.Location = new System.Drawing.Point(369, 3);
            this.dropDictcustomer.Name = "dropDictcustomer";
            this.dropDictcustomer.Size = new System.Drawing.Size(160, 29);
            this.dropDictcustomer.TabIndex = 12;
            this.dropDictcustomer.WaterText = "";
            // 
            // dropReportStatus
            // 
            this.dropReportStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dropReportStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropReportStatus.FormattingEnabled = true;
            this.dropReportStatus.Location = new System.Drawing.Point(369, 75);
            this.dropReportStatus.Name = "dropReportStatus";
            this.dropReportStatus.Size = new System.Drawing.Size(160, 29);
            this.dropReportStatus.TabIndex = 13;
            this.dropReportStatus.WaterText = "";
            // 
            // tbxName
            // 
            this.tbxName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxName.BackColor = System.Drawing.Color.Transparent;
            this.tbxName.DownBack = null;
            this.tbxName.Icon = null;
            this.tbxName.IconIsButton = false;
            this.tbxName.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbxName.IsPasswordChat = '\0';
            this.tbxName.IsSystemPasswordChar = false;
            this.tbxName.Lines = new string[0];
            this.tbxName.Location = new System.Drawing.Point(80, 76);
            this.tbxName.Margin = new System.Windows.Forms.Padding(0);
            this.tbxName.MaxLength = 32767;
            this.tbxName.MinimumSize = new System.Drawing.Size(28, 28);
            this.tbxName.MouseBack = null;
            this.tbxName.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbxName.Multiline = false;
            this.tbxName.Name = "tbxName";
            this.tbxName.NormlBack = null;
            this.tbxName.Padding = new System.Windows.Forms.Padding(5);
            this.tbxName.ReadOnly = false;
            this.tbxName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbxName.Size = new System.Drawing.Size(166, 28);
            // 
            // 
            // 
            this.tbxName.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbxName.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxName.SkinTxt.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F);
            this.tbxName.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.tbxName.SkinTxt.Name = "BaseText";
            this.tbxName.SkinTxt.Size = new System.Drawing.Size(156, 22);
            this.tbxName.SkinTxt.TabIndex = 0;
            this.tbxName.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbxName.SkinTxt.WaterText = "";
            this.tbxName.TabIndex = 18;
            this.tbxName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbxName.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbxName.WaterText = "";
            this.tbxName.WordWrap = true;
            // 
            // tbxOrderNum
            // 
            this.tbxOrderNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxOrderNum.BackColor = System.Drawing.Color.Transparent;
            this.tbxOrderNum.DownBack = null;
            this.tbxOrderNum.Icon = null;
            this.tbxOrderNum.IconIsButton = false;
            this.tbxOrderNum.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbxOrderNum.IsPasswordChat = '\0';
            this.tbxOrderNum.IsSystemPasswordChar = false;
            this.tbxOrderNum.Lines = new string[0];
            this.tbxOrderNum.Location = new System.Drawing.Point(366, 39);
            this.tbxOrderNum.Margin = new System.Windows.Forms.Padding(0);
            this.tbxOrderNum.MaxLength = 32767;
            this.tbxOrderNum.MinimumSize = new System.Drawing.Size(28, 28);
            this.tbxOrderNum.MouseBack = null;
            this.tbxOrderNum.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbxOrderNum.Multiline = true;
            this.tbxOrderNum.Name = "tbxOrderNum";
            this.tbxOrderNum.NormlBack = null;
            this.tbxOrderNum.Padding = new System.Windows.Forms.Padding(5);
            this.tbxOrderNum.ReadOnly = false;
            this.tbxOrderNum.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbxOrderNum.Size = new System.Drawing.Size(166, 29);
            // 
            // 
            // 
            this.tbxOrderNum.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbxOrderNum.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxOrderNum.SkinTxt.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F);
            this.tbxOrderNum.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.tbxOrderNum.SkinTxt.Multiline = true;
            this.tbxOrderNum.SkinTxt.Name = "BaseText";
            this.tbxOrderNum.SkinTxt.Size = new System.Drawing.Size(156, 19);
            this.tbxOrderNum.SkinTxt.TabIndex = 0;
            this.tbxOrderNum.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbxOrderNum.SkinTxt.WaterText = "";
            this.tbxOrderNum.TabIndex = 19;
            this.tbxOrderNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbxOrderNum.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbxOrderNum.WaterText = "";
            this.tbxOrderNum.WordWrap = true;
            // 
            // dpSFrom
            // 
            this.dpSFrom.CustomFormat = "yyyy-MM-dd";
            this.dpSFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpSFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpSFrom.Location = new System.Drawing.Point(615, 39);
            this.dpSFrom.Name = "dpSFrom";
            this.dpSFrom.Size = new System.Drawing.Size(160, 28);
            this.dpSFrom.TabIndex = 20;
            this.dpSFrom.Value = new System.DateTime(2016, 4, 1, 0, 0, 0, 0);
            // 
            // dgv_orders
            // 
            this.dgv_orders.AllowUserToAddRows = false;
            this.dgv_orders.AllowUserToDeleteRows = false;
            this.dgv_orders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_orders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_orders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrderStatus,
            this.Cell_OrderNumber,
            this.Cell_FullName,
            this.Cell_Sex,
            this.Cell_Age,
            this.Cell_Mobile,
            this.Cell_CreateDate,
            this.Cell_OrganizationName,
            this.Cell_OrderPackageName,
            this.Cell_Section,
            this.Cell_SamplingDate,
            this.Cell_PostAddress,
            this.Cell_Recipient,
            this.Cell_ContractNumber});
            this.dgv_orders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_orders.Location = new System.Drawing.Point(3, 153);
            this.dgv_orders.Name = "dgv_orders";
            this.dgv_orders.ReadOnly = true;
            this.dgv_orders.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_orders.RowTemplate.Height = 24;
            this.dgv_orders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_orders.Size = new System.Drawing.Size(980, 348);
            this.dgv_orders.TabIndex = 5;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "AdvanceSearch.png");
            this.imageList.Images.SetKeyName(1, "ico_print.png");
            this.imageList.Images.SetKeyName(2, "Symbol-Check.png");
            this.imageList.Images.SetKeyName(3, "System_Module.png");
            // 
            // OrderStatus
            // 
            this.OrderStatus.DataPropertyName = "STATUSNAME";
            this.OrderStatus.HeaderText = "订单状态";
            this.OrderStatus.Name = "OrderStatus";
            this.OrderStatus.ReadOnly = true;
            this.OrderStatus.Width = 76;
            // 
            // Cell_OrderNumber
            // 
            this.Cell_OrderNumber.DataPropertyName = "ORDERNUM";
            this.Cell_OrderNumber.HeaderText = "订单号";
            this.Cell_OrderNumber.Name = "Cell_OrderNumber";
            this.Cell_OrderNumber.ReadOnly = true;
            this.Cell_OrderNumber.Width = 76;
            // 
            // Cell_FullName
            // 
            this.Cell_FullName.DataPropertyName = "REALNAME";
            this.Cell_FullName.HeaderText = "姓名";
            this.Cell_FullName.Name = "Cell_FullName";
            this.Cell_FullName.ReadOnly = true;
            this.Cell_FullName.Width = 61;
            // 
            // Cell_Sex
            // 
            this.Cell_Sex.DataPropertyName = "SEX";
            this.Cell_Sex.HeaderText = "性别";
            this.Cell_Sex.Name = "Cell_Sex";
            this.Cell_Sex.ReadOnly = true;
            this.Cell_Sex.Width = 61;
            // 
            // Cell_Age
            // 
            this.Cell_Age.DataPropertyName = "AGE";
            this.Cell_Age.HeaderText = "年龄";
            this.Cell_Age.Name = "Cell_Age";
            this.Cell_Age.ReadOnly = true;
            this.Cell_Age.Width = 61;
            // 
            // Cell_Mobile
            // 
            this.Cell_Mobile.DataPropertyName = "MOBILE";
            this.Cell_Mobile.HeaderText = "联系方式";
            this.Cell_Mobile.Name = "Cell_Mobile";
            this.Cell_Mobile.ReadOnly = true;
            this.Cell_Mobile.Width = 76;
            // 
            // Cell_CreateDate
            // 
            this.Cell_CreateDate.DataPropertyName = "createdate";
            this.Cell_CreateDate.HeaderText = "登记时间";
            this.Cell_CreateDate.Name = "Cell_CreateDate";
            this.Cell_CreateDate.ReadOnly = true;
            this.Cell_CreateDate.Width = 76;
            this.Cell_CreateDate.DefaultCellStyle.Format = "yyyy-MM-dd";
            // 
            // Cell_OrganizationName
            // 
            this.Cell_OrganizationName.DataPropertyName = "CUSTOMERNAME";
            this.Cell_OrganizationName.HeaderText = "体检单位";
            this.Cell_OrganizationName.Name = "Cell_OrganizationName";
            this.Cell_OrganizationName.ReadOnly = true;
            this.Cell_OrganizationName.Width = 76;
            // 
            // Cell_OrderPackageName
            // 
            this.Cell_OrderPackageName.DataPropertyName = "ORDERTESTLST";
            this.Cell_OrderPackageName.HeaderText = "套餐名称";
            this.Cell_OrderPackageName.Name = "Cell_OrderPackageName";
            this.Cell_OrderPackageName.ReadOnly = true;
            this.Cell_OrderPackageName.Width = 76;
            // 
            // Cell_Section
            // 
            this.Cell_Section.DataPropertyName = "section";
            this.Cell_Section.HeaderText = "部门[地区]";
            this.Cell_Section.Name = "Cell_Section";
            this.Cell_Section.ReadOnly = true;
            this.Cell_Section.Width = 79;
            // 
            // Cell_SamplingDate
            // 
            this.Cell_SamplingDate.DataPropertyName = "samplingdate";
            this.Cell_SamplingDate.HeaderText = "采样时间";
            this.Cell_SamplingDate.Name = "Cell_SamplingDate";
            this.Cell_SamplingDate.ReadOnly = true;
            this.Cell_SamplingDate.Width = 76;
            this.Cell_SamplingDate.DefaultCellStyle.Format = "yyyy-MM-dd";
            // 
            // Cell_PostAddress
            // 
            this.Cell_PostAddress.DataPropertyName = "POSTADDRESS";
            this.Cell_PostAddress.HeaderText = "邮寄地址";
            this.Cell_PostAddress.Name = "Cell_PostAddress";
            this.Cell_PostAddress.ReadOnly = true;
            this.Cell_PostAddress.Width = 76;
            // 
            // Cell_Recipient
            // 
            this.Cell_Recipient.DataPropertyName = "RECIPIENT";
            this.Cell_Recipient.HeaderText = "收件人";
            this.Cell_Recipient.Name = "Cell_Recipient";
            this.Cell_Recipient.ReadOnly = true;
            this.Cell_Recipient.Width = 76;
            // 
            // Cell_ContractNumber
            // 
            this.Cell_ContractNumber.DataPropertyName = "CONTACTNUMBER";
            this.Cell_ContractNumber.HeaderText = "联系电话";
            this.Cell_ContractNumber.Name = "Cell_ContractNumber";
            this.Cell_ContractNumber.ReadOnly = true;
            this.Cell_ContractNumber.Width = 76;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印工具";
            this.Load += new System.EventHandler(this.MainFormTabImpl_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.MainTabControl.ResumeLayout(false);
            this.skinTabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ReportToolBar.ResumeLayout(false);
            this.ReportToolBar.PerformLayout();
            this.tlp_queryConditions.ResumeLayout(false);
            this.tlp_queryConditions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_orders)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private CCWin.SkinControl.SkinTabControl MainTabControl;
        private CCWin.SkinControl.SkinTabPage skinTabPage1;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CCWin.SkinControl.SkinToolStrip ReportToolBar;
        private System.Windows.Forms.ToolStripButton btnQueryOrder;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.TableLayoutPanel tlp_queryConditions;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private CCWin.SkinControl.SkinLabel skinLabel4;
        private CCWin.SkinControl.SkinLabel skinLabel5;
        private CCWin.SkinControl.SkinLabel skinLabel6;
        private CCWin.SkinControl.SkinLabel skinLabel7;
        private CCWin.SkinControl.SkinLabel skinLabel8;
        private CCWin.SkinControl.SkinLabel skinLabel9;
        private CCWin.SkinControl.SkinLabel skinLabel10;
        private CCWin.SkinControl.SkinComboBox dropDictLab;
        private CCWin.SkinControl.SkinComboBox dropStatus;
        private CCWin.SkinControl.SkinComboBox dropDictcustomer;
        private CCWin.SkinControl.SkinComboBox dropReportStatus;
        private CCWin.SkinControl.SkinTextBox tbxName;
        private CCWin.SkinControl.SkinTextBox tbxOrderNum;
        private System.Windows.Forms.DateTimePicker dpSFrom;
        private System.Windows.Forms.DateTimePicker dpSTo;
        private System.Windows.Forms.DateTimePicker dpTo;
        private System.Windows.Forms.DateTimePicker dpFrom;
        private System.Windows.Forms.DataGridView dgv_orders;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_OrderNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_FullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_Sex;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_Age;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_Mobile;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_CreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_OrganizationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_OrderPackageName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_Section;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_SamplingDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_PostAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_Recipient;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell_ContractNumber;
    }
}