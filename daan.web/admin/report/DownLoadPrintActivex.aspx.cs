using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Threading;
using System.Text;

namespace daan.web.admin.report
{
    public partial class DownLoadPrintActivex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //private string fileName = HttpContext.Current.Server.UrlEncode("ActiveXPrintSetup.rar");
        //private string filePath = HttpContext.Current.Request.PhysicalApplicationPath + "download\\ActiveXPrintSetup.rar";
        //string tt = "";

        //protected void btnDownLoad_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        FileInfo info = new FileInfo(filePath);
        //        long fileSize = info.Length;
        //        Response.Clear();
        //        Response.ContentType = "application/x-zip-compressed";
        //        Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
        //        //不指明Content-Length用Flush的话不会显示下载进度 
        //        Response.AddHeader("Content-Length", fileSize.ToString());
        //        Response.TransmitFile(filePath, 0, fileSize);
        //        Response.Flush();
        //        Response.Close();
        //    }
        //    catch
        //    { }
        //}


    }
}