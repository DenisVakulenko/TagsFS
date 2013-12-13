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
        public class AttrributeChangedEventArgs : EventArgs {
            public TFSAttr Attr { get; set; }
            public AttrributeChangedEventArgs(TFSAttr _Attr) {
                Attr = _Attr;
            }
        }
        public delegate void TagClickEventHandler(object sender, AttrributeChangedEventArgs e);
        public event EventHandler<AttrributeChangedEventArgs> AttrributeChanged;

        public ucFile() {
            InitializeComponent();
        }
        public ucFile(TFSFile _File) {
            InitializeComponent();
            File = _File;
        }

        //private Boolean mShowAttributes = false;
        //public Boolean ShowAttributes {
        //    get {
        //        return ltAttributes.Visible;
        //    }
        //    set {
        //        ltAttributes.Visible = value;
        //        ucFile_Resize(this, new EventArgs());
        //    }
        //}

        private TFSFile mFile;
        public TFSFile File {
            get {
                return mFile;
            }
            set {
                mFile = value;

                String SongBy = mFile.GetAttributeValue("Song By");
                String SongName = mFile.GetAttributeValue("Song Name");
                if (SongBy != null && SongName != null)
                    lblName.Text = SongBy + " - " + SongName;
                else
                    lblName.Text = mFile.Path; // + "   id:" + mFile.ID.ToString();


                aftNewTag.DB = mFile.DB;

                Point loc = new Point(10, 0);
                while (ltTags.Controls.Count > 1) ltTags.Controls.RemoveAt(1);
                if (mFile.Tags != null) {
                    ucFileTagBranch Tag = null;
                    foreach (TFSFileTag FileTag in mFile.Tags) {
                        Tag = new ucFileTagBranch(FileTag);
                        Tag.DeleteClick += Tag_DeleteClick;
                        Tag.Font = new Font("Segoe UI", 8);
                        Tag.ForeColor = Color.FromArgb(210, 40, 40, 40);
                        ltTags.Controls.Add(Tag);
                        if (Tag.Width + loc.X > ltTags.Width) {
                            loc.X = 10;
                            loc.Y += Tag.Height;
                        }
                        Tag.Location = loc;
                        loc.X += Tag.Width + 7;
                    }
                    if (aftNewTag.Width + loc.X > ltTags.Width) {
                        loc.X = 10;
                        loc.Y += Tag.Height;
                    }
                    aftNewTag.Left = ltTags.Width - aftNewTag.Width;
                    aftNewTag.Top = loc.Y;

                    loc.X += aftNewTag.Width;
                    loc.Y += aftNewTag.Height;
                }
                else {
                    aftNewTag.Left = ltTags.Width - aftNewTag.Width;
                    aftNewTag.Top = loc.Y;

                    loc.X += aftNewTag.Width;
                    loc.Y += aftNewTag.Height;
                }

                Height = loc.Y + ltTags.Top + 2;
                MinimumSize = new Size(0, loc.Y + ltTags.Top + 2);
            }
        }

        void Tag_DeleteClick(object sender, ucFileTagBranch.DeleteClickEventArgs e) {
            e.FileTag.Delete();
            File = mFile;
        }

        private void label1_Click(object sender, EventArgs e) {
            Focus();
        }

        private void ucFile_Resize(object sender, EventArgs e) {
            ltTags.Width = Width - 4;
            ltTags.Height = Height - ltTags.Top - 2;
            File = mFile;
            //aftNewTag.Left = ltTags.Width - aftNewTag.Width;
        }

        private void ucAddFileTag1_AddFileTag(object sender, ucAddFileTag.AddFileTagEventArgs e) {
            e.FileTag.File = mFile;
            e.FileTag.FixInBase();
            if (mFile.Tags == null) mFile.Tags = new List<TFSFileTag>();
            mFile.Tags.Add(e.FileTag);

            File = mFile;
        }

        private void ucFile_Enter(object sender, EventArgs e) {
            BackColor = Color.FromArgb(250, 250, 250);
            Refresh();
            aftNewTag.Refresh();
            //ltTags.BackColor = Color.FromArgb(250, 250, 250);
        }

        private void ucFile_Leave(object sender, EventArgs e) {
            BackColor = SystemColors.Control;
            Refresh();
            //ltTags.BackColor = SystemColors.Control;
        }

        private void ltTags_Click(object sender, EventArgs e) {
            this.Focus();
        }

        private void lblName_DoubleClick(object sender, EventArgs e) {
            System.Diagnostics.Process.Start(mFile.Path);
        }

        private void ucFile_Load(object sender, EventArgs e) {

        }
    }
}
