using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using daan.service.login;
using daan.domain;
using System.Collections;
using daan.service.dict;
using daan.service.order;
using daan.util.Common;
using FastReport;
using daan.web.code;
using System.Configuration;
using FastReport.Export.Pdf;
using System.IO;
using daan.service.common;

namespace daan.ui.exportExcel
{
    public partial class FrmReport : Form
    {
        static readonly LoginService loginservice = new LoginService();
        static readonly OrdersService os = new OrdersService();
        DataTable dtExport = null;
        public FrmReport()
        {
            InitializeComponent();
            BindLab();
            dateStart.Text = DateTime.Now.AddDays(-7).ToString();
            gridStat.AutoGenerateColumns = false;
        }

        #region 初始化下拉框
        private void BindLab()
        {
            comLab.Items.Clear();
            List<Dictlab> lablist = new List<Dictlab>();
            Dictlab lab1 = new Dictlab();
            lab1.Labname = "全部";
            lab1.Dictlabid = -1;
            //List<Dictlab> lablist = loginservice.GetLoginDictlab();
            //lablist.Insert(0, lab);
            //comLab.DataSource = lablist;
            //comLab.DisplayMember = "Labname";
            //comLab.ValueMember = "Dictlabid";
            lablist.Add(lab1);
            //lablist.Insert(0, lab1);
            Dictlab lab2 = new Dictlab() { Labname = "济南实验室", Dictlabid = 48 };
            lablist.Add(lab2);
            //lablist.Insert(1, lab2);
            Dictlab lab3 = new Dictlab() { Labname = "其他实验室", Dictlabid = 2 };
            lablist.Add(lab3);
            //lablist.Insert(2, lab3);
            comLab.DataSource = lablist;
            comLab.DisplayMember = "Labname";
            comLab.ValueMember = "Dictlabid";
        }

