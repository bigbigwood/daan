using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CCWin.SkinControl;
using log4net;

namespace daan.ui.controls
{
    public partial class SearchableComboBox : SkinComboBox
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //初始化绑定默认关键词（此数据源可以从数据库取）
        List<ListItem> listOnit = new List<ListItem>();
        //输入key之后，返回的关键词
        List<ListItem> listNew = new List<ListItem>();
        public string _showText = "";

        public SearchableComboBox()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public void BindCustomDataSource(List<ListItem> dataSource)
        {
            listOnit = dataSource;
            Items.AddRange(dataSource.ToArray());
            DisplayMember = "Value";
            ValueMember = "Key";
        }

        public string GetSelectedKey()
        {
            var selectedItem = SelectedItem as ListItem;
            return (selectedItem != null) ? selectedItem.Key : string.Empty;
        }

        private void SearchableComboBox_TextUpdate(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Text.Trim()))
            {
                Items.Clear();
                Items.AddRange(listOnit.ToArray());
                Cursor = Cursors.Default; //保持鼠标指针形状，有时候鼠标指针会被覆盖，所以要进行一次设置。
                return;
            }

            string text = this.Text;
            listNew = listOnit.FindAll(i => i.Value.Contains(text));

            if (listNew.Any())
            {
                Items.Clear();
                Items.AddRange(listNew.ToArray());

                //自动弹出下拉框
                if (!DroppedDown)
                {
                    DroppedDown = true;
                }

                //防止微软的bug
                this.Text = text;
            }
            else
            {
                DroppedDown = false;
            }

            SelectionStart = Text.Length; //设置光标位置，否则光标位置始终保持在第一列，造成输入关键词的倒序排列
            Cursor = Cursors.Default; //保持鼠标指针形状，有时候鼠标指针会被覆盖，所以要进行一次设置。
        }

        private void SearchableComboBox_TextChanged(object sender, EventArgs e)
        {
            ComboBox comb = sender as ComboBox;
            _showText = comb.Text;
        }

        private void SearchableComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox comb = sender as ComboBox;
            comb.Text = _showText;
            SendKeys.Send("{END}");
        }

        private void SearchableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comb = sender as ComboBox;
            _showText = comb.SelectedText;
        }

        private void SearchableComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)  // or Key.Enter or Key.Return
            {
                //Log.Info("enter key fired.");
                ComboBox comb = sender as ComboBox;
                //Log.InfoFormat("SelectedText={0}", comb.SelectedText);
                _showText = comb.SelectedText;
            }
        }
    }
}
