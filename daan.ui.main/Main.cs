using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;


namespace daan.ui.main
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
            //shequ88.ShowDialog();


        }
        private void 达安Lis数据传输ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childrenForm in this.MdiChildren)
            {
                //检测是不是当前子窗体名称
                if (childrenForm.Name == "FrmSendOrdersToLis")
                {
                    //是的话就是把他显示   
                    childrenForm.Visible = true;           //并激活该窗体  
                    childrenForm.Activate();
                    childrenForm.WindowState = FormWindowState.Maximized;
                    return;
                }
            }
            FrmSendOrdersToLis form2 = new FrmSendOrdersToLis();
            form2.MdiParent = this;
            form2.WindowState = FormWindowState.Maximized;
            form2.Show();
            //form2.ShowDialog();
        }

        //系统托盘右键退出事件，打开验证密码窗体，输入密码正确，关闭主窗体
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Exit ex = new Exit();
            ex.ShowDialog();
        }

        private void lIS异常订单上传体检ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childrenForm in this.MdiChildren)
            {
                //检测是不是当前子窗体名称
                if (childrenForm.Name == "FrmException")
                {
                    //是的话就是把他显示   
                    childrenForm.Visible = true;           //并激活该窗体  
                    childrenForm.Activate();
                    childrenForm.WindowState = FormWindowState.Maximized;
                    return;
                }
            }
            FrmException form2 = new FrmException();
            form2.MdiParent = this;
            form2.WindowState = FormWindowState.Maximized;
            form2.Show();
        }

        private void 自动小结ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childrenForm in this.MdiChildren)
            {
                //检测是不是当前子窗体名称
                if (childrenForm.Name == "FrmXiaoJie")
                {
                    //是的话就是把他显示   
                    childrenForm.Visible = true;           //并激活该窗体  
                    childrenForm.Activate();
                    childrenForm.WindowState = FormWindowState.Maximized;
                    return;
                }
            }
            FrmXiaoJie form2 = new FrmXiaoJie();
            form2.MdiParent = this;
            form2.WindowState = FormWindowState.Maximized;
            form2.Show();
        }

        private void 批量单位上传ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childrenForm in this.MdiChildren)
            {
                //检测是不是当前子窗体名称
                if (childrenForm.Name == "FrmUploadFileDetail")
                {
                    //是的话就是把他显示   
                    childrenForm.Visible = true;           //并激活该窗体  
                    childrenForm.Activate();
                    childrenForm.WindowState = FormWindowState.Maximized;
                    return;
                }
            }
            FrmUploadFileDetail form2 = new FrmUploadFileDetail();
            form2.MdiParent = this;
            form2.WindowState = FormWindowState.Maximized;
            form2.Show();
        }

        private void tM统计查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childredForm in MdiChildren)
            {
                if (childredForm.Name == "FrmTMStat")
                {
                    childredForm.Visible = true;
                    childredForm.Activate();
                    childredForm.WindowState = FormWindowState.Maximized;
                    return;
                }
            }
            FrmTMStat tm = new FrmTMStat();
            tm.MdiParent = this;
            tm.WindowState = FormWindowState.Maximized;
            tm.Show();
        }

        private void 康源项目对照同步ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in MdiChildren)
            {
                if (f.Name == "FrmTestitems")
                {
                    f.Visible = true;
                    f.Activate();
                    f.WindowState = FormWindowState.Maximized;
                    return;
                }
            }
            FrmTestitems items = new FrmTestitems();
            items.MdiParent = this;
            items.WindowState = FormWindowState.Maximized;
            items.Show();
        }

        private void 自动初步总检ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in MdiChildren)
            {
                if (f.Name == "FrmAutoChuBu")
                {
                    f.Visible = true;
                    f.Activate();
                    f.WindowState = FormWindowState.Maximized;
                    return;
                }
            }
            FrmAutoChuBu items = new FrmAutoChuBu();
            items.MdiParent = this;
            items.WindowState = FormWindowState.Maximized;
            items.Show();
        }

        private void 同步JsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in MdiChildren)
            {
                if (f.Name == "FrmJsonFilesWatcher")
                {
                    f.Visible = true;
                    f.Activate();
                    f.WindowState = FormWindowState.Maximized;
                    return;
                }
            }
            FrmJsonFilesWatcher fm = new FrmJsonFilesWatcher();
            fm.MdiParent = this;
            fm.WindowState = FormWindowState.Maximized;
            fm.Show();
        }
       
    }
}
