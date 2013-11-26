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
    public partial class ucFile : UserControl {
        public ucFile() {
            InitializeComponent();
        }
        public ucFile(TFSFile _File) {
            InitializeComponent();
            File = _File;
        }

        private TFSFile mFile;

        public TFSFile File {
            get {
                return mFile;
            }
            set {
                mFile = value;

                label1.Text = mFile.Path + "   id:" + mFile.ID.ToString();

                ltAttributes.Controls.Clear();
                if (mFile.Attrs != null)
                foreach (TFSFileAttribute FileAttr in mFile.Attrs) {
                    var Attribute = new ucFileAttribute(FileAttr);
                    //tb.TagClick += TagClick;
                    ltAttributes.Controls.Add(Attribute);
                }

                ltTags.Controls.Clear();
                if (mFile.Tags != null)
                foreach (TFSFileTag FileTag in mFile.Tags) {
                    var Tag = new ucTagBranch(FileTag.Tag);
                    //tb.TagClick += TagClick;
                    ltTags.Controls.Add(Tag);
                }
            }
        }
    }
}
