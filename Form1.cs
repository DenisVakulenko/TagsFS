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

            return;
            //addFilesInDir(@"F:\");

            //String Dir = "D:\\_RockGroup";
            String Dir = "D:\\";
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
            //lstTags.Items.Add(Tag.ID.ToString() + "  " + Tag.Name + "  pid:" + Tag.ParentID.ToString());

            // Attributes test
            //mTagsDB.CheckAttribute("Definition");
            //mTagsDB.CheckAttribute("Definition");
            //mTagsDB.CheckAttribute("Definition", "Nice,Good");
            //mTagsDB.CheckAttribute("Definition", "Good,Cool,Bad");
            //mTagsDB.CheckAttribute("Definition");

            mTagsDB.LoadAllTags();
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

                var NewFilesAttributes = mTagsDB.GenerateFileAttributes(NewFiles);
                mTagsDB.CheckFilesAttributes(NewFilesAttributes);
                
                var NewFilesTags = new List<TFSFileTag>();
                mTagsDB.GenerateFileTags(NewFiles); //NewFilesTags.AddRange(mTagsDB.GenerateFileTags(NewFiles));    
                foreach (TFSFile File in NewFiles) {
                    NewFilesTags.AddRange(File.Tags);
                }
                mTagsDB.CheckFilesTags(NewFilesTags);

                var SubDirsTags = new List<TFSTag>();
                foreach (String SubDir in Directory.GetDirectories(Dir)) {
                    SubDirsTags.Add(new TFSTag(Path.GetFileName(SubDir), _ParentTagID));
                }
                mTagsDB.CheckTags(SubDirsTags);


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
            Text = "FindFilesLike";
            String fileswhere = "";
            String tagswhere = "";
            String attrswhere = "";
            if (txtFileSearch.Text == "") return;
            var prms = txtFileSearch.Text.Split(' ');
            foreach (var prm in prms) {
                if (prm != "") {
                    //tagswhere += "name REGEXP '[[:<:]]" + prm + "[[:>:]]' AND ";
                    tagswhere += "([name] LIKE '% " + prm + " %' OR ";
                    tagswhere += "[name] LIKE  '% " + prm + "' OR ";
                    tagswhere += "[name] LIKE  '" + prm + " %' OR ";

                    tagswhere += "[name] LIKE  '% " + prm + "/_%' ESCAPE '/' OR ";
                    tagswhere += "[name] LIKE  '%/_" + prm + " %' ESCAPE '/' OR ";

                    tagswhere += "[name] LIKE  '%/_" + prm + "/_%' ESCAPE '/' OR ";
                    tagswhere += "[name] LIKE  '%/_" + prm + "' ESCAPE '/' OR ";
                    tagswhere += "[name] LIKE  '" + prm + "/_%' ESCAPE '/' OR ";

                    tagswhere += "[name] LIKE  '" + prm + "') AND ";


                    attrswhere += "([value] LIKE '% " + prm + " %' OR ";
                    attrswhere += "[value] LIKE  '% " + prm + "' OR ";
                    attrswhere += "[value] LIKE  '" + prm + " %' OR ";

                    attrswhere += "[value] LIKE  '% " + prm + "/_%' ESCAPE '/' OR ";
                    attrswhere += "[value] LIKE  '%/_" + prm + " %' ESCAPE '/' OR ";

                    attrswhere += "[value] LIKE  '%/_" + prm + "/_%' ESCAPE '/' OR ";
                    attrswhere += "[value] LIKE  '%/_" + prm + "' ESCAPE '/' OR ";
                    attrswhere += "[value] LIKE  '" + prm + "/_%' ESCAPE '/' OR ";

                    attrswhere += "[value] LIKE  '" + prm + "') AND ";

                    fileswhere += "path LIKE '%" + prm + "%' AND ";

                    //if (prm.Substring(0, 4) == "tag:")
                    //    tagswhere += "name LIKE '" + prm.Substring(4) + "'";
                    //else if (prm.Substring(0, 5) == "attr:")
                    //    attrswhere += "value LIKE '" + prm.Substring(5) + "'";
                    //else
                    //    fileswhere += "path LIKE '" + Name + "'";
                }
            }
            tagswhere  = tagswhere.Substring(0, tagswhere.Length - 5);
            attrswhere = attrswhere.Substring(0, attrswhere.Length - 5);
            fileswhere = fileswhere.Substring(0, fileswhere.Length - 5);

            var Tags = mTagsDB.FindTagsWhere(tagswhere);

            flowLayoutPanel1.Controls.Clear();
            if (Tags != null)
                foreach (TFSTag Tag in Tags) {
                    //lstTags.Items.Add(Tag.Name);
                    //mTagsDB.GetTagHierarchy(Tag);
                    var tb = new ucTagBranch(Tag);
                    tb.TagClick += TagClick;
                    flowLayoutPanel1.Controls.Add(tb);
                }

            var Attrs = mTagsDB.FindFileAttrsWhere(attrswhere);
            if (Attrs.Count > 10) Attrs = Attrs.GetRange(0, 10);

            //ltAttributes.Controls.Clear();
            while (ltAttributes.Controls.Count > 1)
                ltAttributes.Controls.RemoveAt(0);

            if (Attrs != null)
                foreach (TFSFileAttribute FileAttr in Attrs) {
                    var Attribute = new ucFileAttribute(FileAttr);
                    ltAttributes.Controls.Add(Attribute);
                    ltAttributes.Controls.SetChildIndex(Attribute, ltAttributes.Controls.Count - 2);
                }

            //return;
            var Files = mTagsDB.FindFilesWhere(fileswhere); //Like("%" + txtFileSearch.Text + "%");

            if (Files.Count > 10) Files = Files.GetRange(0, 10);
            Text = "FillFilesAttributes";
            mTagsDB.FillFilesAttributes(Files);
            Text = "FillFilesTags";
            mTagsDB.FillFilesTags(Files);

            ltFiles.Controls.Clear();

            Text = "Filling list";

            int i = 10;
            foreach (TFSFile File in Files) {
                ucFile ucF = new ucFile(File);
                ucF.Enter += ucF_Enter;
                //ucF.Dock = DockStyle.Fill;
                ucF.Width = ltFiles.Width - 30;
                ltFiles.Controls.Add(ucF);

                i--;
                if (i < 0) return;
            }

            Text = "Ready";
        }

        void ucF_Enter(object sender, EventArgs e) {
            mTagsDB.LoadAllAttrs();

            TFSFile File = ((ucFile)sender).File;

            ucAddFileAttr1.DB = File.DB;

            while (ltAttributes.Controls.Count > 1)
                ltAttributes.Controls.RemoveAt(0);

            if (File.Attrs != null)
                foreach (TFSFileAttribute FileAttr in File.Attrs) {
                    var Attribute = new ucFileAttribute(FileAttr);
                    ltAttributes.Controls.Add(Attribute);
                    ltAttributes.Controls.SetChildIndex(Attribute, ltAttributes.Controls.Count - 2);
                }

            mFile = File;
        }

        private TFSFile mFile = null;
        private TagsDB mTagsDB = new TagsDB();

        private void ucAddFileAttr1_AddFileAttr(object sender, ucAddFileAttr.AddFileAttrEventArgs e) {
            e.FileAttr.File = mFile;
            mFile.Attrs.Add(e.FileAttr);
            e.FileAttr.FixInBase();

            TFSFile File = mFile;

            ucAddFileAttr1.DB = File.DB;

            while (ltAttributes.Controls.Count > 1)
                ltAttributes.Controls.RemoveAt(0);

            if (File.Attrs != null)
                foreach (TFSFileAttribute FileAttr in File.Attrs) {
                    var Attribute = new ucFileAttribute(FileAttr);
                    ltAttributes.Controls.Add(Attribute);
                    ltAttributes.Controls.SetChildIndex(Attribute, ltAttributes.Controls.Count - 2);
                }
        }
    }
}
