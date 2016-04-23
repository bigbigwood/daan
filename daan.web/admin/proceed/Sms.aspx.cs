using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.domain;
using daan.service.login;
using System.Data;
using daan.service.order;
using System.Collections;
using ExtAspNet;
using System.Text;
using System.Configuration;
using daan.util.Web;
using daan.util.Common;
using daan.web.code;

namespace daan.web.admin.proceed
{
    public partial class Sms : PageBase
    {        
        LoginService loginservice = new LoginService();
        OrdersService orderService = new OrdersService();

        protected void Page_Load(object sender, EventArgs e)
        {
            ExtAspNet.PageContext.RegisterStartupScript("(Ext.getCmp('" + DropCustomer.ClientID + "')).listWidth=250;");  
            if (!IsPostBack)
            {
                DDLDictLabBinder(dropDictLab, true);
                dropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
                DropDictcustomerBinder(DropCustomer, dropDictLab.SelectedValue, true);

                BindSmsList();
                datebegin.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                dateend.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
                
        }
        /// <summary>绑定短信模板数据
        /// 
        /// </summary>
        private void BindSmsList()
        {
            gvSmsList.DataSource = loginservice.GetLoginDictSmsModuleList();
            gvSmsList.DataBind();
        }

        /// <summary>选择分点，分点与单位联动
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDictcustomerBinder(DropCustomer, dropDictLab.SelectedValue, true);
        }
        
        /// <summary>绑定列表数据
        /// 
        /// </summary>
        private void BindData()
        {
            PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
            Hashtable ht = new Hashtable();
            ht.Add("ordernum", txtOrderNum.Text);
            ht.Add("labid", dropDictLab.SelectedValue);
            ht.Add("status", ddlStatus.SelectedValue);
            ht.Add("customerid", DropCustomer.SelectedValue);
            ht.Add("DateStart", datebegin.Text);
            ht.Add("DateEnd", Convert.ToDateTime(dateend.Text).AddDays(1).ToString("yyyy-MM-dd"));
            ht.Add("pageStart", pageUtil.GetPageStartNum());
            ht.Add("pageEnd", pageUtil.GetPageEndNum());
            gvList.RecordCount = orderService.WaitSendSmsUsersCount(ht);
            gvList.DataSource = orderService.WaitSendSmsUsers(ht);
            gvList.DataBind();
        }

        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvList.PageIndex = 0;
            BindData();
        }

        //gvList分页事件
        protected void gvList_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindData();
        }

        #region >>>短信发送

        // 选择短信模板
        protected void gvSmsList_RowClick(object sender, GridRowClickEventArgs e)
        {
            string[] row = gvSmsList.Rows[e.RowIndex].Values;
            txtSmsContent.Text = row[2].ToString();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {           
            //获取要发送的ordernum与phone
            StringBuilder strphone=new StringBuilder();
            StringBuilder strordernum = new StringBuilder();
            foreach (int row in gvList.SelectedRowIndexArray)
            {
                if (string.IsNullOrEmpty(gvList.DataKeys[row][2].ToString()))
                {
                    continue;
                }
                strordernum.Append(gvList.DataKeys[row][0].ToString());
                strordernum.Append(",");

                strphone.Append(gvList.DataKeys[row][2].ToString());
                strphone.Append(",");                
            }
            if (strphone.ToString().TrimEnd(',').Length == 0)
            {
                MessageBoxShow("请选择要发送短信的客户", MessageBoxIcon.Information);   
                return;
            }

            Hashtable ht = new Hashtable();
            //身份验证信息
            ht["CallerID"] = "A9EF3719DCC0481A";                  //唯一身份标识
            ht["AccessKey"] = "0CA937136DF3A125";                 //密码            
            ht["SourceCode"] = "tijian";                          //体检系统

            //短信内容
            ht["ReceivePhone"] = strphone.ToString().TrimStart(',').TrimEnd(',');                     //手机号码用逗号隔开，最多500个
            ht["Contents"] = txtSmsContent.Text.Trim() + "【达安临检中心】";     //短信内容最多200个汉字
            ht["SendDate"] = DateTime.Now.ToString();                     //短信发送时间，用于定时发送短信，即时发为当前时间
            ht["SendID"] = Userinfo.userId+"|"+Userinfo.userName;                                      //短信发送人ID,没有时可填写平台标识

            string url = ConfigurationManager.AppSettings["SmsUrl"];   //调用webservice地址
            string toPara = CommonFuncLibClient.ObjToBytes(ht);     //格式化参数            
            object objResult = CommonFuncLibClient.InvokeWebservice(url, "SMSCenter", "WsSendSms", "SendSms", new object[] { toPara });

            var rlt = objResult.ToString();
            //[101:身份验证信息不成功][200:短信插入成功][201:短信插入不成功]
            if (rlt == "200")
            {
                MessageBoxShow("信息已提交至发送中心，但可能会存在延迟而影响接收时间",MessageBoxIcon.Information);

                ht = new Hashtable();
                ht.Add("SmsTitle", txtSmsContent.Text.Trim());
                ht.Add("OrderNum", strordernum.ToString().TrimEnd(','));
                orderService.UpdateOrdersSms(ht);
            }
            else
            {
                if (rlt == "101")  MessageBoxShow("身份验证信息不成功,错误码：" + rlt,MessageBoxIcon.Error);
                if (rlt == "201") MessageBoxShow("短信写入数据库出错,错误码：" + rlt, MessageBoxIcon.Error);
            }

            btnSearch_Click(null,null);
        }
        #endregion


        #region >>>导出到Excel
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvList.Rows.Count == 0)
                {
                    MessageBoxShow("没有导出的数据", MessageBoxIcon.Information);
                    return;
                }
                Hashtable ht = new Hashtable();
                ht.Add("ordernum", txtOrderNum.Text);
                ht.Add("labid", dropDictLab.SelectedValue);
                ht.Add("status", ddlStatus.SelectedValue);
                ht.Add("customerid", DropCustomer.SelectedValue);
                ht.Add("DateStart", datebegin.Text);
                ht.Add("DateEnd", Convert.ToDateTime(dateend.Text).AddDays(1).ToString("yyyy-MM-dd"));

                List<Orders> orderlist = orderService.WaitDownLoadUsers(ht);
                if (orderlist.Count > 0)
                {
                    String sheetname = DateTime.Now.ToString("yyyy-MM-dd");
                    String filename = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                    SortedList sortlist = new SortedList(new MySort());
                    sortlist.Add("Ordernum", "体检号");
                    sortlist.Add("Realname", "姓名");
                    sortlist.Add("Sex", "性别");
                    sortlist.Add("Mobile", "联系方式");
                    sortlist.Add("addres", "地址");
                    for (int i = 0; i < orderlist.Count; i++)
                    {
                        if (orderlist[i].Sex == "M")
                        {
                            orderlist[i].Sex = "男";
                        }
                        else if (orderlist[i].Sex == "F")
                        {
                            orderlist[i].Sex = "女";
                        }
                        else if (orderlist[i].Sex == "U")
                        {
                            orderlist[i].Sex = "未知";
                        }
                    }
                    ExcelOperation<Orders>.ExportListToExcel(orderlist, sortlist, filename, sheetname);             
                }
                else
                {
                    MessageBoxShow("没有导出的数据", MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion     
    }
}