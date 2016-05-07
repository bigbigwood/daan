using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace daan.ui.PrintingApplication
{
    public class VersioningManager
    {


        

        public void GetCurrentApplicationVersion()
        {
            string applicationVersionFile = ConfigurationManager.AppSettings.Get("ApplicationVersionFile");
        }

        public void GetServerApplicationVersion()
        {
            
        }

    }
}
