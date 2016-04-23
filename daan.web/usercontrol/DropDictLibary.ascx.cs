using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.domain;
using daan.service.dict;

namespace daan.web.usercontrol
{
    public partial class DropDictLibary : System.Web.UI.UserControl
    {
        private string _libarycode;

        /// <summary>
        /// 基础数据类型
        /// </summary>
        public string LibraryCode
        {
            set { _libarycode = value; }
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
        /// 获取选中项的value值
        /// </summary>
        public string SelectedValue
        {
            get { return ddllib.SelectedValue; }
        }

        /// <summary>
        /// 获取选中项的text值
        /// </summary>
        public string SelectedText
        {
            get { return ddllib.SelectedText; }
        }

        /// <summary>
        /// 获取选中项(ExtAspNet.ListItem)
        /// </summary>
        public ExtAspNet.ListItem SelectedItem
        {
            get { return ddllib.SelectedItem; }
        }

        /// <summary>
        /// 设置选中项SelectedIndex
        /// </summary>
        public int SelectIndex
        {
            get { return ddllib.SelectedIndex; }
            set { ddllib.SelectedIndex = value; }
        }

        #region 修改后加载事件（OnInit 执行在page_load之前 方便修改页面时先初始化用户控件 zhouy）
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(_labletext))
                    ddllib.Label = _labletext;

                ddllib.ShowLabel = _showlable;

                if (_width > 0)
                {
                    ddllib.Width = _width;
                }

                if (_height > 0)
                {
                    ddllib.Height = _height;
                }

                if (!string.IsNullOrEmpty(_libarycode))
                {
                    DictLibraryItemService itemservice = new DictLibraryItemService();
                    IList listitem = itemservice.SelectDictlibraryitemByCode(_libarycode);
                    ddllib.Items.Add(new ExtAspNet.ListItem("请选择", "-1"));
                    foreach (var item in listitem)
                    {
                        Dictlibraryitem dictlibraryitem = (Dictlibraryitem)item;
                        ddllib.Items.Add(new ExtAspNet.ListItem(dictlibraryitem.Itemname, dictlibraryitem.Dictlibraryitemid.ToString()));
                    }
                }
            }
        }
        #endregion
       

        //原绑定事件
        protected void Page_Load(object sender, EventArgs e)
        {


        }
    }
}