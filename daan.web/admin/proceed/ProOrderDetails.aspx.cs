using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Collections;

using ExtAspNet;
using daan.util.Common;
using daan.domain;
using daan.service.proceed;
using daan.service.login;
using hlis.service.common;
using daan.service.common;
using daan.service;
using daan.service.dict;

namespace daan.web.admin.proceed
{
    public partial class ProOrderDetails : PageBase
    {
        static ProRegisterService registerserver = new ProRegisterService();
        static LoginService loginservice = new LoginService();
        static DictmemberService memberservice = new DictmemberService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["OrderNum"] != null && Request.QueryString["OrderNum"].ToString() != string.Empty)
                {
                    string OrderNum = tbxOrderNum.Text = Request.QueryString["OrderNum"].ToString();
                    if (OrderNum == string.Empty) { MessageBoxShow("体检流水号为空,请重新打开窗口！"); return; }

                    List<OrderRegister> OrderRegisterList = registerserver.SelectOrdersDetail(OrderNum);
                    //修改绑定头数据
                    OrderBindData(OrderNum);
                    //绑定列表数据
                    BindGridTest(OrderRegisterList);
                }
            }
        }

        /// <summary>绑定Grid列表
        /// 绑定Grid列表
        /// </summary>
        /// <param name="OrderRegisterList">数据源LIST集合</param>
        private void BindGridTest(List<OrderRegister> OrderRegisterList)
        {
            GridTest.DataSource = OrderRegisterList;
            try
            {
                GridTest.DataBind();
            }
            catch (Exception) { }
        }

        //行绑定  
        protected void GridTest_RowDataBound(object sender, GridRowEventArgs e)
        {
            OrderRegister _register = e.DataItem as OrderRegister;
            e.Values[3] = _register.IsGroup ? "组合" : "项目";
            e.Values[4] = _register.Isactive == "1" ? "正常" : "已停测试";
        }

        // 修改时绑定会员数据和订单表数据
        private void OrderBindData(string ordernum)
        {
            Orders order = registerserver.SelectOrderInfo(ordernum);
            Dictmember member = memberservice.GetMemberById(order.Dictmemberid);
            //会员赋值
            SetMemberInfo(member);

            //订单
            tbxRemark.Text = order.Remarks;
            tbxItemTest.Label = tbxItemTest.Text = order.Ordertestlst;
            string[] strage = order.Age.Split('岁');
            tbxAge.Text = strage[0];
            string[] strmoneth = strage[1].Split('月');
            tbxMonth.Text = strmoneth[0];
            string[] strday = strmoneth[1].Split('日');
            tbxDay.Text = strday[0];
            string[] strhour = strday[1].Split('时');
            tbxHour.Text = strhour[0];
            radlCustomerType.SelectedValue = order.Ordersource;
            radlIsMarried.SelectedValue = order.Ismarried;
            tbxSection.Text = order.Section;
            tbxProvincename.Text = order.Province != null ? order.Province : "";
            tbxCityname.Text = order.City != null ? order.City : "";
            tbxCountyname.Text = order.County != null ? order.County : "";

            tbxDictLab.Text = loginservice.GetLoginDictlab().Where<Dictlab>(c => c.Dictlabid == order.Dictlabid).First<Dictlab>().Labname;

            if (order.Ordersource == "1")
            {
                IEnumerable<Dictcustomer> IEcustomer = loginservice.GetDictcustomer().Where<Dictcustomer>(c => c.Dictcustomerid == order.Dictcustomerid);
                if (IEcustomer.Count() > 0)
                {
                    tbxCustomer.Text = IEcustomer.First<Dictcustomer>().Customername;
                }
            }
            else {
                tbxCustomer.Text = "个人客户";
            }

        }

        // 会员资料赋值
        private void SetMemberInfo(Dictmember member)
        {
            tbxPhone.Text = member.Phone;
            tbxMobile.Text = member.Mobile;
            tbxEMail.Text = member.Email;
            tbxBirthday.Text = member.Birthday == null ? "" : member.Birthday.Value.ToString("yyyy-MM-dd");
            tbxAddres.Text = member.Addres;
            tbxIDNumber.Text = member.Idnumber;
            tbxName.Text = member.Realname;
            tbxSex.Text = member.SexName;
        }

        //关闭窗口
        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }
    }
}



