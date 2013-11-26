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
            this.ltAttributes = new System.Windows.Forms.FlowLayoutPanel();
            this.ltTags = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ltAttributes
            // 
            this.ltAttributes.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ltAttributes.Location = new System.Drawing.Point(3, 32);
            this.ltAttributes.Name = "ltAttributes";
            this.ltAttributes.Size = new System.Drawing.Size(255, 100);
            this.ltAttributes.TabIndex = 0;
            this.ltAttributes.WrapContents = false;
            // 
            // ltTags
            // 
            this.ltTags.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ltTags.Location = new System.Drawing.Point(264, 32);
            this.ltTags.Name = "ltTags";
            this.ltTags.Size = new System.Drawing.Size(285, 100);
            this.ltTags.TabIndex = 1;
            this.ltTags.WrapContents = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // ucFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ltTags);
            this.Controls.Add(this.ltAttributes);
            this.Name = "ucFile";
            this.Size = new System.Drawing.Size(552, 135);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel ltAttributes;
        private System.Windows.Forms.FlowLayoutPanel ltTags;
        private System.Windows.Forms.Label label1;
    }
}
