using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.domain;
using daan.service.dict;

namespace daan.web.usercontrol
{
    public partial class DropDictLab : System.Web.UI.UserControl
    {
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
            set { ddldictlab.SelectedValue = value; }
            get { return ddldictlab.SelectedValue; }
        }

        /// <summary>
        /// 获取选中项的text值
        /// </summary>
        public string SelectedText
        {
            get { return ddldictlab.SelectedText; }
        }

        /// <summary>
        /// 设置第一项[请选择]是否可选
        /// </summary>
        public bool Items0EnableSelect
        {
            get { return ddldictlab.Items[0].EnableSelect; }
            set { ddldictlab.Items[0].EnableSelect = value; }
        }

        /// <summary>
        /// 获取选中项(ExtAspNet.ListItem)
        /// </summary>
        public ExtAspNet.ListItem SelectedItem
        {
            get { return ddldictlab.SelectedItem; }
        }

        /// <summary>
        /// 设置ddldictlab选中项
        /// </summary>
        public int SelectIndex
        {
            set { ddldictlab.SelectedIndex = value; }
            get { return ddldictlab.SelectedIndex; }
        }

        /// <summary>
        /// 设置ddldictlab是否只读
        /// </summary>
        public bool Readonly
        {
            get { return ddldictlab.Readonly; }
            set { ddldictlab.Readonly = value; }
        }

        /// <summary>
        /// 设置ddldictlab是否可用
        /// </summary>
        public bool Enabled
        {
            get { return ddldictlab.Enabled; }
            set { ddldictlab.Enabled = value; }
        }

        #region 修改后加载事件（OnInit 执行在page_load之前 方便修改页面时先初始化用户控件 zhouy）
        protected override void OnInit(EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty(_labletext))
                    ddldictlab.Label = _labletext;

                ddldictlab.ShowLabel = _showlable;

                if (_width > 0)
                {
                    ddldictlab.Width = _width;
                }

                if (_height > 0)
                {
                    ddldictlab.Height = _height;
                }

                BindDrop();
            }

        }
        #endregion

        //原绑定事件
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void BindDrop()
        {
            DictlabService dictlabService = new DictlabService();
            IEnumerator<Dictlab> enumerator = dictlabService.GetDictlabList().GetEnumerator();
            ddldictlab.Items.Add(new ExtAspNet.ListItem("请选择", "-1"));

            while (enumerator.MoveNext())
            {
                ddldictlab.Items.Add(new ExtAspNet.ListItem(enumerator.Current.Labname, enumerator.Current.Dictlabid.ToString()));
            }
        }
    }
}