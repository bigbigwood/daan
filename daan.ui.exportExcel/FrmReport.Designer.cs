namespace daan.ui.exportExcel
{
    partial class FrmReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReport));
            this.btnExit = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSection = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dateEnd = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dateStart = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.comCustomer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comLab = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gridStat = new System.Windows.Forms.DataGridView();
            this.radTM = new System.Windows.Forms.RadioButton();
            this.radC14 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radAll = new System.Windows.Forms.RadioButton();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.ckPrinted = new System.Windows.Forms.CheckBox();
            this.ckXianChang = new System.Windows.Forms.CheckBox();
            this.ckChangeStatus = new System.Windows.Forms.CheckBox();
            this.订单状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.分点 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.section = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordernum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customername = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordertestlst = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridStat)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(926, 81);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 26;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(836, 81);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 25;
            this.btnExport.Text = "生成PDF";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(746, 81);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 24;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSection
            // 
            this.txtSection.Location = new System.Drawing.Point(801, 11);
            this.txtSection.Name = "txtSection";
            this.txtSection.Size = new System.Drawing.Size(189, 21);
            this.txtSection.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(712, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 12);
            this.label5.TabIndex = 22;
            this.label5.Text = "区域/体检号：";
            // 
            // dateEnd
            // 
            this.dateEnd.Location = new System.Drawing.Point(205, 46);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.Size = new System.Drawing.Size(114, 21);
            this.dateEnd.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(182, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "至：";
            // 
            // dateStart
            // 
            this.dateStart.Location = new System.Drawing.Point(63, 46);
            this.dateStart.Name = "dateStart";
            this.dateStart.Size = new System.Drawing.Size(113, 21);
            this.dateStart.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "登记时间：";
            // 
            // comCustomer
            // 
            this.comCustomer.FormattingEnabled = true;
            this.comCustomer.Location = new System.Drawing.Point(382, 11);
            this.comCustomer.Name = "comCustomer";
            this.comCustomer.Size = new System.Drawing.Size(311, 20);
            this.comCustomer.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(347, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "单位：";
            // 
            // comLab
            // 
            this.comLab.FormattingEnabled = true;
            this.comLab.Location = new System.Drawing.Point(49, 12);
            this.comLab.Name = "comLab";
            this.comLab.Size = new System.Drawing.Size(270, 20);
            this.comLab.TabIndex = 15;
            this.comLab.SelectedIndexChanged += new System.EventHandler(this.comLab_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "分点：";
            // 
            // gridStat
            // 
            this.gridStat.AllowUserToAddRows = false;
            this.gridStat.AllowUserToDeleteRows = false;
            this.gridStat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStat.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.订单状态,
            this.分点,
            this.单位,
            this.section,
            this.ordernum,
            this.realname,
            this.customername,
            this.ordertestlst,
            this.createdate});
            this.gridStat.Location = new System.Drawing.Point(12, 107);
            this.gridStat.Name = "gridStat";
            this.gridStat.RowTemplate.Height = 23;
            this.gridStat.Size = new System.Drawing.Size(999, 483);
            this.gridStat.TabIndex = 27;
            this.gridStat.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.gridStat_RowStateChanged);
            // 
            // radTM
            // 
            this.radTM.AutoSize = true;
            this.radTM.Location = new System.Drawing.Point(73, 18);
            this.radTM.Name = "radTM";
            this.radTM.Size = new System.Drawing.Size(35, 16);
            this.radTM.TabIndex = 31;
            this.radTM.Text = "TM";
            this.radTM.UseVisualStyleBackColor = true;
            // 
            // radC14
            // 
            this.radC14.AutoSize = true;
            this.radC14.Location = new System.Drawing.Point(127, 18);
            this.radC14.Name = "radC14";
            this.radC14.Size = new System.Drawing.Size(41, 16);
            this.radC14.TabIndex = 32;
            this.radC14.Text = "C14";
            this.radC14.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radAll);
            this.groupBox1.Controls.Add(this.radC14);
            this.groupBox1.Controls.Add(this.radTM);
            this.groupBox1.Location = new System.Drawing.Point(349, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 36);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "报告类型";
            // 
            // radAll
            // 
            this.radAll.AutoSize = true;
            this.radAll.Checked = true;
            this.radAll.Location = new System.Drawing.Point(6, 17);
            this.radAll.Name = "radAll";
            this.radAll.Size = new System.Drawing.Size(47, 16);
            this.radAll.TabIndex = 33;
            this.radAll.TabStop = true;
            this.radAll.Text = "全部";
            this.radAll.UseVisualStyleBackColor = true;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(12, 81);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(65, 12);
            this.lblTotal.TabIndex = 34;
            this.lblTotal.Text = "列表总数：";
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.ForeColor = System.Drawing.Color.Blue;
            this.lblCurrent.Location = new System.Drawing.Point(217, 81);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(0, 12);
            this.lblCurrent.TabIndex = 35;
            // 
            // ckPrinted
            // 
            this.ckPrinted.AutoSize = true;
            this.ckPrinted.Checked = true;
            this.ckPrinted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckPrinted.Location = new System.Drawing.Point(561, 56);
            this.ckPrinted.Name = "ckPrinted";
            this.ckPrinted.Size = new System.Drawing.Size(132, 16);
            this.ckPrinted.TabIndex = 36;
            this.ckPrinted.Text = "是否过滤已打印报告";
            this.ckPrinted.UseVisualStyleBackColor = true;
            // 
            // ckXianChang
            // 
            this.ckXianChang.AutoSize = true;
            this.ckXianChang.Checked = true;
            this.ckXianChang.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckXianChang.Location = new System.Drawing.Point(714, 56);
            this.ckXianChang.Name = "ckXianChang";
            this.ckXianChang.Size = new System.Drawing.Size(132, 16);
            this.ckXianChang.TabIndex = 37;
            this.ckXianChang.Text = "是否过滤掉现场报告";
            this.ckXianChang.UseVisualStyleBackColor = true;
            // 
            // ckChangeStatus
            // 
            this.ckChangeStatus.AutoSize = true;
            this.ckChangeStatus.Location = new System.Drawing.Point(853, 55);
            this.ckChangeStatus.Name = "ckChangeStatus";
            this.ckChangeStatus.Size = new System.Drawing.Size(144, 16);
            this.ckChangeStatus.TabIndex = 38;
            this.ckChangeStatus.Text = "是否更新状态为已打印";
            this.ckChangeStatus.UseVisualStyleBackColor = true;
            // 
            // 订单状态
            // 
            this.订单状态.DataPropertyName = "STATUSNAME";
            this.订单状态.HeaderText = "订单状态";
            this.订单状态.Name = "订单状态";
            this.订单状态.Width = 80;
            // 
            // 分点
            // 
            this.分点.DataPropertyName = "labname";
            this.分点.HeaderText = "分点";
            this.分点.Name = "分点";
            // 
            // 单位
            // 
            this.单位.DataPropertyName = "customername";
            this.单位.HeaderText = "单位";
            this.单位.Name = "单位";
            this.单位.Width = 200;
            // 
            // section
            // 
            this.section.DataPropertyName = "section";
            this.section.HeaderText = "区域";
            this.section.Name = "section";
            this.section.Width = 180;
            // 
            // ordernum
            // 
            this.ordernum.DataPropertyName = "ordernum";
            this.ordernum.HeaderText = "体检号";
            this.ordernum.Name = "ordernum";
            // 
            // realname
            // 
            this.realname.DataPropertyName = "realname";
            this.realname.HeaderText = "姓名";
            this.realname.Name = "realname";
            this.realname.Width = 80;
            // 
            // customername
            // 
            this.customername.DataPropertyName = "sex";
            this.customername.HeaderText = "性别";
            this.customername.Name = "customername";
            this.customername.Width = 60;
            // 
            // ordertestlst
            // 
            this.ordertestlst.DataPropertyName = "ordertestlst";
            this.ordertestlst.HeaderText = "套餐";
            this.ordertestlst.Name = "ordertestlst";
            this.ordertestlst.Width = 150;
            // 
            // createdate
            // 
            this.createdate.DataPropertyName = "createdate";
            this.createdate.HeaderText = "登记日期";
            this.createdate.Name = "createdate";
            // 
            // FrmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 602);
            this.Controls.Add(this.ckChangeStatus);
            this.Controls.Add(this.ckXianChang);
            this.Controls.Add(this.ckPrinted);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gridStat);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSection);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateEnd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comCustomer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comLab);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmReport";
            this.Text = "报告单生成";
            ((System.ComponentModel.ISupportInitialize)(this.gridStat)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSection;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateEnd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comCustomer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comLab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView gridStat;
        private System.Windows.Forms.RadioButton radTM;
        private System.Windows.Forms.RadioButton radC14;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radAll;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.CheckBox ckPrinted;
        private System.Windows.Forms.CheckBox ckXianChang;
        private System.Windows.Forms.CheckBox ckChangeStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单状态;
        private System.Windows.Forms.DataGridViewTextBoxColumn 分点;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单位;
        private System.Windows.Forms.DataGridViewTextBoxColumn section;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordernum;
        private System.Windows.Forms.DataGridViewTextBoxColumn realname;
        private System.Windows.Forms.DataGridViewTextBoxColumn customername;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordertestlst;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdate;
    }
}