using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagsFS {
    public partial class ucAddFileTag : UserControl {
        public class AddFileTagEventArgs : EventArgs {
            public TFSFileTag FileTag { get; set; }
            public AddFileTagEventArgs(TFSFileTag _FileTag) {
                FileTag = _FileTag;
            }
        }
        public delegate void AddFileTagEventHandler(object sender, AddFileTagEventArgs e);
        public event EventHandler<AddFileTagEventArgs> AddFileTag;


        public ucAddFileTag() {
            InitializeComponent();
        }
        public ucAddFileTag(TagsDB _DB) {
            InitializeComponent();
            DB = _DB;
        }

        TagsDB mDB = null;
        public TagsDB DB {
            get {
                return mDB;
            }
            set {
                mDB = value;

                if (mDB != null)
                foreach (var Tag in mDB.mTags)
                    txtTag.AutoCompleteCustomSource.Add(Tag.Value.Name);
            }
        }

        private void ucAddFileTag_Load(object sender, EventArgs e) {

        }

        private void ucAddFileTag_Resize(object sender, EventArgs e) {
            txtTag.Width = Width - btnAdd.Width;
            btnAdd.Left = txtTag.Right;
            btnAdd.Height = Height+2;

            //lineShape1.X2 = txtTag.Width;
            //lineShape1.Y1 = txtTag.Bottom;
            //lineShape1.Y2 = txtTag.Bottom;
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            TFSFileTag result;
            EventHandler<AddFileTagEventArgs> handler = this.AddFileTag;

            foreach (var Tag in DB.mTags)
            if (Tag.Value.Name.ToLower() == txtTag.Text.ToLower()) {
                result = new TFSFileTag(null, Tag.Value, 1, DB);
             
                if (handler != null)
                    handler(this, new AddFileTagEventArgs(result));

                return;
            }

            var NewTag = mDB.AddTag(txtTag.Text);
            if (NewTag == null) return;

            result = new TFSFileTag(null, NewTag, 1, DB);
            
            if (handler != null)
                handler(this, new AddFileTagEventArgs(result));

            txtTag.Text = "";
        }

        private void ucAddFileTag_FontChanged(object sender, EventArgs e) {
            txtTag.Font = Font;
            btnAdd.Font = Font;
            Height = txtTag.Height+1;
        }

        private void txtTag_TextChanged(object sender, EventArgs e) {

        }

        private void txtTag_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter)
                btnAdd_Click(sender, e);
        }

        bool txtTagFocus = false;
        private void txtTag_Enter(object sender, EventArgs e) {
            //lineShape1.BorderColor = Color.DarkGray;
            //txtTag.BackColor = Color.FromArgb(250, 250, 250);
            txtTagFocus = true;
            Refresh();
        }

        private void txtTag_Leave(object sender, EventArgs e) {
            //lineShape1.BorderColor = Color.LightGray;
            //txtTag.BackColor = SystemColors.Control;
            txtTagFocus = false;
            Refresh();
        }

        private void ucAddFileTag_BackColorChanged(object sender, EventArgs e) {
            txtTag.BackColor = BackColor;
        }

        private void ucAddFileTag_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            txtTag.BackColor = Parent.Parent.BackColor; // (Focused || Parent.Focused) ? Color.FromArgb(250, 250, 250) : SystemColors.Control;
            g.FillRectangle(new SolidBrush(Parent.BackColor), ClientRectangle);
            g.DrawLine(new Pen(txtTagFocus ? Color.DarkGray : Color.LightGray), 0, txtTag.Bottom, txtTag.Right - 1, txtTag.Bottom);
        }
    }
}