        private void comLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comLab.SelectedItem == null) return;
            string labid = comLab.SelectedIndex == 0 ? "0" : comLab.SelectedValue.ToString();
            BindCustomer(labid);
        }

        private void BindCustomer(string labid)
        {
            List<Dictcustomer> dictcustomerback = new List<Dictcustomer>();
            if (labid == "0")
            {
                List<Dictcustomer> CustomerList = loginservice.GetDictcustomer();
                List<Dictlab> dictList = loginservice.GetLoginDictlab();
                foreach (Dictlab dict in dictList)
                {
                    List<Dictcustomer> dictcustomerfirt = CustomerList.FindAll(c => (c.Dictlabid == dict.Dictlabid && c.Customertype == "0" && c.Active == "1") || (c.IsPublic == "1" && c.Active == "1"));
                    foreach (Dictcustomer dictcust in dictcustomerfirt)
                    {
                        if (!dictcustomerback.Contains(dictcust))
                            dictcustomerback.Add(dictcust);
                    }
                }
            }
            else if (labid == "48")//济南
            {
                dictcustomerback = loginservice.GetDictcustomer().FindAll(c => (c.Dictlabid == double.Parse(labid) && c.Customertype == "0" && c.Active == "1") || (c.IsPublic == "1" && c.Active == "1"));
            }
            else
            {
                string labids = "43,44,45,46,47,2,3";
                dictcustomerback = (List<Dictcustomer>)new DictCustomerService().GetDictcustomerByLabid(labids);
            }
            Dictcustomer cus = new Dictcustomer();
            cus.Customername = "全部";
            cus.Dictcustomerid = -1;
            dictcustomerback.Insert(0, cus);
            comCustomer.DataSource = dictcustomerback;
            comCustomer.DisplayMember = "Customername";
            comCustomer.ValueMember = "Dictcustomerid";
        }
        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SetButtonEnable(false);
            BindDate();
            SetButtonEnable(true);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("是否确定退出系统?", "退出系统", messButton);
            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                Application.Exit();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (gridStat.RowCount == 0 || dtExport == null || dtExport.Rows.Count == 0)
            {
                messageshow("无数据!");
                return;
            }
            try
            {
                int i = 0;
                if (gridStat.SelectedRows.Count > 0)
                {
                    DialogResult dr = MessageBox.Show("您选中了" + gridStat.SelectedRows.Count.ToString() + "行记录，是否只生成选中报告？", "体检系统", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)//如果点击“确定”按钮
                    {
                        foreach (DataGridViewRow row in gridStat.SelectedRows)
                        {
                            CreatePDF(row);
                            i++;
                            lblCurrent.Text = "生成第" + i.ToString() + "份报告成功";
                        }
                        if (i >= gridStat.SelectedRows.Count)
                        {
                            //gridStat.DataSource = new DataTable();
                            string mes = string.Format("您选中的{0}份报告已全部生成完毕！",i.ToString());
                            messageshow(mes);
                            return;
                        }
                    }
                }
                else
                {
                    DialogResult dr = MessageBox.Show("是否开始生成列表中所有报告？", "体检系统", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)//如果点击“确定”按钮
                    {
                        foreach (DataGridViewRow row in gridStat.Rows)
                        {
                            CreatePDF(row);
                            i++;
                            lblCurrent.Text = "生成第" + i.ToString() + "份报告成功";
                        }
                        if (i >= gridStat.Rows.Count)
                        {
                            //gridStat.DataSource = new DataTable();
                            string mes = string.Format("列表中{0}份报告已全部生成完毕！",i.ToString());
                            messageshow(mes);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                messageshow(ex.Message);
            }
        }

        private void CreatePDF(DataGridViewRow row)
        {
            string ordernum = row.Cells[4].Value.ToString().Trim();
            string cname = row.Cells[2].Value.ToString();
            string realname = row.Cells[5].Value.ToString();
            string section = row.Cells[3].Value.ToString().Replace("\\", "").Replace("/","");
            string labname = row.Cells[1].Value.ToString();
            string strpath = Application.StartupPath + "\\reporttemplate\\";
            using (Report report = new CommonReport().GetReport(ordernum, strpath))
            {
                //生成PDF文件保存在PdfFile文件夹内,以时间命名
                string FilePdfPath = Application.StartupPath + "\\PdfFile\\" + labname + "\\" + ordernum.ToString().Substring(0, 8) + "\\" + cname + "\\";
                string randomName = string.Empty;
                if (!string.IsNullOrEmpty(section))
                {
                    FilePdfPath += section + "\\"; 
                }
                randomName = string.Format("{0}_{1}.pdf", ordernum, realname);
                string pdfPath = FilePdfPath + randomName;
                if (!Directory.Exists(FilePdfPath))
                {
                    Directory.CreateDirectory(FilePdfPath); //若文件夹不存在则新建文件夹
                }
                if (File.Exists(pdfPath))
                    File.Delete(pdfPath);
                PDFExport tyt = new PDFExport() { Compressed = true, RichTextQuality = 50, EmbeddingFonts = false };
                report.Export(tyt, pdfPath);
            }
            if (ckChangeStatus.Checked)
            {
                //修改状态
                Hashtable ht = new Hashtable();
                ht.Add("ordernum", ordernum);
                ht.Add("oldstatus", (int)ParamStatus.OrdersStatus.FinishCheck);
                ht.Add("status", (int)ParamStatus.OrdersStatus.FinishPrint);
                os.EditStatusByOldStatus(ht);
            }
        }
        #region private method
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <returns></returns>
        private Hashtable getPara()
        {
            Hashtable ht = new Hashtable();
            ht.Add("StartDate", dateStart.Text == "" ? null : Convert.ToDateTime(dateStart.Text).ToString("yyyy-MM-dd"));
            ht.Add("EndDate", dateEnd.Text == "" ? null : Convert.ToDateTime(dateEnd.Text).AddDays(1).ToString("yyyy-MM-dd"));
            if (comLab.SelectedIndex == 0)
            {
                List<Dictlab> list = new DictlabService().GetDictlabList();
                StringBuilder joinlabid = new StringBuilder();
                foreach (Dictlab lab in list)
                {
                    joinlabid.Append(lab.Dictlabid + ",");
                }
                ht.Add("labid", joinlabid.ToString().TrimEnd(','));
            }
            else if (comLab.SelectedIndex == 1)
            {
                ht.Add("labid", comLab.SelectedValue);
            }
            else
            {
                ht.Add("labid", "43,44,45,46,47,2,3");
            }
            if (comCustomer.SelectedIndex == 0)
            {
                ht.Add("dictcustomerid", null);
            }
            else
            {
                ht.Add("dictcustomerid", comCustomer.SelectedValue);
            }
            ht.Add("Section", txtSection.Text.Trim());
            if (radAll.Checked)
            {
                ht.Add("reporttype",null);
            }
            if (radC14.Checked)
            {
                ht.Add("reporttype", "61");
            }
            if (radTM.Checked)
            {
                ht.Add("reporttype", "3");
            }
            //是否过滤已打印报告
            if (ckPrinted.Checked)
            {
                ht.Add("printed","25");
            }
            //是否过滤现场报告
            if (ckXianChang.Checked)
            {
                ht.Add("xianchang","1");
            }
            return ht;
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        private void BindDate()
        {
            try
            {
                Hashtable ht = getPara();
                dtExport = os.GetReportDataForWinform(ht);
                gridStat.DataSource = dtExport;
                lblTotal.Text = "查询总数："+dtExport.Rows.Count.ToString();
                lblCurrent.Text = "";
            }
            catch (Exception e)
            {
                messageshow("查询数据出错：" + e.Message);
            }
        }
        /// <summary>
        /// 弹出消息框
        /// </summary>
        /// <param name="mes"></param>
        private void messageshow(string mes)
        {
            MessageBox.Show(mes);
        }
        /// <summary>
        /// 设置按钮是否可用
        /// </summary>
        /// <param name="f"></param>
        private void SetButtonEnable(bool f)
        {
            btnExport.Enabled = f;
            btnSearch.Enabled = f;
        }
        #endregion

        private void gridStat_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }

    }
}
