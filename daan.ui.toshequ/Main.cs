using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using daan.ui.main;

namespace daan.ui.LisToshequ
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        //添加（重写）窗口尺寸变动函数Form1_Resize
        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)    //最小化到系统托盘      
            {
                notifyIcon1.Visible = true;
                //显示托盘图标         
                this.Hide();
                //隐藏窗口        
            }
        }
        //添加（重写）关闭窗口事件
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //注意判断关闭事件Reason来源于窗体按钮，否则用菜单退出时无法退出!   
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                //取消"关闭窗口"事件          
                this.WindowState = FormWindowState.Minimized;
                //使关闭时窗口向右下角缩小的效果             
                notifyIcon1.Visible = true;
                this.Hide();
                return;
            }
        }



        //添加双击托盘图标事件（双击显示窗口）
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            this.Focus();
        }



        private void 社区在线数据传输ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childrenForm in this.MdiChildren)
            {
                //检测是不是当前子窗体名称
                if (childrenForm.Name == "FrmUploadShequ88")
                {
                    //是的话就是把他显示   
                    childrenForm.Visible = true;           //并激活该窗体  
                    childrenForm.Activate();
                    childrenForm.WindowState = FormWindowState.Maximized;
                    return;
                }
            }

            FrmUploadShequ88 shequ88 = new FrmUploadShequ88();
            shequ88.MdiParent = this;
            shequ88.WindowState = FormWindowState.Maximized;
            shequ88.Show();
          
        }

        //系统托盘右键退出事件，打开验证密码窗体，输入密码正确，关闭主窗体
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Exit ex = new Exit();
            ex.ShowDialog();
        }

       
    }
}
