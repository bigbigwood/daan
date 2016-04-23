using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.DirectoryServices;
using ExtAspNet;
using System.Drawing.Printing;
using System.Management;
using System.Data;
using daan.service.dict;
using System.Collections;
using daan.domain;


namespace daan.web.admin.Setting
{

    public partial class LocalSettingInfo : PageBase
    {
        //本机MAC码ID
        //public string MacHost
        //{
        //    get { return (ViewState["MacHost"]).ToString() == "" ? null : (ViewState["MacHost"]).ToString(); }
        //    set { ViewState["MacHost"] = value; }
        //}
        string errorType = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExtAspNet.PageContext.RegisterStartupScript("GetPrinter();");
            }
            if (Request.Form["__EVENTTARGET"] == tbxPrint.ClientID && Request.Form["__EVENTARGUMENT"] == "specialkey")
            {
                BingDrop();
                BingText(); //获取本地MAC码和本机名称及打印机名称
            }
        }

        private void BingDrop()
        {
            try
            {
                string print = this.tbxPrint.Text.TrimEnd(',');
                string[] str = print.Split(',');
                Dictionary<string, string> pkInstalledPrinters = new Dictionary<string, string>();
                for (int i = 0; i < str.Count(); i++)
                {
                    pkInstalledPrinters.Add(str[i], str[i]);
                }

                this.Drop_A4PRINTER.DataSource = pkInstalledPrinters;
                this.Drop_A4PRINTER.DataTextField = "Key";
                this.Drop_A4PRINTER.DataValueField = "Value";
                this.Drop_A4PRINTER.DataBind();

                this.Drop_A5PRINTER.DataSource = pkInstalledPrinters;
                this.Drop_A5PRINTER.DataTextField = "Key";
                this.Drop_A5PRINTER.DataValueField = "Value";
                this.Drop_A5PRINTER.DataBind();

                this.Drop_BARCODEPRINTER.DataSource = pkInstalledPrinters;
                this.Drop_BARCODEPRINTER.DataTextField = "Key";
                this.Drop_BARCODEPRINTER.DataValueField = "Value";
                this.Drop_BARCODEPRINTER.DataBind();

                this.Drop_PDFPRINTER.DataSource = pkInstalledPrinters;
                this.Drop_PDFPRINTER.DataTextField = "Key";
                this.Drop_PDFPRINTER.DataValueField = "Value";
                this.Drop_PDFPRINTER.DataBind();

            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }



        private void BingText()
        {

            try
            {
                Initlocalsetting initlocalsettint = new Initlocalsetting();
                initlocalsettint.Hostmac = this.tbxMac.Text;    //MacHost;
                initlocalsettint = new InitlocalsettingService().GetInitlocalsettingInfo(initlocalsettint);
                if (initlocalsettint != null)
                {
                    this.Drop_A4PRINTER.SelectedValue = initlocalsettint.A4printer.ToString();
                    this.Drop_A5PRINTER.SelectedValue = initlocalsettint.A5printer.ToString();
                    this.Drop_BARCODEPRINTER.SelectedValue = initlocalsettint.Barcodeprinter.ToString();
                    this.Drop_PDFPRINTER.SelectedValue = initlocalsettint.Pdfprinter.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }


        }

        protected void btnSaveRefresh_Click(object sender, EventArgs e)
        {
            if (SaveDictlibrary())
            {
                BingText();
                MessageBoxShow("保存成功！");
            }
            else
            {
                BingText();
                MessageBoxShow(errorType, MessageBoxIcon.Error);
                return;
            }

        }


        //保存数据的逻辑方法
        public bool SaveDictlibrary()
        {
            try
            {
                Initlocalsetting initlocalsettint = new Initlocalsetting();
                initlocalsettint.Hostmac = this.tbxMac.Text;
                Initlocalsetting initlocalsettintList = new InitlocalsettingService().GetInitlocalsettingInfo(initlocalsettint);
                if (initlocalsettintList != null)
                {
                    initlocalsettintList.Pdfprinter = this.Drop_PDFPRINTER.SelectedValue;
                    initlocalsettintList.A4printer = this.Drop_A4PRINTER.SelectedValue;
                    initlocalsettintList.A5printer = this.Drop_A5PRINTER.SelectedValue;
                    initlocalsettintList.Barcodeprinter = this.Drop_BARCODEPRINTER.SelectedValue;
                }
                else
                {
                    initlocalsettintList = new Initlocalsetting();
                    initlocalsettintList.Hostmac = this.tbxMac.Text.Trim();
                    initlocalsettintList.Hostname = this.tbxName.Text.Trim();
                    initlocalsettintList.Pdfprinter = this.Drop_PDFPRINTER.SelectedValue;
                    initlocalsettintList.A4printer = this.Drop_A4PRINTER.SelectedValue;
                    initlocalsettintList.A5printer = this.Drop_A5PRINTER.SelectedValue;
                    initlocalsettintList.Barcodeprinter = this.Drop_BARCODEPRINTER.SelectedValue;
                }
                return new InitlocalsettingService().SaveDictlab(initlocalsettintList);
                
            }
            catch (Exception ex)
            {
                errorType = ex.Message;
                return false;
            }
        }



    }
}