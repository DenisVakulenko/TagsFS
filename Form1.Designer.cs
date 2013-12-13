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
            this.txtFileSearch = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ltFiles = new System.Windows.Forms.FlowLayoutPanel();
            this.ltAttributes = new System.Windows.Forms.FlowLayoutPanel();
            this.ucAddFileAttr1 = new TagsFS.ucAddFileAttr();
            this.ltAttributes.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTagSearch
            // 
            this.txtTagSearch.Location = new System.Drawing.Point(12, 12);
            this.txtTagSearch.Name = "txtTagSearch";
            this.txtTagSearch.Size = new System.Drawing.Size(306, 20);
            this.txtTagSearch.TabIndex = 0;
            this.txtTagSearch.TextChanged += new System.EventHandler(this.txtTagSearch_TextChanged);
            // 
            // txtFileSearch
            // 
            this.txtFileSearch.Location = new System.Drawing.Point(324, 12);
            this.txtFileSearch.Name = "txtFileSearch";
            this.txtFileSearch.Size = new System.Drawing.Size(269, 20);
            this.txtFileSearch.TabIndex = 2;
            this.txtFileSearch.TextChanged += new System.EventHandler(this.txtFileSearch_TextChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 38);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(306, 410);
            this.flowLayoutPanel1.TabIndex = 4;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // ltFiles
            // 
            this.ltFiles.AutoScroll = true;
            this.ltFiles.Cursor = System.Windows.Forms.Cursors.Default;
            this.ltFiles.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ltFiles.Location = new System.Drawing.Point(324, 38);
            this.ltFiles.Name = "ltFiles";
            this.ltFiles.Size = new System.Drawing.Size(470, 410);
            this.ltFiles.TabIndex = 5;
            this.ltFiles.WrapContents = false;
            // 
            // ltAttributes
            // 
            this.ltAttributes.Controls.Add(this.ucAddFileAttr1);
            this.ltAttributes.Location = new System.Drawing.Point(800, 38);
            this.ltAttributes.Name = "ltAttributes";
            this.ltAttributes.Size = new System.Drawing.Size(288, 410);
            this.ltAttributes.TabIndex = 6;
            // 
            // ucAddFileAttr1
            // 
            this.ucAddFileAttr1.DB = null;
            this.ucAddFileAttr1.Location = new System.Drawing.Point(3, 3);
            this.ucAddFileAttr1.Name = "ucAddFileAttr1";
            this.ucAddFileAttr1.Size = new System.Drawing.Size(285, 23);
            this.ucAddFileAttr1.TabIndex = 0;
            this.ucAddFileAttr1.AddFileAttr += new System.EventHandler<TagsFS.ucAddFileAttr.AddFileAttrEventArgs>(this.ucAddFileAttr1_AddFileAttr);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 460);
            this.Controls.Add(this.ltAttributes);
            this.Controls.Add(this.ltFiles);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.txtFileSearch);
            this.Controls.Add(this.txtTagSearch);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ltAttributes.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTagSearch;
        private System.Windows.Forms.TextBox txtFileSearch;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel ltFiles;
        private System.Windows.Forms.FlowLayoutPanel ltAttributes;
        private ucAddFileAttr ucAddFileAttr1;
    }
}

