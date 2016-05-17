using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace daan.ui.PrintingApplication.Control
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
            cbCol.ReadOnly = false;
            dgv.Columns.Insert(0, cbCol);

            var rect = dgv.GetCellDisplayRectangle(0, -1, true);
            var checkBox = new System.Windows.Forms.CheckBox();
            checkBox.Name = "AllSelectCheckBox";
            checkBox.Size = new System.Drawing.Size(13, 13);
            checkBox.Location = new System.Drawing.Point(rect.Location.X + dgv.Columns[0].Width / 2 - 13 / 2 - 1, rect.Location.Y + 3);
            checkBox.CheckedChanged += new EventHandler(ckBox_CheckedChanged);
            dgv.Controls.Add(checkBox);
        }

        public static List<DataGridViewRow> GetSelectedRows()
        {
            List<DataGridViewRow> selectedRows = dgv.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells[0].EditedFormattedValue.ToString() == Boolean.TrueString)
                .ToList();

            return selectedRows;
        }

        static void ckBox_CheckedChanged(object sender, EventArgs e)
        {
            dgv.EndEdit();
            dgv.CurrentCell = null;
            bool isSelectAll = ((System.Windows.Forms.CheckBox)sender).Checked;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Value = isSelectAll;
            }
        }

        public static void Refresh()
        {
            var findControls = dgv.Controls.Find("AllSelectCheckBox", false).FirstOrDefault();
            if (findControls != null)
            {
                CheckBox allSelectCheckBox = findControls as CheckBox;
                allSelectCheckBox.Checked = false;
            }
        }
    }
}
