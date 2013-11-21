namespace TagsFS
{
    partial class Form1
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
            this.txtTagSearch = new System.Windows.Forms.TextBox();
            this.lstTags = new System.Windows.Forms.ListBox();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.txtFileSearch = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtTagSearch
            // 
            this.txtTagSearch.Location = new System.Drawing.Point(12, 12);
            this.txtTagSearch.Name = "txtTagSearch";
            this.txtTagSearch.Size = new System.Drawing.Size(156, 20);
            this.txtTagSearch.TabIndex = 0;
            this.txtTagSearch.TextChanged += new System.EventHandler(this.txtTagSearch_TextChanged);
            // 
            // lstTags
            // 
            this.lstTags.FormattingEnabled = true;
            this.lstTags.Location = new System.Drawing.Point(12, 38);
            this.lstTags.Name = "lstTags";
            this.lstTags.Size = new System.Drawing.Size(156, 303);
            this.lstTags.TabIndex = 1;
            // 
            // lstFiles
            // 
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.Location = new System.Drawing.Point(174, 38);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(387, 303);
            this.lstFiles.TabIndex = 3;
            // 
            // txtFileSearch
            // 
            this.txtFileSearch.Location = new System.Drawing.Point(174, 12);
            this.txtFileSearch.Name = "txtFileSearch";
            this.txtFileSearch.Size = new System.Drawing.Size(156, 20);
            this.txtFileSearch.TabIndex = 2;
            this.txtFileSearch.TextChanged += new System.EventHandler(this.txtFileSearch_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 361);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.txtFileSearch);
            this.Controls.Add(this.lstTags);
            this.Controls.Add(this.txtTagSearch);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTagSearch;
        private System.Windows.Forms.ListBox lstTags;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.TextBox txtFileSearch;
    }
}

