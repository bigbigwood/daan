using System;
using System.Windows.Forms;

namespace daan.ui.controls
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
