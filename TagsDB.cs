using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Common;

namespace TagsFS {
    class TFSTag {
        public TFSTag(String _Name = "", long _ParentID = -1) {
            Name = _Name;
            ParentID = _ParentID;
        }
        public TFSTag(long _ID, String _Name, long _ParentID) {
            ID = _ID;
            Name = _Name;
            ParentID = _ParentID;
        }

        public long ID = -1;
        public String Name = "";
        public long ParentID = -1;

        //public List<String> Tags;
    }

    class TFSFile {
        public TFSFile(String _Path = "", List<TFSTag> _Tags = null) {
            Path = _Path;
            Tags = _Tags;
        }
        public TFSFile(long _ID, String _Path = "", List<TFSTag> _Tags = null) {
            ID = _ID;
            Path = _Path;
            Tags = _Tags;
        }

        public long ID = -1;
        public String Path = "";
        //public String Name = ""; ?

        public List<TFSTag> Tags;
    }


    class TagsDB {
        public TagsDB() {
            String Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Tags3.db3";

            if (!System.IO.File.Exists(Path)) {
                Connection = new SQLiteConnection("URI=file:" + Path);
                CreateDB("URI=file:" + Path);
            }
            else
                Connection = new SQLiteConnection("URI=file:" + Path);
        }


        public void CheckTags(ref List<TFSTag> Tags) {
            Connection.Open();

            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                cmd.CommandText = "SELECT * FROM tags WHERE";

                foreach (TFSTag Tag in Tags) {
                    cmd.CommandText += " (name = '" + Tag.Name + "' AND parentid = " + Tag.ParentID + ") OR";
                }
                cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 3) + ";";

                using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                    foreach (DbDataRecord record in reader) {
                        foreach (TFSTag Tag in Tags) {
                            if (Tag.Name == record.GetString(1))
                                Tag.ID = record.GetInt64(0);
                        }
                    }
                }

                int n = 0;

                cmd.CommandText = "begin;\n";
                foreach (TFSTag Tag in Tags) {
                    if (Tag.ID == -1) {
                        n++;
                        cmd.CommandText += "INSERT INTO tags(name, parentid) VALUES('" + Tag.Name + "', '" + Tag.ParentID + "');\n";
                    }
                }
                cmd.CommandText += "commit;";

                cmd.ExecuteNonQuery();

