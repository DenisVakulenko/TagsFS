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
            
            String Dir = "F:\\_ univer";
            var Tags = new List<TFSTag>();

            long PrevID = -1;
            foreach (String tag in Dir.Split('\\')) {
                TFSTag CurrentTag = mTagsDB.AddTag(tag, PrevID);
                PrevID = CurrentTag.ID;
                Tags.Add(CurrentTag);
            }

            addFilesInDir(Dir, PrevID, Tags);

            //foreach (TFSTag Tag in newTags)
            //    lstTags.Items.Add(Tag.ID.ToString() + "  " + Tag.Name + "  pid:" + Tag.ParentID.ToString());
        }

        private void addFilesInDir(String Dir, long _ParentTagID = -1, List<TFSTag> _Tags = null) {
            var NewFiles = new List<TFSFile>();

            foreach (String file in Directory.GetFiles(Dir)) {
                NewFiles.Add(new TFSFile(file, _Tags));
            }

            mTagsDB.CheckFiles(ref NewFiles);

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

        private TagsDB mTagsDB = new TagsDB();

        private void txtTagSearch_TextChanged(object sender, EventArgs e) {

        }
    }
}
