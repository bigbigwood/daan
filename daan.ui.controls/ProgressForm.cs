using System;
using System.Windows.Forms;

namespace daan.ui.controls
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
