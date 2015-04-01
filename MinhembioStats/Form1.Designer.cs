namespace MinhembioStats
{
    partial class mainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.listBox = new System.Windows.Forms.ListBox();
            this.buttonExportExcel = new System.Windows.Forms.Button();
            this.buttonUpdateAllGames = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelLastUpdated = new System.Windows.Forms.Label();
            this.labelMostVisitors = new System.Windows.Forms.Label();
            this.labelLeastVisitors = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(12, 12);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(274, 238);
            this.listBox.TabIndex = 0;
            // 
            // buttonExportExcel
            // 
            this.buttonExportExcel.Location = new System.Drawing.Point(300, 12);
            this.buttonExportExcel.Name = "buttonExportExcel";
            this.buttonExportExcel.Size = new System.Drawing.Size(140, 23);
            this.buttonExportExcel.TabIndex = 4;
            this.buttonExportExcel.Text = "Exportera till Excel";
            this.buttonExportExcel.UseVisualStyleBackColor = true;
            this.buttonExportExcel.Click += new System.EventHandler(this.buttonExportExcel_Click);
            // 
            // buttonUpdateAllGames
            // 
            this.buttonUpdateAllGames.Location = new System.Drawing.Point(300, 209);
            this.buttonUpdateAllGames.Name = "buttonUpdateAllGames";
            this.buttonUpdateAllGames.Size = new System.Drawing.Size(140, 23);
            this.buttonUpdateAllGames.TabIndex = 5;
            this.buttonUpdateAllGames.Text = "Uppdatera statistik";
            this.buttonUpdateAllGames.UseVisualStyleBackColor = true;
            this.buttonUpdateAllGames.Click += new System.EventHandler(this.buttonUpdateAllGames_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(300, 238);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(140, 12);
            this.progressBar.TabIndex = 6;
            this.progressBar.Visible = false;
            // 
            // labelLastUpdated
            // 
            this.labelLastUpdated.AutoSize = true;
            this.labelLastUpdated.Location = new System.Drawing.Point(297, 170);
            this.labelLastUpdated.Name = "labelLastUpdated";
            this.labelLastUpdated.Size = new System.Drawing.Size(103, 13);
            this.labelLastUpdated.TabIndex = 7;
            this.labelLastUpdated.Text = "Senast uppdaterad: ";
            // 
            // labelMostVisitors
            // 
            this.labelMostVisitors.AutoSize = true;
            this.labelMostVisitors.Location = new System.Drawing.Point(292, 48);
            this.labelMostVisitors.Name = "labelMostVisitors";
            this.labelMostVisitors.Size = new System.Drawing.Size(99, 13);
            this.labelMostVisitors.TabIndex = 8;
            this.labelMostVisitors.Text = "Flest nya besökare:";
            this.labelMostVisitors.Visible = false;
            // 
            // labelLeastVisitors
            // 
            this.labelLeastVisitors.AutoSize = true;
            this.labelLeastVisitors.Location = new System.Drawing.Point(292, 90);
            this.labelLeastVisitors.Name = "labelLeastVisitors";
            this.labelLeastVisitors.Size = new System.Drawing.Size(102, 13);
            this.labelLeastVisitors.TabIndex = 9;
            this.labelLeastVisitors.Text = "Minst nya besökare:";
            this.labelLeastVisitors.Visible = false;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 262);
            this.Controls.Add(this.labelLeastVisitors);
            this.Controls.Add(this.labelMostVisitors);
            this.Controls.Add(this.labelLastUpdated);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonUpdateAllGames);
            this.Controls.Add(this.buttonExportExcel);
            this.Controls.Add(this.listBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(468, 300);
            this.MinimumSize = new System.Drawing.Size(468, 300);
            this.Name = "mainForm";
            this.Text = "Minhembio Statistik";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Button buttonExportExcel;
        private System.Windows.Forms.Button buttonUpdateAllGames;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelLastUpdated;
        private System.Windows.Forms.Label labelMostVisitors;
        private System.Windows.Forms.Label labelLeastVisitors;
    }
}

