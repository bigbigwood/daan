using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace daan.ui.exportExcel
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        //登录
        private void button1_Click(object sender, EventArgs e)
        { 
            string u = ConfigurationSettings.AppSettings["UserName"];
            string p = ConfigurationSettings.AppSettings["Password"];
            string username = txtUsername.Text.Trim();
            string pwd = txtPwd.Text.Trim();
            if (username == "" || pwd == "")
            {
                MessageBox.Show("用户名或密码不能为空！");
                this.DialogResult = DialogResult.No;
            }
            else if (username != u || pwd != p)
            {
                MessageBox.Show("用户名或密码不正确！");
                this.DialogResult = DialogResult.No;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        //重置
        private void button2_Click(object sender, EventArgs e)
        {
            txtPwd.Text = string.Empty;
            txtUsername.Text = string.Empty;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
