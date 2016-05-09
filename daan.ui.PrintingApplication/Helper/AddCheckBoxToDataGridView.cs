using System;
using System.Windows.Forms;

namespace daan.ui.PrintingApplication.Helper
{
    public class AddCheckBoxToDataGridView
    {
        public static System.Windows.Forms.DataGridView dgv;
        public static void AddFullSelect()
        {
            var cbCol = new DataGridViewCheckBoxColumn();
            cbCol.Width = 50;
            cbCol.HeaderText = "";
            cbCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            cbCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv.Columns.Insert(0, cbCol);

            var rect = dgv.GetCellDisplayRectangle(0, -1, true);
            var checkBox = new System.Windows.Forms.CheckBox();
            checkBox.Text = "checkboxHeader";
            checkBox.Size = new System.Drawing.Size(13, 13);
            checkBox.Location = new System.Drawing.Point(rect.Location.X + dgv.Columns[0].Width / 2 - 13 / 2 - 1, rect.Location.Y + 3);
            checkBox.CheckedChanged += new EventHandler(ckBox_CheckedChanged);
            dgv.Controls.Add(checkBox);
        }

        static void ckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dgv.Rows)
            {
                dr.Cells[0].Value = ((System.Windows.Forms.CheckBox)sender).Checked;
            }
        }
    }
}
