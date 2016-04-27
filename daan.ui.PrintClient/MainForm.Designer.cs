namespace daan.ui.PrinterApplication
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.skinFlowLayoutPanel1 = new CCWin.SkinControl.SkinFlowLayoutPanel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.skinSplitContainer1 = new CCWin.SkinControl.SkinSplitContainer();
            this.skinPushPanel1 = new CCWin.SkinControl.SkinPushPanel();
            this.pushPanelItem_PrintList = new CCWin.SkinControl.PushPanelItem();
            this.pushPanelItem_Setting = new CCWin.SkinControl.PushPanelItem();
            this.pushPanelItem3 = new CCWin.SkinControl.PushPanelItem();
            this.MainTab = new CCWin.SkinControl.SkinTabControl();
            this.skinTabPage1 = new CCWin.SkinControl.SkinTabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblVersion = new CCWin.SkinControl.SkinLabel();
            this.lbl_Datetime = new CCWin.SkinControl.SkinLabel();
            this.skinTabPage2 = new CCWin.SkinControl.SkinTabPage();
            this.skinTabPage3 = new CCWin.SkinControl.SkinTabPage();
            this.skinFlowLayoutPanel2 = new CCWin.SkinControl.SkinFlowLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.skinSplitContainer1)).BeginInit();
            this.skinSplitContainer1.Panel1.SuspendLayout();
            this.skinSplitContainer1.Panel2.SuspendLayout();
            this.skinSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.skinPushPanel1)).BeginInit();
            this.skinPushPanel1.SuspendLayout();
            this.pushPanelItem_PrintList.SuspendLayout();
            this.MainTab.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinFlowLayoutPanel1
            // 
            this.skinFlowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.skinFlowLayoutPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("skinFlowLayoutPanel1.BackgroundImage")));
            this.skinFlowLayoutPanel1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinFlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinFlowLayoutPanel1.DownBack = null;
            this.skinFlowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.skinFlowLayoutPanel1.MouseBack = null;
            this.skinFlowLayoutPanel1.Name = "skinFlowLayoutPanel1";
            this.skinFlowLayoutPanel1.NormlBack = null;
            this.skinFlowLayoutPanel1.Size = new System.Drawing.Size(968, 59);
            this.skinFlowLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.skinFlowLayoutPanel1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.skinSplitContainer1, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(4, 28);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(974, 523);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // skinSplitContainer1
            // 
            this.skinSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.skinSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinSplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.skinSplitContainer1.Location = new System.Drawing.Point(3, 68);
            this.skinSplitContainer1.Name = "skinSplitContainer1";
            // 
            // skinSplitContainer1.Panel1
            // 
            this.skinSplitContainer1.Panel1.Controls.Add(this.skinPushPanel1);
            // 
            // skinSplitContainer1.Panel2
            // 
            this.skinSplitContainer1.Panel2.Controls.Add(this.MainTab);
            this.skinSplitContainer1.Size = new System.Drawing.Size(968, 417);
            this.skinSplitContainer1.SplitterDistance = 201;
            this.skinSplitContainer1.TabIndex = 3;
            // 
            // skinPushPanel1
            // 
            this.skinPushPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinPushPanel1.Items.AddRange(new CCWin.SkinControl.PushPanelItem[] {
            this.pushPanelItem_PrintList,
            this.pushPanelItem_Setting,
            this.pushPanelItem3});
            this.skinPushPanel1.Location = new System.Drawing.Point(0, 0);
            this.skinPushPanel1.Name = "skinPushPanel1";
            this.skinPushPanel1.Size = new System.Drawing.Size(201, 417);
            this.skinPushPanel1.TabIndex = 0;
            // 
            // pushPanelItem_PrintList
            // 
            this.pushPanelItem_PrintList.CaptionFont = new System.Drawing.Font("Segoe UI", 9F);
            this.pushPanelItem_PrintList.Controls.Add(this.skinFlowLayoutPanel2);
            this.pushPanelItem_PrintList.Name = "pushPanelItem_PrintList";
            this.pushPanelItem_PrintList.Text = "Print Function";
            // 
            // pushPanelItem_Setting
            // 
            this.pushPanelItem_Setting.CaptionFont = new System.Drawing.Font("Segoe UI", 9F);
            this.pushPanelItem_Setting.Name = "pushPanelItem_Setting";
            this.pushPanelItem_Setting.Text = "User Setting";
            // 
            // pushPanelItem3
            // 
            this.pushPanelItem3.CaptionFont = new System.Drawing.Font("Segoe UI", 9F);
            this.pushPanelItem3.Name = "pushPanelItem3";
            this.pushPanelItem3.Text = "System Setting";
            // 
            // MainTab
            // 
            this.MainTab.AnimatorType = CCWin.SkinControl.AnimationType.HorizSlide;
            this.MainTab.CloseRect = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.MainTab.Controls.Add(this.skinTabPage1);
            this.MainTab.Controls.Add(this.skinTabPage2);
            this.MainTab.Controls.Add(this.skinTabPage3);
            this.MainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTab.HeadBack = null;
            this.MainTab.ImgTxtOffset = new System.Drawing.Point(0, 0);
            this.MainTab.ItemSize = new System.Drawing.Size(70, 36);
            this.MainTab.Location = new System.Drawing.Point(0, 0);
            this.MainTab.Name = "MainTab";
            this.MainTab.PageArrowDown = ((System.Drawing.Image)(resources.GetObject("MainTab.PageArrowDown")));
            this.MainTab.PageArrowHover = ((System.Drawing.Image)(resources.GetObject("MainTab.PageArrowHover")));
            this.MainTab.PageCloseHover = ((System.Drawing.Image)(resources.GetObject("MainTab.PageCloseHover")));
            this.MainTab.PageCloseNormal = ((System.Drawing.Image)(resources.GetObject("MainTab.PageCloseNormal")));
            this.MainTab.PageDown = ((System.Drawing.Image)(resources.GetObject("MainTab.PageDown")));
            this.MainTab.PageHover = ((System.Drawing.Image)(resources.GetObject("MainTab.PageHover")));
            this.MainTab.PageImagePosition = CCWin.SkinControl.SkinTabControl.ePageImagePosition.Left;
            this.MainTab.PageNorml = null;
            this.MainTab.SelectedIndex = 2;
            this.MainTab.Size = new System.Drawing.Size(763, 417);
            this.MainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.MainTab.TabIndex = 0;
            // 
            // skinTabPage1
            // 
            this.skinTabPage1.BackColor = System.Drawing.Color.White;
            this.skinTabPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTabPage1.Location = new System.Drawing.Point(0, 36);
            this.skinTabPage1.Name = "skinTabPage1";
            this.skinTabPage1.Size = new System.Drawing.Size(763, 381);
            this.skinTabPage1.TabIndex = 0;
            this.skinTabPage1.TabItemImage = null;
            this.skinTabPage1.Text = "skinTabPage1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.DarkGray;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Controls.Add(this.lblVersion, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Datetime, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 491);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(968, 29);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.BorderColor = System.Drawing.Color.White;
            this.lblVersion.BorderSize = 0;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblVersion.ForeColor = System.Drawing.SystemColors.Control;
            this.lblVersion.Location = new System.Drawing.Point(849, 5);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(116, 19);
            this.lblVersion.TabIndex = 0;
            this.lblVersion.Text = "Version 1.0.0.0";
            // 
            // lbl_Datetime
            // 
            this.lbl_Datetime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_Datetime.AutoSize = true;
            this.lbl_Datetime.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Datetime.BorderColor = System.Drawing.Color.White;
            this.lbl_Datetime.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Datetime.Location = new System.Drawing.Point(668, 4);
            this.lbl_Datetime.Name = "lbl_Datetime";
            this.lbl_Datetime.Size = new System.Drawing.Size(0, 20);
            this.lbl_Datetime.TabIndex = 1;
            // 
            // skinTabPage2
            // 
            this.skinTabPage2.BackColor = System.Drawing.Color.White;
            this.skinTabPage2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTabPage2.Location = new System.Drawing.Point(0, 36);
            this.skinTabPage2.Name = "skinTabPage2";
            this.skinTabPage2.Size = new System.Drawing.Size(763, 381);
            this.skinTabPage2.TabIndex = 1;
            this.skinTabPage2.TabItemImage = null;
            this.skinTabPage2.Text = "skinTabPage2";
            // 
            // skinTabPage3
            // 
            this.skinTabPage3.BackColor = System.Drawing.Color.White;
            this.skinTabPage3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTabPage3.Location = new System.Drawing.Point(0, 36);
            this.skinTabPage3.Name = "skinTabPage3";
            this.skinTabPage3.Size = new System.Drawing.Size(763, 381);
            this.skinTabPage3.TabIndex = 2;
            this.skinTabPage3.TabItemImage = null;
            this.skinTabPage3.Text = "skinTabPage3";
            this.skinTabPage3.Click += new System.EventHandler(this.skinTabPage3_Click);
            // 
            // skinFlowLayoutPanel2
            // 
            this.skinFlowLayoutPanel2.AutoScroll = true;
            this.skinFlowLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.skinFlowLayoutPanel2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinFlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinFlowLayoutPanel2.DownBack = null;
            this.skinFlowLayoutPanel2.Location = new System.Drawing.Point(2, 24);
            this.skinFlowLayoutPanel2.MouseBack = null;
            this.skinFlowLayoutPanel2.Name = "skinFlowLayoutPanel2";
            this.skinFlowLayoutPanel2.NormlBack = null;
            this.skinFlowLayoutPanel2.Size = new System.Drawing.Size(193, 335);
            this.skinFlowLayoutPanel2.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 555);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Daan Printer Application";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.skinSplitContainer1.Panel1.ResumeLayout(false);
            this.skinSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.skinSplitContainer1)).EndInit();
            this.skinSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.skinPushPanel1)).EndInit();
            this.skinPushPanel1.ResumeLayout(false);
            this.pushPanelItem_PrintList.ResumeLayout(false);
            this.MainTab.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinFlowLayoutPanel skinFlowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private CCWin.SkinControl.SkinSplitContainer skinSplitContainer1;
        private CCWin.SkinControl.SkinTabControl MainTab;
        private CCWin.SkinControl.SkinTabPage skinTabPage1;
        private CCWin.SkinControl.SkinPushPanel skinPushPanel1;
        private CCWin.SkinControl.PushPanelItem pushPanelItem_PrintList;
        private CCWin.SkinControl.PushPanelItem pushPanelItem_Setting;
        private CCWin.SkinControl.PushPanelItem pushPanelItem3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CCWin.SkinControl.SkinLabel lblVersion;
        private CCWin.SkinControl.SkinLabel lbl_Datetime;
        private CCWin.SkinControl.SkinTabPage skinTabPage2;
        private CCWin.SkinControl.SkinTabPage skinTabPage3;
        private CCWin.SkinControl.SkinFlowLayoutPanel skinFlowLayoutPanel2;




    }
}