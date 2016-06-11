namespace daan.ui.controls
{
    partial class SearchableComboBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SearchableComboBox
            // 
            this.FormattingEnabled = true;
            this.SelectedIndexChanged += new System.EventHandler(this.SearchableComboBox_SelectedIndexChanged);
            this.TextUpdate += new System.EventHandler(this.SearchableComboBox_TextUpdate);
            this.DropDownClosed += new System.EventHandler(this.SearchableComboBox_DropDownClosed);
            this.TextChanged += new System.EventHandler(this.SearchableComboBox_TextChanged);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
