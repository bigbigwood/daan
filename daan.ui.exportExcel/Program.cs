using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace daan.ui.exportExcel
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Form f = new exportExcel.Main();
            //f.ShowDialog();
            //if (f.DialogResult == DialogResult.OK)
            //{
            //    Application.Run(new FrmExport());
            //}
            //else
            //{
            //    Application.Run(new Main());
            //}
            Application.Run(new FrmReport());
        }
    }
}
