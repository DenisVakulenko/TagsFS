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
    public partial class ucAddFileAttr : UserControl {
        public class AddFileAttrEventArgs : EventArgs {
            public TFSFileAttribute FileAttr { get; set; }
            public AddFileAttrEventArgs(TFSFileAttribute _FileAttr) {
                FileAttr = _FileAttr;
            }
        }
        public delegate void AddFileAttrEventHandler(object sender, AddFileAttrEventArgs e);
        public event EventHandler<AddFileAttrEventArgs> AddFileAttr;


        public ucAddFileAttr() {
            InitializeComponent();
        }
        public ucAddFileAttr(TagsDB _DB) {
            InitializeComponent();
            DB = _DB;
        }

        private void ucAddFileAttr_Load(object sender, EventArgs e) {

        }

        private void ucAddFileAttr_Resize(object sender, EventArgs e) {
            int w = (Width - btnAdd.Width - 4) / 2;
            cmbAttr.Width = w;
            txtValue.Width = w;
            txtValue.Location = new Point(w + 2, 1);
            btnAdd.Location = new Point(Width - btnAdd.Width - 1);
        }

        protected TagsDB mDB;
        public TagsDB DB {
            get {
                return mDB;
            }
            set {
                if (mDB == value) return;
                mDB = value;
                cmbAttr.Items.Clear();
                if (mDB == null) return;
                foreach (TFSAttr Attr in mDB.mAttrs) {
                    cmbAttr.Items.Add(Attr.Name);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (cmbAttr.SelectedIndex < 0) return;
            TFSAttr Attr = DB.mAttrs[cmbAttr.SelectedIndex];
            TFSFileAttribute result = new TFSFileAttribute(null, Attr, txtValue.Text, 1, DB);
            txtValue.Text = "";

            EventHandler<AddFileAttrEventArgs> handler = this.AddFileAttr;
            if (handler != null)
                handler(this, new AddFileAttrEventArgs(result));
        }
    }
}
