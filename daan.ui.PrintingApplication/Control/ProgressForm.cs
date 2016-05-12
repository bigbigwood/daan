using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace daan.ui.PrintingApplication.Control
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
        }

        /// <summary>
        /// Increase process bar
        /// </summary>
        /// <param name="nValue">the value increased</param>
        /// <returns></returns>
        public bool Increase(int nValue)
        {
            if (nValue > 0)
            {
                if (progressBar.Value + nValue < progressBar.Maximum)
                {
                    progressBar.Value += nValue;
                    return true;
                }
                else
                {
                    progressBar.Value = progressBar.Maximum;
                    this.Close();
                    return false;
                }

            }
            return false;
        }

        public void ReportProgress(int nValue)
        {
            if (nValue > 0)
            {
                if (nValue < progressBar.Maximum)
                {
                    progressBar.Value = nValue;
                }
                else
                {
                    progressBar.Value = progressBar.Maximum;
                    this.Close();
                }
            }
        }
    }
}
