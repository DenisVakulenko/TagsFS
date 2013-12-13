namespace TagsFS {
    partial class ucFile {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lblName = new System.Windows.Forms.Label();
            this.ltTags = new System.Windows.Forms.Panel();
            this.aftNewTag = new TagsFS.ucAddFileTag();
            this.ltTags.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(2, 2);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "label1";
            this.lblName.Click += new System.EventHandler(this.label1_Click);
            this.lblName.DoubleClick += new System.EventHandler(this.lblName_DoubleClick);
            // 
            // ltTags
            // 
            this.ltTags.BackColor = System.Drawing.Color.Transparent;
            this.ltTags.Controls.Add(this.aftNewTag);
            this.ltTags.Location = new System.Drawing.Point(2, 16);
            this.ltTags.Name = "ltTags";
            this.ltTags.Size = new System.Drawing.Size(460, 93);
            this.ltTags.TabIndex = 3;
            this.ltTags.Click += new System.EventHandler(this.ltTags_Click);
            // 
            // aftNewTag
            // 
            this.aftNewTag.BackColor = System.Drawing.Color.Transparent;
            this.aftNewTag.DB = null;
            this.aftNewTag.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aftNewTag.Location = new System.Drawing.Point(204, 23);
            this.aftNewTag.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.aftNewTag.Name = "aftNewTag";
            this.aftNewTag.Size = new System.Drawing.Size(112, 15);
            this.aftNewTag.TabIndex = 0;
            this.aftNewTag.AddFileTag += new System.EventHandler<TagsFS.ucAddFileTag.AddFileTagEventArgs>(this.ucAddFileTag1_AddFileTag);
            // 
            // ucFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ltTags);
            this.Controls.Add(this.lblName);
            this.Name = "ucFile";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(491, 167);
            this.Load += new System.EventHandler(this.ucFile_Load);
            this.Enter += new System.EventHandler(this.ucFile_Enter);
            this.Leave += new System.EventHandler(this.ucFile_Leave);
            this.Resize += new System.EventHandler(this.ucFile_Resize);
            this.ltTags.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Panel ltTags;
        private ucAddFileTag aftNewTag;
    }
}
