namespace daan.ui.PrintingApplication.Control
{
    partial class ExtendProgressBar
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
            this.progressBar = new CCWin.SkinControl.SkinProgressBar();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Back = null;
            this.progressBar.BackColor = System.Drawing.Color.Transparent;
            this.progressBar.BarBack = null;
            this.progressBar.BarRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(0, 0);
            this.progressBar.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar.Name = "progressBar";
            this.progressBar.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.progressBar.Size = new System.Drawing.Size(500, 34);
            this.progressBar.TabIndex = 0;
            // 
            // ExtendProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progressBar);
            this.Name = "ExtendProgressBar";
            this.Size = new System.Drawing.Size(500, 34);
            this.Load += new System.EventHandler(this.ExtendProgressBar_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinProgressBar progressBar;
    }
}