                long id = Connection.LastInsertRowId - n;
                foreach (TFSTag Tag in Tags) {
                    if (Tag.ID == -1) {
                        id++;
                        Tag.ID = id;
                    }
                }
            }

            Connection.Close();
        }

        public void CheckFiles(ref List<TFSFile> Files) {
            //Connection.Open();

            //using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
            //    cmd.CommandText = "SELECT * FROM files WHERE";

            //    foreach (TFSFile File in Files) {
            //        cmd.CommandText += " path = '" + File.Path + "' OR";
            //    }
            //    cmd.CommandText = cmd.CommandText.Substring(1, cmd.CommandText.Length - 3) + ";";

            //    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
            //        foreach (DbDataRecord record in reader) {
            //            foreach (TFSFile File in Files) {
            //                if (File.Path == record.GetString(1))
            //                    Files.Remove(File);
            //            }
            //        }
            //    }

            //    //
            //}

            Connection.Open();

            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                cmd.CommandText = "SELECT * FROM files WHERE";

                foreach (TFSFile File in Files) {
                    cmd.CommandText += " path = '" + File.Path + "' OR";
                }
                cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 3) + ";";

                using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                    foreach (DbDataRecord record in reader) {
                        foreach (TFSFile File in Files) {
                            if (File.Path == record.GetString(1))
                                File.ID = record.GetInt64(0);
                        }
                    }
                }

                int n = 0;

                cmd.CommandText = "begin;\n";
                foreach (TFSFile File in Files) {
                    if (File.ID == -1) {
                        n++;
                        cmd.CommandText += "INSERT INTO files(path, name) VALUES('" + File.Path + "', '" + System.IO.Path.GetFileName(File.Path) + "');\n";
                        //cmd.CommandText += "INSERT INTO tags(name, parentid) VALUES('" + Tag.Name + "', '" + Tag.ParentID + "');\n";
                    }
                }
                cmd.CommandText += "commit;";

                cmd.ExecuteNonQuery();

                long id = Connection.LastInsertRowId - n;
                foreach (TFSFile File in Files) {
                    if (File.ID == -1) {
                        id++;
                        File.ID = id;
                    }
                }
            }

            Connection.Close();
        }


        public void AddFile(String File) {
            long ID = -1;
            String DirectoryName = System.IO.Path.GetDirectoryName(File);

            Connection.Open();

            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                cmd.CommandText = "SELECT * FROM files WHERE path = '" + File + "';";

                using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                    foreach (DbDataRecord record in reader) {
                        ID = record.GetInt64(0);
                        break;
                    }
                }

                if (ID == -1) {
                    cmd.CommandText = "INSERT INTO files(path, name) VALUES('" + File + "', '" + System.IO.Path.GetFileName(File) + "')";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT * FROM files WHERE path = '" + File + "';";

                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                        foreach (DbDataRecord record in reader) {
                            ID = record.GetInt64(0);
                            break;
                        }
                    }
                }
            }
            
            Connection.Close();

            long PrevID = -1;
            foreach (String tag in DirectoryName.Split('\\')) {
                PrevID = AddTag(tag, PrevID).ID;
            }

            Connection.Open();

            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                cmd.CommandText = "INSERT INTO filestags(fileid, tagid, accuracy) VALUES(" + ID.ToString() + ", " + PrevID.ToString() + ", " + "100" + ")";
                cmd.ExecuteNonQuery();
            }

            Connection.Close();
        }
        public void AddFiles(List<String> Files) {
            
        }


        public long FindTagsLike(String Name) {
            long ID = -1;

            Connection.Open();
            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {

                cmd.CommandText = "SELECT * FROM tags WHERE name IS LIKE '" + Name + ";";

                using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                    foreach (DbDataRecord record in reader) {
                        ID = record.GetInt64(0);
                        break;
                    }
                }
            }
            Connection.Close();

            return ID;
        }
        public long FindTag(String Name, long PTagID) {
            long ID = -1;

            Connection.Open();
            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {

                cmd.CommandText = "SELECT * FROM tags WHERE name = '" + Name + "' AND parentid = " + PTagID.ToString() + ";";

                using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                    foreach (DbDataRecord record in reader) {
                        ID = record.GetInt64(0);
                        break;
                    }
                }
            }
            Connection.Close();

            return ID;
        }
        public TFSTag AddTag(String _Name, long _PTagID = -1) {
            long ID = -1;
            Connection.Open();
            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {

                cmd.CommandText = "SELECT * FROM tags WHERE name = '" + _Name + "' AND parentid = " + _PTagID.ToString() + ";";

                using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                    foreach (DbDataRecord record in reader) {
                        ID = record.GetInt64(0);
                        break;
                    }
                }

                if (ID == -1) {
                    cmd.CommandText = "INSERT INTO tags(name, parentid) VALUES('" + _Name + "', " + _PTagID + ")";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT * FROM tags WHERE name = '" + _Name + "' AND parentid = " + _PTagID.ToString() + ";";

                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                        foreach (DbDataRecord record in reader) {
                            ID = record.GetInt64(0);
                            break;
                        }
                    }
                }
            }

            Connection.Close();

            return new TFSTag(ID, _Name, _PTagID);
        }


        public List<TFSTag> GetTagHierarchy(TFSTag Tag) {
            List<TFSTag> Hierarchy = new List<TFSTag>();

            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                Hierarchy.Add(Tag);


                if (Tag.ParentID != -1) {

                    cmd.CommandText = "SELECT * FROM tags WHERE id = '" + Tag.ParentID.ToString() + "';";

                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                        if (reader.HasRows) {
                            Tag.ID = reader.GetInt64(0);
                            Tag.Name = reader.GetString(1);
                            Tag.ParentID = reader.GetInt64(2);
                        }
                    }
                }
            }

            return Hierarchy;
        }


        private void CreateDB(String Path) {
            String StrCommand = "";
            StrCommand += "CREATE TABLE files (id INTEGER PRIMARY KEY, path TEXT, name TEXT);\n";
            StrCommand += "CREATE TABLE tags (id INTEGER PRIMARY KEY, name TEXT, parentid INTEGER);\n";
            StrCommand += "CREATE TABLE attributes (id INTEGER PRIMARY KEY, name TEXT, [values] TEXT);\n";

            StrCommand += "CREATE TABLE filestags (fileid INTEGER, tagid INTEGER, accuracy INTEGER);\n";
            StrCommand += "CREATE TABLE filesattributes (fileid INTEGER, attributeid INTEGER, value TEXT, accuracy INTEGER);\n";
            StrCommand += "CREATE TABLE tagssynonyms (tagida INTEGER, tagidb INTEGER, accuracy INTEGER);\n";
            StrCommand += "CREATE TABLE tagsattributes (tagid INTEGER, attributeid INTEGER, value TEXT, accuracy INTEGER);\n";


            SQLiteCommand Command = new SQLiteCommand(StrCommand, Connection);
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }


        private SQLiteConnection Connection;
    }


}
