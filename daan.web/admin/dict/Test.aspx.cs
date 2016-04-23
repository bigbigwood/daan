using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.login;
using System.Data;
using daan.web.code;
using daan.domain;
using System.Collections;
using System.Text;
using daan.util.Web;
using daan.service.dict;
using ExtAspNet;
using System.Configuration;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;


namespace daan.web.admin.dict
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Write(Math.Pow(3,2));
            }
        }
    }
}