﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.ui.PrintingApplication.Helper
{
    public class FilePathHelper
    {
        public static string BuildPath(string path)
        {
            string fullPath = path;

            if (path.StartsWith("."))
                fullPath = string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, path.Substring(2)); //remove ".\"

            return fullPath;
        }
    }
}
