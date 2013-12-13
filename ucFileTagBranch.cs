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
    public partial class ucFileTagBranch : ucTagBranch {
        public class DeleteClickEventArgs : EventArgs {
            public TFSFileTag FileTag { get; set; }
            public DeleteClickEventArgs(TFSFileTag _FileTag) {
                FileTag = _FileTag;
            }
        }
        public delegate void DeleteClickEventHandler(object sender, DeleteClickEventArgs e);
        public event EventHandler<DeleteClickEventArgs> DeleteClick;

        public ucFileTagBranch() {
            InitializeComponent();
        }
        public ucFileTagBranch(TFSFileTag _Tag) {
            InitializeComponent();
            Tag = _Tag;
        }

        public TFSFileTag mFileTag;
        public TFSFileTag Tag {
            get {
                return mFileTag;
            }
            set {
                mTag = value.Tag;
                mFileTag = value;

                Graphics g = this.CreateGraphics();
                TFSTag t = mTag;
                PointF p = new PointF(0, 0);
                Font f = Font;// new Font("Segoe UI", 9);

                List<float> sizes = new List<float>();

                int i = 0;
                while (t != null) {
                    float size = g.MeasureString(t.Name, f).Width + 2;
                    sizes.Add(size);
                    p.X += sizes.Last();
                    t = t.ParentTag;
                }

                Width = (int)p.X + Height;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {

        }

        private void ucDeletableTagBranch_Resize(object sender, EventArgs e) {
            Tag = mFileTag;
            btnDelete.Size = new Size(Height - 2, Height - 1);
            btnDelete.Location = new Point(Width - btnDelete.Width - 1, 1);
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            EventHandler<DeleteClickEventArgs> handler = this.DeleteClick;
            if (handler != null)
                handler(this, new DeleteClickEventArgs(mFileTag));
        }

        private void ucFileTagBranch_Load(object sender, EventArgs e) {

        }

        private void btnDelete_MouseMove(object sender, MouseEventArgs e) {
            //Focus();
        }

        private void btnDelete_MouseEnter(object sender, EventArgs e) {
            mFocused = true;
            Refresh();
        }

        private void btnDelete_MouseLeave(object sender, EventArgs e) {
            mFocused = false;
            Refresh();
        }
    }
}
