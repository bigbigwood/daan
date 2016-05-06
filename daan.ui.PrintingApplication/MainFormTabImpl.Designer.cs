namespace daan.ui.PrintingApplication
{
    partial class MainFormTabImpl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormTabImpl));
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MainTabControl = new CCWin.SkinControl.SkinTabControl();
            this.skinTabPage1 = new CCWin.SkinControl.SkinTabPage();
            this.skinTabPage3 = new CCWin.SkinControl.SkinTabPage();
            this.skinTabPage4 = new CCWin.SkinControl.SkinTabPage();
            this.skinTabPage2 = new CCWin.SkinControl.SkinTabPage();
            this.tab_Settings = new CCWin.SkinControl.SkinTabControl();
            this.tabPage_PrinterSetting = new CCWin.SkinControl.SkinTabPage();
            this.tabPage_SystemSetting = new CCWin.SkinControl.SkinTabPage();
            this.skinTabPage5 = new CCWin.SkinControl.SkinTabPage();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.MainTableLayoutPanel.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.skinTabPage2.SuspendLayout();
            this.tab_Settings.SuspendLayout();
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
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(992, 518);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // MainTabControl
            // 
            this.MainTabControl.AnimatorType = CCWin.SkinControl.AnimationType.HorizSlide;
            this.MainTabControl.CloseRect = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.MainTabControl.Controls.Add(this.skinTabPage1);
            this.MainTabControl.Controls.Add(this.skinTabPage3);
            this.MainTabControl.Controls.Add(this.skinTabPage4);
            this.MainTabControl.Controls.Add(this.skinTabPage2);
            this.MainTabControl.Controls.Add(this.skinTabPage5);
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
            this.MainTabControl.SelectedIndex = 3;
            this.MainTabControl.Size = new System.Drawing.Size(986, 479);
            this.MainTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.MainTabControl.TabIndex = 0;
            // 
            // skinTabPage1
            // 
            this.skinTabPage1.BackColor = System.Drawing.Color.White;
            this.skinTabPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTabPage1.Font = new System.Drawing.Font("Baiduan Number", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skinTabPage1.ImageIndex = 1;
            this.skinTabPage1.Location = new System.Drawing.Point(0, 75);
            this.skinTabPage1.Name = "skinTabPage1";
            this.skinTabPage1.Size = new System.Drawing.Size(986, 404);
            this.skinTabPage1.TabIndex = 0;
            this.skinTabPage1.TabItemImage = null;
            this.skinTabPage1.Text = "体检报告";
            // 
            // skinTabPage3
            // 
            this.skinTabPage3.BackColor = System.Drawing.Color.White;
            this.skinTabPage3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTabPage3.ImageIndex = 0;
            this.skinTabPage3.Location = new System.Drawing.Point(0, 75);
            this.skinTabPage3.Name = "skinTabPage3";
            this.skinTabPage3.Size = new System.Drawing.Size(986, 404);
            this.skinTabPage3.TabIndex = 2;
            this.skinTabPage3.TabItemImage = null;
            this.skinTabPage3.Text = "易感基因检测";
            // 
            // skinTabPage4
            // 
            this.skinTabPage4.BackColor = System.Drawing.Color.White;
            this.skinTabPage4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTabPage4.ImageIndex = 2;
            this.skinTabPage4.Location = new System.Drawing.Point(0, 75);
            this.skinTabPage4.Name = "skinTabPage4";
            this.skinTabPage4.Size = new System.Drawing.Size(986, 404);
            this.skinTabPage4.TabIndex = 3;
            this.skinTabPage4.TabItemImage = null;
            this.skinTabPage4.Text = "条形码";
            // 
            // skinTabPage2
            // 
            this.skinTabPage2.BackColor = System.Drawing.Color.White;
            this.skinTabPage2.Controls.Add(this.tab_Settings);
            this.skinTabPage2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTabPage2.Font = new System.Drawing.Font("Baiduan Number", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skinTabPage2.ImageIndex = 3;
            this.skinTabPage2.Location = new System.Drawing.Point(0, 75);
            this.skinTabPage2.Name = "skinTabPage2";
            this.skinTabPage2.Size = new System.Drawing.Size(986, 404);
            this.skinTabPage2.TabIndex = 1;
            this.skinTabPage2.TabItemImage = null;
            this.skinTabPage2.Text = "设置";
            // 
            // tab_Settings
            // 
            this.tab_Settings.AnimatorType = CCWin.SkinControl.AnimationType.HorizSlide;
            this.tab_Settings.CloseRect = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.tab_Settings.Controls.Add(this.tabPage_PrinterSetting);
            this.tab_Settings.Controls.Add(this.tabPage_SystemSetting);
            this.tab_Settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_Settings.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.tab_Settings.HeadBack = null;
            this.tab_Settings.ImgTxtOffset = new System.Drawing.Point(0, 0);
            this.tab_Settings.ItemSize = new System.Drawing.Size(120, 30);
            this.tab_Settings.Location = new System.Drawing.Point(0, 0);
            this.tab_Settings.Name = "tab_Settings";
            this.tab_Settings.PageArrowDown = ((System.Drawing.Image)(resources.GetObject("tab_Settings.PageArrowDown")));
            this.tab_Settings.PageArrowHover = ((System.Drawing.Image)(resources.GetObject("tab_Settings.PageArrowHover")));
            this.tab_Settings.PageBorderColor = System.Drawing.Color.Black;
            this.tab_Settings.PageCloseHover = ((System.Drawing.Image)(resources.GetObject("tab_Settings.PageCloseHover")));
            this.tab_Settings.PageCloseNormal = ((System.Drawing.Image)(resources.GetObject("tab_Settings.PageCloseNormal")));
            this.tab_Settings.PageDown = ((System.Drawing.Image)(resources.GetObject("tab_Settings.PageDown")));
            this.tab_Settings.PageHover = ((System.Drawing.Image)(resources.GetObject("tab_Settings.PageHover")));
            this.tab_Settings.PageImagePosition = CCWin.SkinControl.SkinTabControl.ePageImagePosition.Left;
            this.tab_Settings.PageNorml = null;
            this.tab_Settings.PagePalace = true;
            this.tab_Settings.SelectedIndex = 0;
            this.tab_Settings.Size = new System.Drawing.Size(986, 404);
            this.tab_Settings.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tab_Settings.TabIndex = 0;
            this.tab_Settings.SelectedIndexChanged += new System.EventHandler(this.tab_Settings_SelectedIndexChanged);
            // 
            // tabPage_PrinterSetting
            // 
            this.tabPage_PrinterSetting.BackColor = System.Drawing.Color.White;
            this.tabPage_PrinterSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPage_PrinterSetting.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.tabPage_PrinterSetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            this.tabPage_PrinterSetting.Location = new System.Drawing.Point(0, 30);
            this.tabPage_PrinterSetting.Name = "tabPage_PrinterSetting";
            this.tabPage_PrinterSetting.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabPage_PrinterSetting.Size = new System.Drawing.Size(986, 374);
            this.tabPage_PrinterSetting.TabIndex = 0;
            this.tabPage_PrinterSetting.TabItemImage = null;
            this.tabPage_PrinterSetting.Text = "打印机设置";
            // 
            // tabPage_SystemSetting
            // 
            this.tabPage_SystemSetting.BackColor = System.Drawing.Color.White;
            this.tabPage_SystemSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPage_SystemSetting.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.tabPage_SystemSetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            this.tabPage_SystemSetting.Location = new System.Drawing.Point(0, 30);
            this.tabPage_SystemSetting.Name = "tabPage_SystemSetting";
            this.tabPage_SystemSetting.Size = new System.Drawing.Size(986, 374);
            this.tabPage_SystemSetting.TabIndex = 1;
            this.tabPage_SystemSetting.TabItemImage = null;
            this.tabPage_SystemSetting.Text = "系统设置";
            // 
            // skinTabPage5
            // 
            this.skinTabPage5.BackColor = System.Drawing.Color.White;
            this.skinTabPage5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTabPage5.ImageIndex = 0;
            this.skinTabPage5.Location = new System.Drawing.Point(0, 75);
            this.skinTabPage5.Name = "skinTabPage5";
            this.skinTabPage5.Size = new System.Drawing.Size(986, 404);
            this.skinTabPage5.TabIndex = 4;
            this.skinTabPage5.TabItemImage = null;
            this.skinTabPage5.Text = "帮助";
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
            // MainFormTabImpl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 550);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "MainFormTabImpl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainFormTabImpl";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainFormTabImpl_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.MainTabControl.ResumeLayout(false);
            this.skinTabPage2.ResumeLayout(false);
            this.tab_Settings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private CCWin.SkinControl.SkinTabControl MainTabControl;
        private CCWin.SkinControl.SkinTabPage skinTabPage1;
        private CCWin.SkinControl.SkinTabPage skinTabPage2;
        private System.Windows.Forms.ImageList imageList;
        private CCWin.SkinControl.SkinTabPage skinTabPage3;
        private CCWin.SkinControl.SkinTabPage skinTabPage4;
        private CCWin.SkinControl.SkinTabPage skinTabPage5;
        private CCWin.SkinControl.SkinTabControl tab_Settings;
        private CCWin.SkinControl.SkinTabPage tabPage_PrinterSetting;
        private CCWin.SkinControl.SkinTabPage tabPage_SystemSetting;
    }
}