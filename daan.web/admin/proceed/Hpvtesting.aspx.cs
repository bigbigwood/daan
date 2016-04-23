using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.util.Web;
using System.Collections;
using ExtAspNet;
using daan.domain;
using daan.service.login;
using daan.service.order;
using daan.service.dict;
using System.Text.RegularExpressions;
using daan.service.proceed;

namespace daan.web.admin.proceed
{
    public partial class Hpvtesting : PageBase
    {
        HpvtestingService hpvService = new HpvtestingService();
        LoginService loginservice = new LoginService();
        List<Hpvinstruments> hpvinstrumentsLst = new List<Hpvinstruments>();
        //加载事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDicttestitemid();
                //绑定分点
                DDLDictLabBinder(DropDictLab, true);

                //体检单位初始化
                DropDictcustomerBinder(Drop_Ctcustomer, DropDictLab.SelectedValue, false);
            }
        }
        //选择分点，分点与单位联动
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDictcustomerBinder(Drop_Ctcustomer, DropDictLab.SelectedValue, true);
        }  

        /// <summary>绑定套餐选择
        /// 
        /// </summary>
        private void BindDicttestitemid()
        {
            try
            {
                IList<Dicttestitem> list = hpvService.GetDicttestitemWithIsProject();
                if (list == null)
                    return;

                foreach (Dicttestitem item in list)
                {
                    this.Drop_Dicttestitem.Items.Add(item.Testname, item.Dicttestitemid == null ? "-1" : item.Dicttestitemid.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }


        //扫描耗材条码,并累积显示本次扫描的条码
        protected void tbxbarcode_TriggerClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.tbxbarcode.Text.Trim()))
                {
                    return;
                }
                Hashtable ht = new Hashtable();
                ht.Add("Instrumentsbarcode", tbxbarcode.Text);
                //是否存在相同的耗材条码 如已存在则跳出
                List<Hpvinstruments> hpvlist = hpvService.GetHpvinstrumentsByWhere(ht);
                if (hpvlist.Count > 0)
                {
                    MessageBoxShow("该耗材条码已存在！", MessageBoxIcon.Information);
                    return;
                }
                Hpvinstruments hpvs = new Hpvinstruments();
                hpvs.Dictcustomerid = Convert.ToDouble(this.Drop_Ctcustomer.SelectedValue);
                hpvs.Dicttestitemid = Convert.ToDouble(this.Drop_Dicttestitem.SelectedValue);
                hpvs.Instrumentsbarcode = this.tbxbarcode.Text.Trim();
                hpvs.Instrumentsbarcode = this.tbxbarcode.Text.Trim();
                hpvs.Instenterby = Userinfo.userName;              
                hpvs.Isactive = "1";
                bool flag = hpvService.InsertHpvinstruments(hpvs);
                if (flag)
                {
                    hpvs.Customername = this.Drop_Ctcustomer.SelectedText;
                    hpvs.Testname = this.Drop_Dicttestitem.SelectedText;
                    hpvs.Instcreatedate = DateTime.Now;
                    hpvinstrumentsLst.Add(hpvs);
                    gvList.DataSource = hpvinstrumentsLst;
                    gvList.DataBind();
                    this.tbxbarcode.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.tbxbarcode.Text = string.Empty;
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        //条码段生成 输入开始条码、结束条码、条码间隔生成条形码
        public void Create_Click(object sender, EventArgs e)
        {            


            hpvinstrumentsLst.Clear();
            try
            {
                double nStar = double.Parse(TextStar.Text);
                double nEnd = double.Parse(TextEnd.Text);
                int spaceNum = int.Parse(txtSpaceNum.Text);

                List<Hpvinstruments> hpvlist=new List<Hpvinstruments>();
                while (nStar <= nEnd)
                {
                    //是否存在相同的耗材条码 如已存在则跳出.
                    Hashtable ht = new Hashtable();
                    ht.Add("Instrumentsbarcode", nStar);
                    hpvlist = hpvService.GetHpvinstrumentsByWhere(ht);
                    if (hpvlist.Count > 0)
                    {
                        nStar = nStar + spaceNum;
                        continue;
                    }
                    Hpvinstruments hpvs = new Hpvinstruments();
                    hpvs.Dictcustomerid = Convert.ToDouble(this.Drop_Ctcustomer.SelectedValue);
                    hpvs.Dicttestitemid = Convert.ToDouble(this.Drop_Dicttestitem.SelectedValue);
                    hpvs.Instrumentsbarcode = nStar.ToString();
                    hpvs.Instenterby = Userinfo.userName;
                    hpvs.Instcreatedate = DateTime.Now;
                    hpvs.Isactive = "1";
                    hpvs.Customername = this.Drop_Ctcustomer.SelectedText;
                    hpvs.Testname = this.Drop_Dicttestitem.SelectedText;
                    hpvService.InsertHpvinstruments(hpvs);

                    hpvinstrumentsLst.Add(hpvs);
                    nStar = nStar + spaceNum;
                }
                gvList.DataSource = hpvinstrumentsLst;
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
    }
}