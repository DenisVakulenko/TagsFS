using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security;
using System.Security.AccessControl;


namespace TagsFS {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            //var newTags = new List<TFSTag>();
            //newTags.Add(new TFSTag("TEST5", 80));
            //newTags.Add(new TFSTag("TEST9"));

            //mTagsDB.CheckTags(ref newTags);
            //mTagsDB.AddTag("image");
            //mTagsDB.AddTag("song");

            //addFilesInDir(@"F:\");

            String Dir = "D:\\_RockGroup";
            //String Dir = "D:\\";
            var Tags = new List<TFSTag>();

            long PrevID = -1;
            foreach (String tag in Dir.Split('\\')) {
                if (tag != "") {
                    TFSTag CurrentTag = mTagsDB.AddTag(tag, PrevID);
                    PrevID = CurrentTag.ID;
                    Tags.Add(CurrentTag);
                }
            }

            mTagsDB.OpenConnection();
            addFilesInDir(Dir, PrevID, Tags);
            mTagsDB.CloseConnection();
            //foreach (TFSTag Tag in newTags)
            //    lstTags.Items.Add(Tag.ID.ToString() + "  " + Tag.Name + "  pid:" + Tag.ParentID.ToString());

            // Attributes test
            //mTagsDB.CheckAttribute("Definition");
            //mTagsDB.CheckAttribute("Definition");
            //mTagsDB.CheckAttribute("Definition", "Nice,Good");
            //mTagsDB.CheckAttribute("Definition", "Good,Cool,Bad");
            //mTagsDB.CheckAttribute("Definition");

            
        }

        public void TagClick(object sender, TagsFS.ucTagBranch.TagClickEventArgs e) {
            this.Text = e.Tag.Name;

            flowLayoutPanel1.Controls.Clear();

            List<TFSTag> Tags = mTagsDB.FindAllTagChildren(e.Tag);

            if (Tags == null) return;

            //lstTags.Items.Clear();
            foreach (TFSTag Tag in Tags) {
                //lstTags.Items.Add(Tag.Name);
                mTagsDB.GetTagHierarchy(Tag);
                var tb = new ucTagBranch(Tag);
                tb.TagClick += TagClick;
                flowLayoutPanel1.Controls.Add(tb);
            }
        }

        private void addFilesInDir(String Dir, long _ParentTagID = -1, List<TFSTag> _Tags = null) {
            try {
                var NewFiles = new List<TFSFile>();
                foreach (String file in Directory.GetFiles(Dir)) {
                    NewFiles.Add(new TFSFile(file, _Tags.Last()));
                }
                mTagsDB.CheckFiles(ref NewFiles);


                var NewFilesTags = new List<TFSFileTag>();
                foreach (TFSFile File in NewFiles) {
                    NewFilesTags.AddRange(File.Tags);
                }
                mTagsDB.CheckFilesTags(NewFilesTags);


                var SubDirsTags = new List<TFSTag>();
                foreach (String SubDir in Directory.GetDirectories(Dir)) {
                    SubDirsTags.Add(new TFSTag(Path.GetFileName(SubDir), _ParentTagID));
                }
                mTagsDB.CheckTags(ref SubDirsTags);


                int i = 0;
                foreach (String SubDir in Directory.GetDirectories(Dir)) {
                    List<TFSTag> NewTags = _Tags;
                    NewTags.Add(SubDirsTags[i]);

                    addFilesInDir(SubDir, SubDirsTags[i].ID, NewTags);
                    i++;
                }
            }
            catch { }
        }

        private void txtTagSearch_TextChanged(object sender, EventArgs e) {
            flowLayoutPanel1.Controls.Clear();

            List<TFSTag> Tags = mTagsDB.FindTagsLike(txtTagSearch.Text);

            if (Tags == null) return;

            //lstTags.Items.Clear();
            foreach (TFSTag Tag in Tags) {
                //lstTags.Items.Add(Tag.Name);
                mTagsDB.GetTagHierarchy(Tag);
                var tb = new ucTagBranch(Tag);
                tb.TagClick += TagClick;
                flowLayoutPanel1.Controls.Add(tb);
            }
        }

        private void txtFileSearch_TextChanged(object sender, EventArgs e) {
            List<TFSFile> Files = mTagsDB.FindFilesLike(txtFileSearch.Text);

            if (Files == null) return;

            lstFiles.Items.Clear();
            ltFiles.Controls.Clear();
            foreach (TFSFile File in Files) {
                lstFiles.Items.Add(File.Path);
                
                ltFiles.Controls.Add(new ucFile(File));
            }
        }
        
        private TagsDB mTagsDB = new TagsDB();
    }
}
