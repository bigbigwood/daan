using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace daan.ui.main
{
    public partial class Exit : Form
    {
        public Exit()
        {
            InitializeComponent();
        }

       
        //确认
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string strpath = ConfigurationManager.AppSettings["Exit"].ToString();
            if (this.txtPassword.Text.Equals(strpath))
            {
                MessageBox.Show("密码正确，程序即将关闭！","体检数据传输",MessageBoxButtons.OK,MessageBoxIcon.Information);
                //Application.Exit();
                System.Environment.Exit(0); 
            }
            else
            {
                MessageBox.Show("密码错误，您不能关闭程序！", "体检数据传输", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtPassword.Text = string.Empty;
            }
        }
    }
}
