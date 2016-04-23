using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.order;
using daan.util.Web;
using System.Collections;
using ExtAspNet;
using daan.domain;
using daan.service.dict;
using System.Text.RegularExpressions;
using daan.service.proceed;

namespace daan.web.admin.proceed
{
    public partial class HpvReceiving : PageBase
    {
        HpvtestingService hpvService = new HpvtestingService();       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        /// <summary>绑定列表
        /// 
        /// </summary>
        /// <param name="hpvlist">数据源</param>
        private void BindGrid(List<Hpvinstruments> hpvlist)
        {
            try
            {
                gvList.DataSource = hpvlist;
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message,MessageBoxIcon.Error);
            }
        }

        // 扫描耗材条码,检查该耗材号是否存在及是否已关联标本条码号
        protected void tbxinstrumentsbarcode_TriggerClick(object sender, EventArgs e)
        {
            List<Hpvinstruments> hpvlist = Check();
            BindGrid(hpvlist);
        }

        // 扫描标本条码
        protected void tbxbarcode_TriggerClick(object sender, EventArgs e)
        {                                   
            try
            {
                string barcode = tbxbarcode.Text.Trim();
                
                List<Hpvinstruments> hpvlist = Check();
                if (hpvlist == null || hpvlist.Count == 0)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(barcode))
                {
                    return;
                }                
                else
                {
                    //条码号必须以00结尾,且长度为12
                    if (barcode.Length != 12)
                    {
                        MessageBoxShow(string.Format("此条码号[{0}]格式不正确，请更改条码号！", barcode), MessageBoxIcon.Information);
                        tbxbarcode.Text = string.Empty;
                        return;
                    }
                    if (barcode.Substring(barcode.Length - 2) != "00")
                    {
                        MessageBoxShow(string.Format("此条码号[{0}]不是以00结尾，请更改条码号！", barcode), MessageBoxIcon.Information);
                        tbxbarcode.Text = string.Empty;
                        return;
                    }
                    Hashtable htPara = new Hashtable();
                    //检查耗材表中该条码号是否已使用过
                    htPara.Add("Barcode", barcode);
                    List<Hpvinstruments> hpvbarcode = hpvService.GetHpvinstrumentsByWhere(htPara).ToList(); //是否保存了该标本条码
                    if (hpvbarcode.Count == 1)
                    {
                        MessageBoxShow("该条码已有相关联的耗材，请更换其他条码！", MessageBoxIcon.Information);
                        tbxbarcode.Text = string.Empty;          
                        return;
                    }

                    //检查orderbarcode表中是否存在该条码号
                    htPara.Clear();
                    htPara.Add("ordebarcode", tbxbarcode.Text);
                    List<Orderbarcode> orderbarcodelist = new OrderbarcodeService().SelectOrderbarcode(htPara).ToList();
                    if (orderbarcodelist.Count > 0)
                    {
                        MessageBoxShow("该条码已经生成了订单！", MessageBoxIcon.Information);
                        tbxbarcode.Text = string.Empty;
                        return;
                    }

                    //添加样本条码等信息
                    Hpvinstruments hpvs = hpvlist[0];
                    hpvs.Barcode = this.tbxbarcode.Text.Trim();
                    hpvs.Barcodecreatedate = DateTime.Now;
                    hpvs.Barcodeenterby = Userinfo.userName;
                    hpvs.Testname = hpvlist[0].Testname;
                    bool flag = hpvService.InsertHpvinstruments(hpvs);
                    if (flag)
                    {
                        hpvlist = new List<Hpvinstruments>();
                        hpvlist.Add(hpvs);
                        BindGrid(hpvlist);
                        this.tbxbarcode.Text = string.Empty;
                        this.tbxinstrumentsbarcode.Text = string.Empty;
                        tbxinstrumentsbarcode.Focus();                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>耗材号唯一性检测
        /// 
        /// </summary>
        private List<Hpvinstruments> Check()
        {
            List<Hpvinstruments> hpvlist = new List<Hpvinstruments>();

            if (string.IsNullOrWhiteSpace(tbxinstrumentsbarcode.Text.Trim()))
            {
                BindGrid(hpvlist);
                tbxinstrumentsbarcode.Focus(true);
                return null;
            }

            Hashtable ht = new Hashtable();
            ht.Add("Instrumentsbarcode", this.tbxinstrumentsbarcode.Text.Trim());
            //查询数据库是否有保存该耗材条码信息
            hpvlist = hpvService.GetHpvinstrumentsByWhere(ht).ToList();

            if (hpvlist.Count == 0)
            {
                MessageBoxShow("没有找到该耗材条码信息,请先关联耗材！", MessageBoxIcon.Information);
                BindGrid(hpvlist);
                tbxinstrumentsbarcode.Text = string.Empty;
                tbxbarcode.Text = string.Empty;
                return null;
            }
            //避免重复关联一个耗材号
            if (!string.IsNullOrWhiteSpace(hpvlist[0].Barcode))
            {
                MessageBoxShow("该耗材已有对应的条码号", MessageBoxIcon.Information);
                BindGrid(hpvlist);
                tbxinstrumentsbarcode.Text = string.Empty;
                tbxbarcode.Text = string.Empty;
                tbxinstrumentsbarcode.Focus(true);
            }
            this.tbxbarcode.Focus(true);
            return hpvlist;
        }
    }
}