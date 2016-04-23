using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.domain;
using daan.service.dict;
using daan.service.login;

namespace daan.web.usercontrol
{
    public partial class DropInitBasic : System.Web.UI.UserControl
    {
        private string _basictype;

        /// <summary>
        /// 基础数据类型
        /// </summary>
        public string BasicType
        {
            set { _basictype = value; }
        }

        private int _type = 0;
        public int Type
        {
            set { _type = value; }
        }

        private bool _showlable = false;
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
        /// 设置/获取选中项的value值
        /// </summary>
        public string SelectedValue
        {
            get { return ddlbasic.SelectedValue; }
            set { ddlbasic.SelectedValue = value; }
        }
        /// <summary>
        /// 设置第一项[请选择]是否可选
        /// </summary>
        public ExtAspNet.ListItemCollection Items
        {
            get { return ddlbasic.Items; }
            //  set { ddlbasic.Items = value; }
        }

        /// <summary>
        /// 获取选中项的text值
        /// </summary>
        public string SelectedText
        {

            get { return ddlbasic.SelectedText; }
        }

        /// <summary>
        /// 获取选中项(ExtAspNet.ListItem)
        /// </summary>
        public ExtAspNet.ListItem SelectedItem
        {
            get { return ddlbasic.SelectedItem; }
        }

        /// <summary>
        /// 设置ddldictlab选中项
        /// </summary>
        public int SelectIndex
        {
            get { return ddlbasic.SelectedIndex; }
            set { ddlbasic.SelectedIndex = value; }
        }

        /// <summary>
        /// 设置ddldictlab是否可用
        /// </summary>
        public bool Enabled
        {
            get { return ddlbasic.Enabled; }
            set { ddlbasic.Enabled = value; }
        }

        /// <summary>
        /// 设置ddldictlab是否可用
        /// </summary>
        public bool Readonly
        {
            get { return ddlbasic.Readonly; }
            set { ddlbasic.Readonly = value; }
        }

        #region 修改后加载事件（OnInit 执行在page_load之前 方便修改页面时先初始化用户控件 zhouy）
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(_labletext))
                    ddlbasic.Label = _labletext;

                ddlbasic.ShowLabel = _showlable;

                if (_width > 0)
                {
                    ddlbasic.Width = _width;
                }

                if (_height > 0)
                {
                    ddlbasic.Height = _height;
                }

                if (!string.IsNullOrEmpty(_basictype))
                {
                    //if (_type != 0 && _basictype == "BILLHEADSTATUS")
                    //{
                    //    LoginService loginService = new LoginService();
                    //    List<Initbasic> listitem = loginService.GetLoginInitbasicList().FindAll(c => c.Basictype == _basictype && c.Basicname != "已作废");
                    //    ddlbasic.Items.Add(new ExtAspNet.ListItem("请选择", "-1"));
                    //    foreach (var item in listitem)
                    //    {
                    //        Initbasic dictlibraryitem = (Initbasic)item;
                    //        ddlbasic.Items.Add(new ExtAspNet.ListItem(dictlibraryitem.Basicname, dictlibraryitem.Basicvalue));
                    //    }
                    //}
                    //else
                    //{
                    LoginService loginService = new LoginService();
                    List<Initbasic> listitem = loginService.GetLoginInitbasicList().FindAll(c => c.Basictype == _basictype);
                    ddlbasic.Items.Add(new ExtAspNet.ListItem("请选择", "-1"));
                    foreach (var item in listitem)
                    {
                        Initbasic dictlibraryitem = (Initbasic)item;
                        ddlbasic.Items.Add(new ExtAspNet.ListItem(dictlibraryitem.Basicname, dictlibraryitem.Basicvalue));
                    }
                    //}
                }
            }
        }
        #endregion

        //原加载事件
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}
    }
}