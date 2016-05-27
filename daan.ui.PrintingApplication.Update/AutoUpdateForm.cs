using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CCWin;
using log4net;

namespace daan.ui.PrintingApplication.Update
{
    public partial class AutoUpdateForm : CCSkinMain
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _fileFullPath;
        private string _url;

        public AutoUpdateForm()
        {
            InitializeComponent();
        }

        public void FillDownloadFileInfo(string fileName, string url)
        {
            string directory = string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "daan.ui.PrintingApplication.Update");
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            _fileFullPath = string.Format(@"{0}\daan.ui.PrintingApplication.Update\{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
            if (File.Exists(_fileFullPath))
            {
                // clean file if exist.
                File.Delete(_fileFullPath);
            }

            _url = url;
        }

        private void AutoUpdateForm_Load(object sender, EventArgs e)
        {
            var beginInvokeThread = new Thread(new ThreadStart(BackgroudWork_HttpDownloadFile));
            beginInvokeThread.Start();
        }

        private void InstallNewVersion()
        {
            string appPath = _fileFullPath;
            var process = new ProcessStartInfo();
            process.FileName = appPath;
            Process.Start(process);

            Application.Exit();
        }

        private void BackgroudWork_HttpDownloadFile()
        {
            var request = WebRequest.Create(_url) as HttpWebRequest; //设置参数
            var response = request.GetResponse() as HttpWebResponse; //发送请求并获取相应回应数据
            using (var responseStream = response.GetResponseStream()) //直到request.GetResponse()程序才开始向目标网页发送Post请求
            using (var stream = new FileStream(_fileFullPath, FileMode.Create)) //创建本地文件写入流
            {
                long totalDownloadedByte = 0;
                long totalBytes = response.ContentLength;
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    totalDownloadedByte += size;
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);

                    float percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                    Invoke(new Action(() => extendProgressBar.ReportProgress((int)percent)));
                }

                Invoke(new Action(() => extendProgressBar.ReportProgress(100)));
            }

            Invoke(new Action(InstallNewVersion));
        }
    }
}
