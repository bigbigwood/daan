using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.dict;
using daan.domain;

namespace daan.web.usercontrol
{
    public partial class DropCustomer : System.Web.UI.UserControl
    {

        private bool _showlable = false;
        private string _customertype = "";
        /// <summary>
        /// 是否显示标签
        /// </summary>
        public bool ShowLable
        {
            set { _showlable = value; }
        }

        private string _labletext;

        /// <summary>
        /// 标签值
        /// </summary>
        public string Lable
        {
            set { _labletext = value; }
        }

        private int _width;
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width
        {
            set { _width = value; }
        }

        private int _height;
        /// <summary>
        /// 高度
        /// </summary>
        public int Height
        {
            set { _height = value; }
        }

        /// <summary>
        /// 获取选中项的value值
        /// </summary>
        public string SelectedValue
        {
            set { ddlcustomer.SelectedValue = value; }
            get { return ddlcustomer.SelectedValue; }
        }

        /// <summary>
        /// 获取选中项的text值
        /// </summary>
        public string SelectedText
        {
            get { return ddlcustomer.SelectedText; }
        }

        /// <summary>
        /// 获取选中项(ExtAspNet.ListItem)
        /// </summary>
        public ExtAspNet.ListItem SelectedItem
        {
            get { return ddlcustomer.SelectedItem; }
        }

        /// <summary>
        /// 设置第一项[请选择]是否可选
        /// </summary>
        public bool Items0EnableSelect
        {
            get { return ddlcustomer.Items[0].EnableSelect; }
            set { ddlcustomer.Items[0].EnableSelect = value; }
        }

        /// <summary>
        /// 设置ddldictlab选中项
        /// </summary>
        public int SelectIndex
        {
            set { ddlcustomer.SelectedIndex = value; }
            get { return ddlcustomer.SelectedIndex; }
        }

        /// <summary>
        /// 设置ddldictlab是否可编辑
        /// </summary>
        public bool Readonly
        {
            get { return ddlcustomer.Readonly; }
            set { ddlcustomer.Readonly = value; }
        }
        /// <summary>
        /// 设置ddldictlab是否可用
        /// </summary>
        public bool Enabled
        {
            get { return ddlcustomer.Enabled; }
            set { ddlcustomer.Enabled = value; }
        }

        /// <summary>
        /// 客户类型
        /// </summary>
        public string CustomerType
        {
            get { return _customertype; }
            set { _customertype = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(_labletext))
                    ddlcustomer.Label = _labletext;

                ddlcustomer.ShowLabel = _showlable;

                if (_width > 0)
                    ddlcustomer.Width = _width;
                if (_height > 0)
                    ddlcustomer.Height = _height;
                BindDrop();
            }
        }

        private void BindDrop()
        {
            DictCustomerService dictcustomerService = new DictCustomerService();
            IEnumerator<Dictcustomer> enumerator = dictcustomerService.GetDictCustomerListByType(this.CustomerType).GetEnumerator();
            ddlcustomer.Items.Add(new ExtAspNet.ListItem("请选择", "-1"));

            while (enumerator.MoveNext())
            {
                ddlcustomer.Items.Add(new ExtAspNet.ListItem(enumerator.Current.Customername, enumerator.Current.Dictcustomerid.ToString()));
            }
            ddlcustomer.Items.Remove(new ExtAspNet.ListItem("请选择", "-1"));
            // ddlcustomer.Items.RemoveAt(0);
            ddlcustomer.Items[0].EnableSelect = true;

        }
    }
}