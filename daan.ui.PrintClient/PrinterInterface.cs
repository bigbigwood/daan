using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace daan.ui.PrinterApplication
{
    public class PrinterInterface
    {
        private Font printFont;            //字体 
        private TextReader streamToPrint;  //可读取连续系列的读取器 

        //从文件打印的接口 
        public void PringFile(string filePath, bool preview)
        {
            PrintOut(filePath, true, preview);
        }
        //从字符数据打印的接口 
        public void Print(string text, bool preview)
        {
            PrintOut(text, false, preview);
        }
        /// <summary> 
        /// 打印准备 
        /// </summary> 
        /// <param name="data">要打印的数据</param> 
        /// <param name="file">是否是来自文件</param> 
        /// <param name="preview">是否预览</param> 
        private void PrintOut(string data, bool file, bool preview)
        {
            try
            {
                //如果数据来自文件 则用StreemReader来读取，否则用StringReader来读取 
                using (streamToPrint = file ? (TextReader)new StreamReader(data) : (TextReader)new StringReader(data))
                {
                    //string printerName = "发送至 OneNote 2013";
                    string printerName = "HP 910";
                    //设置要打印的字体样式和高度 
                    printFont = new Font("Arial", 10);
                    //实例化一个PrintDocument对象 
                    PrintDocument pd = new PrintDocument();
                    pd.PrinterSettings.PrinterName = printerName;
                    //注册要讲当前页输出到打印机的PrintPage事件 
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    if (preview)
                    {
                        //实例化一个预览框 
                        PrintPreviewDialog dlg = new PrintPreviewDialog();
                        dlg.Document = pd;
                        dlg.ShowDialog();
                    }
                    else
                        pd.Print();//开始打印，在将当前页输出前调用PrintPage事件 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //事件处理函数，逐行打印 
        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            float line = 0;  //总行数 
            float yPos = 0;          //Y轴高度 
            int count = 0;           //当前正在打印第几行 
            float leftM = e.MarginBounds.Left;  //在打印范围内获取左边的打印点 
            float topT = e.MarginBounds.Top;    //在打印范围内获取上边的打印点 
            string lineStr = null;       //存储一行数据 
            line = e.MarginBounds.Height / printFont.GetHeight(e.Graphics); // 打印区域高度/字体高度 
            //如果没有超过最大行 并且 还存在一行数据 就开始打印 
            while (count < line && ((lineStr = streamToPrint.ReadLine()) != null))
            {
                //因为每打印一行，下一行的位置会发生变化， yPos=打印区域的上边界+ 行数*字体高度 
                yPos = topT + (count * printFont.GetHeight(e.Graphics));
                //打印一行 数据，leftM 和 yPos代表了 打印的起始点坐标 
                e.Graphics.DrawString(lineStr, printFont, Brushes.Black, leftM, yPos, new StringFormat());
                count++; //下一行 
            }
            if (lineStr != null) //如果还有内容 另换一页 
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
        } 
    }
}
