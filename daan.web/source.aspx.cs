using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace daan.web
{
    public partial class source : PageBase
    {
        private static Dictionary<string, string> SOURCE_FILES = new Dictionary<string, string>() { 
            {"data/grid_iframe.aspx", "data/grid_iframe_window.aspx"},
            {"data/tree.aspx", "data/Web.sitemap;data/website.xml"},
            {"aspnet/fileupload.aspx", "aspnet/fileupload_iframe.aspx"},
            {"iframe/button_iframe.aspx", "data/grid_iframe_window.aspx"},
            {"iframe/grid_iframe.aspx", "data/grid_iframe_window.aspx"},
            {"iframe/iframe_iframe.aspx", "iframe/iframe_iframe_window1.aspx;iframe/iframe_iframe_window2.aspx"},
            {"iframe/parent_postback.aspx", "iframe/parent_postback2.aspx;iframe/parent_postback3.aspx"},
            {"iframe/triggerbox_iframe.aspx", "iframe/triggerbox_iframe_iframe.aspx"},
            {"other/accordion_links.aspx", "other/accordion_links_iframe.htm"}
        };

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string files = Request.QueryString["files"];

                files = ResolveQueryString(files);

                if (!String.IsNullOrEmpty(files))
                {
                    string[] fileNames = files.Split(';');

                    for (int i = 0; i < fileNames.Length; i++)
                    {
                        string fileName = fileNames[i];
                        string shortFileName = GetShortFileName(fileName);
                        string iframeUrl = "./source_file.aspx?file=" + fileName;

                        ExtAspNet.Tab tab = new ExtAspNet.Tab();
                        tab.Title = shortFileName;
                        tab.EnableIFrame = true;
                        tab.IFrameUrl = iframeUrl;
                        tab.IconUrl = GetIconUrl(tab.IFrameUrl);
                        TabStrip1.Tabs.Add(tab);

                        // End with .aspx.
                        if (fileName.ToLower().EndsWith(".aspx"))
                        {
                            tab = new ExtAspNet.Tab();
                            tab.Title = shortFileName + ".cs";
                            tab.EnableIFrame = true;
                            tab.IFrameUrl = iframeUrl + ".cs";
                            tab.IconUrl = GetIconUrl(tab.IFrameUrl);
                            TabStrip1.Tabs.Add(tab);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string GetIconUrl(string url)
        {
            string suffix = url.Substring(url.LastIndexOf('.') + 1);
            return "~/images/filetype/vs_" + suffix + ".png";
            //string fileName = "vs_unknow.png";
            //if (url.EndsWith(".aspx"))
            //{
            //    fileName = "vs_aspx.png";
            //}
            //else if (url.EndsWith(".cs"))
            //{
            //    fileName = "vs_cs.png";
            //}
            //else if (url.EndsWith(".xml"))
            //{
            //    fileName = "vs_xml.png";
            //}
            //else if (url.EndsWith(".config"))
            //{
            //    fileName = "vs_config.png";
            //}
            //else if (url.EndsWith(".js"))
            //{
            //    fileName = "vs_js.png";
            //}
            //else if (url.EndsWith(".css"))
            //{
            //    fileName = "vs_css.png";
            //}
            //else if (url.EndsWith(".html") || url.EndsWith(".htm"))
            //{
            //    fileName = "vs_htm.png";
            //}
        }

        private string ResolveQueryString(string files)
        {
            string result = files;
            foreach (string key in SOURCE_FILES.Keys)
            {
                if (files.EndsWith(key))
                {
                    result += ';' + SOURCE_FILES[key];
                    break;
                }
            }
            return result;
        }

        private string GetShortFileName(string fileName)
        {
            int index = fileName.LastIndexOf("/");

            if (index >= 0)
            {
                return fileName.Substring(index + 1);
            }

            return fileName;
        }
    }
}