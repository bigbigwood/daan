using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace daan.ui.PrintingApplication.Control
{
    public partial class ExtendProgressBar : UserControl
    {
        public ExtendProgressBar()
        {
            InitializeComponent();
        }

        public void ReportProgress(int nValue)
        {
            if (nValue >= 0)
            {
                if (nValue == 0)
                {
                    progressBar.Value = progressBar.Minimum;
                }
                else if (nValue < progressBar.Maximum)
                {
                    progressBar.Value = nValue;
                }
                else
                {
                    progressBar.Value = progressBar.Maximum;
                }
            }
        }

        private void ExtendProgressBar_Load(object sender, EventArgs e)
        {
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
        }
    }
}
