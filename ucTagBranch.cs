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
    public partial class ucTagBranch : UserControl {
        public class TagClickEventArgs : EventArgs {
            public TFSTag Tag { get; set; }
            public TagClickEventArgs(TFSTag _Tag) {
                Tag = _Tag;
            }
        }
        public delegate void TagClickEventHandler(object sender, TagClickEventArgs e);
        public event EventHandler<TagClickEventArgs> TagClick;

        public ucTagBranch() {
            InitializeComponent();
        }
        public ucTagBranch(TFSTag _Tag) {
            InitializeComponent();
            Tag = _Tag;
        }

        private void tfsTagBranch_Load(object sender, EventArgs e) {

        }

        private void tfsTagBranch_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            TFSTag t = mTag;
            PointF p = new PointF(0, 0);
            Font f = new Font("Segoe UI", 9);

            List<float> sizes = new List<float>();
            
            int i = 0;
            while (t != null) {
                float size = g.MeasureString(t.Name, f).Width + 2;
                sizes.Add(size);
                p.X += sizes.Last();
                t = t.ParentTag;
            }

            t = mTag;
            while (t != null) {
                i++;
                p.X -= sizes[i-1];
                g.DrawString(t.Name, f, new SolidBrush((t != mTagMM) ? Color.FromArgb(0, 0, 0) : Color.FromArgb(19, 130, 206)), p);
                
                t = t.ParentTag;
            }

            if (mTagMM != null) {
                Rectangle r = ClientRectangle;
                r.Size = r.Size - new Size(1, 1);
                g.DrawRectangle(new Pen(Color.FromArgb(50, 50, 50, 100)), r);
            }
        }

        private TFSTag mTagMM;
        private TFSTag mTag;
        
        public TFSTag Tag {
            get {
                return mTag;
            }
            set {
                mTag = value;

                Graphics g = this.CreateGraphics();
                TFSTag t = mTag;
                PointF p = new PointF(0, 0);
                Font f = new Font("Segoe UI", 9);

                List<float> sizes = new List<float>();

                int i = 0;
                while (t != null) {
                    float size = g.MeasureString(t.Name, f).Width + 2;
                    sizes.Add(size);
                    p.X += sizes.Last();
                    t = t.ParentTag;
                }

                Width = (int)p.X;
            }
        }

        private void tfsTagBranch_MouseMove(object sender, MouseEventArgs e) {
            Graphics g = this.CreateGraphics();
            TFSTag t = mTag;
            PointF p = new PointF(0, 0);
            Font f = new Font("Segoe UI", 9);

            List<float> sizes = new List<float>();

            int i = 0;
            while (t != null) {
                float size = g.MeasureString(t.Name, f).Width + 2;
                sizes.Add(size);
                p.X += sizes.Last();
                t = t.ParentTag;
            }

            t = mTag;
            while (t != null) {
                i++;
                p.X -= sizes[i - 1];
                if (e.X > p.X && e.X < p.X + sizes[i - 1]) {
                    mTagMM = t;
                    Refresh();
                    return;
                }

                t = t.ParentTag;
            }
            mTagMM = null;
            Refresh();
        }

        private void tfsTagBranch_MouseEnter(object sender, EventArgs e) {

        }

        private void tfsTagBranch_MouseLeave(object sender, EventArgs e) {
            mTagMM = null;
            Refresh();
        }

        private void tfsTagBranch_MouseUp(object sender, MouseEventArgs e) {
            EventHandler<TagClickEventArgs> handler = this.TagClick;
            if (handler != null)
                handler(this, new TagClickEventArgs(mTagMM));
        }
    }
}
