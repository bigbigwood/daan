namespace daan.ui.exportExcel
{
    partial class FrmExport
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
            this.label1 = new System.Windows.Forms.Label();
            this.comLab = new System.Windows.Forms.ComboBox();
            this.comCustomer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dateStart = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dateEnd = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSection = new System.Windows.Forms.TextBox();
            this.gridStat = new System.Windows.Forms.DataGridView();
            this.ordercode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customername = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.section = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordertestlst = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.samplingdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordercount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.finishedCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.finishedTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridStat)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "分点：";
            // 
            // comLab
            // 
            this.comLab.FormattingEnabled = true;
            this.comLab.Location = new System.Drawing.Point(71, 21);
            this.comLab.Name = "comLab";
            this.comLab.Size = new System.Drawing.Size(270, 20);
            this.comLab.TabIndex = 1;
            this.comLab.SelectedIndexChanged += new System.EventHandler(this.comLab_SelectedIndexChanged);
            // 
            // comCustomer
            // 
            this.comCustomer.FormattingEnabled = true;
            this.comCustomer.Location = new System.Drawing.Point(404, 20);
            this.comCustomer.Name = "comCustomer";
            this.comCustomer.Size = new System.Drawing.Size(311, 20);
            this.comCustomer.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(369, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "单位：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "登记时间：";
            // 
            // dateStart
            // 
            this.dateStart.Location = new System.Drawing.Point(85, 55);
            this.dateStart.Name = "dateStart";
            this.dateStart.Size = new System.Drawing.Size(166, 21);
            this.dateStart.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(369, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "至：";
            // 
            // dateEnd
            // 
            this.dateEnd.Location = new System.Drawing.Point(404, 52);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.Size = new System.Drawing.Size(166, 21);
            this.dateEnd.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(734, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "区域：";
            // 
            // txtSection
            // 
            this.txtSection.Location = new System.Drawing.Point(781, 20);
            this.txtSection.Name = "txtSection";
            this.txtSection.Size = new System.Drawing.Size(189, 21);
            this.txtSection.TabIndex = 9;
            // 
            // gridStat
            // 
            this.gridStat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStat.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ordercode,
            this.labname,
            this.customername,
            this.section,
            this.ordertestlst,
            this.createdate,
            this.samplingdate,
            this.ordercount,
            this.importCount,
            this.importTime,
            this.resultCount,
            this.resultTime,
            this.finishedCount,
            this.finishedTime,
            this.printCount,
            this.printTime});
            this.gridStat.Location = new System.Drawing.Point(26, 93);
            this.gridStat.Name = "gridStat";
            this.gridStat.RowTemplate.Height = 23;
            this.gridStat.Size = new System.Drawing.Size(977, 498);
            this.gridStat.TabIndex = 10;
            // 
            // ordercode
            // 
            this.ordercode.DataPropertyName = "ordercode";
            this.ordercode.HeaderText = "订单编号";
            this.ordercode.Name = "ordercode";
            // 
            // labname
            // 
            this.labname.DataPropertyName = "labname";
            this.labname.HeaderText = "分点";
            this.labname.Name = "labname";
            // 
            // customername
            // 
            this.customername.DataPropertyName = "customername";
            this.customername.HeaderText = "单位";
            this.customername.Name = "customername";
            // 
            // section
            // 
            this.section.DataPropertyName = "section";
            this.section.HeaderText = "区域";
            this.section.Name = "section";
            // 
            // ordertestlst
            // 
            this.ordertestlst.DataPropertyName = "ordertestlst";
            this.ordertestlst.HeaderText = "套餐";
            this.ordertestlst.Name = "ordertestlst";
            // 
            // createdate
            // 
            this.createdate.DataPropertyName = "createdate";
            this.createdate.HeaderText = "登记日期";
            this.createdate.Name = "createdate";
            // 
            // samplingdate
            // 
            this.samplingdate.DataPropertyName = "samplingdate";
            this.samplingdate.HeaderText = "采样日期";
            this.samplingdate.Name = "samplingdate";
            // 
            // ordercount
            // 
            this.ordercount.DataPropertyName = "ordercount";
            this.ordercount.HeaderText = "订单数量";
            this.ordercount.Name = "ordercount";
            // 
            // importCount
            // 
            this.importCount.DataPropertyName = "importCount";
            this.importCount.HeaderText = "订单上传";
            this.importCount.Name = "importCount";
            // 
            // importTime
            // 
            this.importTime.DataPropertyName = "importTime";
            this.importTime.HeaderText = "最后上传时间";
            this.importTime.Name = "importTime";
            // 
            // resultCount
            // 
            this.resultCount.DataPropertyName = "resultCount";
            this.resultCount.HeaderText = "结果录入";
            this.resultCount.Name = "resultCount";
            // 
            // resultTime
            // 
            this.resultTime.DataPropertyName = "resultTime";
            this.resultTime.HeaderText = "最后录入时间";
            this.resultTime.Name = "resultTime";
            // 
            // finishedCount
            // 
            this.finishedCount.DataPropertyName = "finishedCount";
            this.finishedCount.HeaderText = "总检";
            this.finishedCount.Name = "finishedCount";
            // 
            // finishedTime
            // 
            this.finishedTime.DataPropertyName = "finishedTime";
            this.finishedTime.HeaderText = "最后总检时间";
            this.finishedTime.Name = "finishedTime";
            // 
            // printCount
            // 
            this.printCount.DataPropertyName = "printCount";
            this.printCount.HeaderText = "报告打印";
            this.printCount.Name = "printCount";
            // 
            // printTime
            // 
            this.printTime.DataPropertyName = "printTime";
            this.printTime.HeaderText = "最后打印时间";
            this.printTime.Name = "printTime";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(732, 50);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(829, 50);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(928, 50);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // FrmExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 617);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.gridStat);
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
            this.MaximizeBox = false;
            this.Name = "FrmExport";
            this.Text = "TM交付管理信息报表";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmExport_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.gridStat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comLab;
        private System.Windows.Forms.ComboBox comCustomer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateEnd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSection;
        private System.Windows.Forms.DataGridView gridStat;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordercode;
        private System.Windows.Forms.DataGridViewTextBoxColumn labname;
        private System.Windows.Forms.DataGridViewTextBoxColumn customername;
        private System.Windows.Forms.DataGridViewTextBoxColumn section;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordertestlst;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn samplingdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordercount;
        private System.Windows.Forms.DataGridViewTextBoxColumn importCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn importTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn resultCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn resultTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn finishedCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn finishedTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn printCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn printTime;
    }
}