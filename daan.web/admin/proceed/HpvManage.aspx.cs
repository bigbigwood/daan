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
    public partial class HpvManage : PageBase
    {
        HpvtestingService hpvService = new HpvtestingService();
        LoginService loginservice = new LoginService();
        //加载事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Start.SelectedDate = DateTime.Now.AddDays(-7);
                End.SelectedDate = DateTime.Now;
                
                BindDicttestitemid();
                //绑定分点
                DDLDictLabBinder(DropDictLab, true);
                DropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));

                //体检单位初始化
                DropDictcustomerBinder(Dictcustomerid, DropDictLab.SelectedValue, true);
            }
        }

        //选择分点，分点与单位联动
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDictcustomerBinder(Dictcustomerid, DropDictLab.SelectedValue, true);
        }  

        //查询事件
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            if (this.Start.Text != "" && this.End.Text != "")
            {
                if (this.Start.SelectedDate <= this.End.SelectedDate)
                {
                    BindGrid();
                }
                else
                {
                    MessageBoxShow("结束时间应大于开始时间！", MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBoxShow("请输入开始时间及结束时间查询！", MessageBoxIcon.Information);
            }

        }

        //分页
        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        //行点击事件 删除
        protected void gvList_RowCommand(object sender, GridCommandEventArgs e)
        {
            object[] objvalue = gvList.DataKeys[e.RowIndex];

            //取消关联
            if (e.CommandName == "Cancel")
            {
                if (objvalue[1] == null)
                {
                    MessageBoxShow("没有关联的条码号，如果不需要，可直接删除！");
                    return;
                }
                Hashtable ht = new Hashtable();
                ht.Add("ordebarcode", objvalue[1].ToString());
                List<Orderbarcode> orderbarcodelist = new OrderbarcodeService().SelectOrderbarcode(ht).ToList();
                if (orderbarcodelist.Count > 0)
                {
                    MessageBoxShow("该条码已经生成了订单，不能取消！");
                    return;
                }
                bool falg = hpvService.UpdateHpvinstruments(objvalue[0].ToString());
                if (falg)
                {
                    MessageBoxShow("取消成功！");
                }
                else
                {
                    MessageBoxShow("取消失败！");
                    return;
                }
                BindGrid();
            }



            if (e.CommandName == "delete")
            {
                if (objvalue[1] != null)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("ordebarcode", objvalue[1].ToString());
                    List<Orderbarcode> orderbarcodelist = new OrderbarcodeService().SelectOrderbarcode(ht).ToList();
                    if (orderbarcodelist.Count > 0)
                    {
                        MessageBoxShow("该条码已经生成了订单，不能删除！");
                        return;
                    }
                }
                bool falg = hpvService.DeleteHpvinstruments(objvalue[0].ToString());
                if (!falg)
                {
                    MessageBoxShow("删除失败！");
                    return;
                }
                BindGrid();
            }
        }
        /// <summary>
        /// 绑定套餐选择
        /// </summary>
        private void BindDicttestitemid()
        {
            IList<Dicttestitem> list = hpvService.GetDicttestitemWithIsProject();
            if (list == null)
                return;

            foreach (Dicttestitem item in list)
            {
                this.Dicttestitemid.Items.Add(item.Testname, item.Dicttestitemid == null ? "-1" : item.Dicttestitemid.Value.ToString());
            }
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindGrid()
        {
            //分页查询条件
            PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
            Hashtable ht = new Hashtable();
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();
            ht["Start"] = Start.Text;
            ht["End"] = Convert.ToDateTime(End.Text).AddDays(1).ToString("yyyy-MM-dd");
            ht.Add("Dictcustomerid", Dictcustomerid.SelectedValue == "-1" ? null : Dictcustomerid.SelectedValue);
            ht.Add("Dicttestitemid", Dicttestitemid.SelectedValue == "-1" ? null : Dicttestitemid.SelectedValue);
            ht.Add("Instrumentsbarcode", Instrumentsbarcode.Text.Trim());
            ht.Add("Barcode", Barcode.Text.Trim());

            
            gvList.RecordCount = hpvService.GetHpvinstrumentsPageLstCountNew(ht);
            gvList.DataSource = hpvService.GetHpvinstrumentsByWhereNew(ht);
            gvList.DataBind();
        }
    }
}