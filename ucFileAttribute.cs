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
    public partial class ucFileAttribute : UserControl {
        public class AttributeChangedEventArgs : EventArgs {
            public TFSFileAttribute FileAttribute { get; set; }
            public AttributeChangedEventArgs(TFSFileAttribute _FileAttribute) {
                FileAttribute = _FileAttribute;
            }
        }
        public delegate void AttributeChangedEventHandler(object sender, TFSFileAttribute e);
        public event EventHandler<AttributeChangedEventArgs> FileAttributeChanged;

        public ucFileAttribute() {
            InitializeComponent();
        }
        public ucFileAttribute(TFSFileAttribute _FileAttribute) {
            InitializeComponent();
            FileAttribute = _FileAttribute;
        }

        private TFSFileAttribute mFileAttribute;

        public TFSFileAttribute FileAttribute {
            get {
                return mFileAttribute;
            }
            set {
                mFileAttribute = value;

                lblName.Text = mFileAttribute.Attr.Name;
                txtValue.Text = mFileAttribute.Value;
            }
        }

        private void txtValue_TextChanged(object sender, EventArgs e) {
            
        }

        private void txtValue_Leave(object sender, EventArgs e) {
            mFileAttribute.Value = txtValue.Text;

            EventHandler<AttributeChangedEventArgs> handler = this.FileAttributeChanged;
            if (handler != null)
                handler(this, new AttributeChangedEventArgs(mFileAttribute));
        }
    }
}
