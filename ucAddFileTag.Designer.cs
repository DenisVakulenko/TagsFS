namespace TagsFS {
    partial class ucAddFileTag {
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
            this.txtTag = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtTag
            // 
            this.txtTag.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtTag.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtTag.BackColor = System.Drawing.SystemColors.Control;
            this.txtTag.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTag.Location = new System.Drawing.Point(1, 1);
            this.txtTag.Name = "txtTag";
            this.txtTag.Size = new System.Drawing.Size(113, 13);
            this.txtTag.TabIndex = 0;
            this.txtTag.TextChanged += new System.EventHandler(this.txtTag_TextChanged);
            this.txtTag.Enter += new System.EventHandler(this.txtTag_Enter);
            this.txtTag.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTag_KeyDown);
            this.txtTag.Leave += new System.EventHandler(this.txtTag_Leave);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.Location = new System.Drawing.Point(119, -1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(41, 17);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.TabStop = false;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ucAddFileTag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtTag);
            this.DoubleBuffered = true;
            this.Name = "ucAddFileTag";
            this.Size = new System.Drawing.Size(165, 17);
            this.Load += new System.EventHandler(this.ucAddFileTag_Load);
            this.BackColorChanged += new System.EventHandler(this.ucAddFileTag_BackColorChanged);
            this.FontChanged += new System.EventHandler(this.ucAddFileTag_FontChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ucAddFileTag_Paint);
            this.Resize += new System.EventHandler(this.ucAddFileTag_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTag;
        private System.Windows.Forms.Button btnAdd;
    }
}
