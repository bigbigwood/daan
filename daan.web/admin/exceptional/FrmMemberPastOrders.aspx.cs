using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using ExtAspNet;
using daan.service.order;
using System.Data;
using System.Text;
namespace daan.web.admin.exceptional
{
    public partial class FrmMemberPastOrders : PageBase
    {
        static OrdersService os = new OrdersService();
        protected void Page_Load(object sender, EventArgs e)
        {
            //输入姓名回车触发查询
            if (Request.Form["__EVENTTARGET"] == txtRealName.ClientID && Request.Form["__EVENTARGUMENT"] == "specialkey")
            {
                BindData();
            }
        }
        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }
        //手动关联
        protected void btnGuanLian_Click(object sender, EventArgs e)
        {
            //只有一条信息的就不用进行关联操作
            if (gdPastOrdersList.Rows.Count <=1 || Grid1.Rows.Count <=1)
            {
                return;
            }
            if (gdPastOrdersList.SelectedRowIndexArray.Length == 0)
            {
                MessageBoxShow("请先选择主会员再进行关联操作!",MessageBoxIcon.Information);
                return;
            }
            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                MessageBoxShow("请选择要进行关联的会员!",MessageBoxIcon.Information);
                return;
            }
            string memberid = string.Empty;
            if (gdPastOrdersList.DataKeys[gdPastOrdersList.SelectedRowIndexArray[0]][0] != null)
                memberid = gdPastOrdersList.DataKeys[gdPastOrdersList.SelectedRowIndexArray[0]][0].ToString();
          
            int[] selectedRowsIndex = Grid1.SelectedRowIndexArray;
            StringBuilder str = new StringBuilder();
            foreach (int i in selectedRowsIndex)
            {
                if (Grid1.DataKeys[i][1].ToString() == memberid)
                    continue;
                str.Append(Grid1.DataKeys[i][0].ToString()+",");
            }
            if (str.Length == 0 || memberid.Length == 0) 
                return;
            str = str.Remove(str.Length - 1, 1);

            Hashtable ht = new Hashtable();
            ht.Add("dictmemberid",memberid);
            ht.Add("ordernums", str);
            try
            {
                bool flag = os.UpdateOrdersMemberInfoByName(ht);
                if (flag)
                {
                    MessageBoxShow("关联成功!", MessageBoxIcon.Information);
                    gdPastOrdersList.SelectedRowIndexArray = new int[] { };
                    Grid1.SelectedRowIndexArray = new int[] { };
                    BindData();
                }
                else
                {
                    MessageBoxShow("关联失败", MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBoxShow("关联出错，请联系管理员",MessageBoxIcon.Error);
            }
        }

        private void BindData()
        {
            ClearData();
            if (txtRealName.Text.Trim().Length == 0)
            {
                MessageBoxShow("请输入全名关联", MessageBoxIcon.Warning); 
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("realname", txtRealName.Text.Trim());
            ht.Add("sex", dpSex.SelectedValue.ToString());

            DataTable dt=os.GetMemeberPastOrders(ht);
            gdPastOrdersList.DataSource = dt;
            gdPastOrdersList.DataBind();
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }

        private void ClearData()
        {
            gdPastOrdersList.DataSource = new DataTable();
            gdPastOrdersList.DataBind();
            Grid1.DataSource = new DataTable();
            Grid1.DataBind();
        }
    }
}