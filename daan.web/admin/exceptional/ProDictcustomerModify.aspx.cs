using System;
using System.Collections.Generic;
using System.Linq;
using daan.service.proceed;
using System.Collections;
using ExtAspNet;

namespace daan.web.admin.exceptional
{
    public partial class ProDictcustomerModify : PageBase
    {
        readonly static ProCentralizedManagementService mamagement = new ProCentralizedManagementService();
        protected void Page_Load(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(String.Format("(Ext.getCmp('{0}')).listWidth=250;", DropCustomer.ClientID));
            if (!IsPostBack)
            {
                if (Request.QueryString["OrderNum"] != null && Request.QueryString["OrderNum"] != string.Empty)
                {
                    BindDictLab();
                    BindArea();
                    string OrderNum = Request.QueryString["OrderNum"];
                    hidOrderNums.Text = OrderNum;
                    BindData();
                }
            }
        }

        #region 省市区绑定及下拉选择事件
        private void BindArea()
        {
            DropProvinceBinder(dpProvince);
            DropCityBinder(dpProvince, dpCity);
            DropCountyBinder(dpCity, dpCounty);
        }
        //选择省份
        protected void dpProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropCityBinder(dpProvince, dpCity);
            DropCountyBinder(dpCity, dpCounty);
        }
        //选择市
        protected void dpCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropCountyBinder(dpCity, dpCounty);
        }

        #endregion

        #region 绑定分点  单位
        /// <summary>
        /// 绑定分点
        /// </summary>
        private void BindDictLab()
        {
            DDLDictLabBinder(DropDictLab, true);
            DropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            DropDictLab.SelectedIndex = 0;
            if (DropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(DropDictLab.SelectedValue));
            }
        }

        /// <summary>
        /// 选择分点事件 绑定单位
        /// </summary>
        protected void DropDictLab_SelectedIndexChangeds(object sender, EventArgs e)
        {
            BindCustomer(Convert.ToInt32(DropDictLab.SelectedValue));
        }
        /// <summary>
        /// 绑定单位
        /// </summary>
        private void BindCustomer(double labid)
        {
            DropDictcustomerBinder(DropCustomer, labid.ToString(), true);
        }
        #endregion

        /// <summary>
        /// 绑定Grid数据
        /// </summary>
        /// <param name="ht">查询参数</param>
        /// <param name="isSearch">是否重新查询</param>
        private void BindData()
        {
            Hashtable ht = new Hashtable();
            ht["ordernum"] = hidOrderNums.Text;
            GridOrders.DataSource = mamagement.GetManagementOrdersByDictmemberid(ht);
            GridOrders.DataBind();
        }

        ///保存
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string ordernums = hidOrderNums.Text;
            if (string.IsNullOrEmpty(ordernums))
            {
                MessageBoxShow("没有体检号，请关闭窗口后重新选择！"); return;
            }
            if (ordernums == string.Empty) { return; }
            Hashtable ht = new Hashtable();
            ht.Add("ordernum", ordernums);
            //分点
            int dropDictLab = Convert.ToInt32(DropDictLab.SelectedValue);
            if (dropDictLab != -1)
            {   
                ht.Add("dictlabid", dropDictLab);
            }
            //单位
            int dropCustomer = Convert.ToInt32(DropCustomer.SelectedValue);
            if (dropCustomer != -1)
            {
                ht.Add("dictcustomerid", dropCustomer);
            }
            //省
            string province = dpProvince.SelectedValue;
            if (province != "-1")
                ht.Add("province", province);
            //市
            string city = dpCity.SelectedValue;
            if (city != "-1")
                ht.Add("city",city);
            //区县
            string county = dpCounty.SelectedValue;
            if (county != "-1")
                ht.Add("county", county);
            //部门机构
            if (!string.IsNullOrEmpty(txtSection.Text.Trim()))
                ht.Add("section", txtSection.Text.Trim());
            //营业区
            if (!string.IsNullOrEmpty(txtArea.Text.Trim()))
                ht.Add("area", txtArea.Text.Trim());
            //客户经理
            if (!string.IsNullOrEmpty(txtAccountManager.Text.Trim()))
                ht.Add("accountmanager", txtAccountManager.Text.Trim());
            //采样日期
            if (!string.IsNullOrEmpty(dtSampleDate.Text))
                ht.Add("sampledate", dtSampleDate.Text);
            //报告回寄信息
            if (!string.IsNullOrEmpty(txtAddress.Text.Trim()))
                ht.Add("address", txtAddress.Text.Trim());
            if (!string.IsNullOrEmpty(txtRecName.Text.Trim()))
                ht.Add("recname", txtRecName.Text.Trim());
            if (!string.IsNullOrEmpty(txtTelphone.Text.Trim()))
                ht.Add("telphone", txtTelphone.Text.Trim());
            if (ht.Count <= 1) return;
            
            if (mamagement.UpdateDictmemberidByOrderNum(ht))
            {
                MessageBoxShow("批量修改成功！");
                //刷记录
                BindData();
                //记录操作日志
                string[] arr = ordernums.Split(',');
                foreach (string str in arr)
                {
                    mamagement.AddOperationLog(str, "", "异常管理中心", "批量修改订单[" + str + "]", "修改留痕", "批量" + ordernums);
                }
            }
            else
            {
                MessageBoxShow("修改单位名称失败，请刷新页面重试！"); 
            }
        }
        //关闭窗口
        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }
    }
}