using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;

namespace TagsFS {
    public class TFSTag {
        public TFSTag(String _Name = "", Int64 _ParentID = -1) {
            Name = _Name;
            ParentID = _ParentID;
        }
        public TFSTag(Int64 _ID, String _Name, Int64 _ParentID) {
            ID = _ID;
            Name = _Name;
            ParentID = _ParentID;
        }

        public Int64 ID = -1;
        public String Name = "";
        public Int64 ParentID = -1;
        public TFSTag ParentTag = null;
    }
    public enum StandartAttr {
        Name,
        Extention,
        Type,
        Rating,

        File_WasCreated,
        File_WasModified,
        
        Song_By,
        Song_Name,
        Song_Genre,
        Song_Year,
        Song_Lyrics,
        Song_Length,
        Song_Album,

        Image_Resolution,
        Image_Place,
        Image_Peoples,
        
        Thumbnail_Id,
        Thumbnail_Date,

        TVSeries_Season,
        TVSeries_Episode
    }
    public class TFSAttr {
        public TFSAttr(StandartAttr _Attr) {
            ID = (Int64)_Attr;
            Name = _Attr.ToString();
        }
        public TFSAttr(String _Name = "", long _Values = -1) {
            Name = _Name;
            Values += _Values;
        }
        public TFSAttr(long _ID, String _Name, long _Values) {
            ID = _ID;
            Name = _Name;
            Values += _Values;
        }

        public long ID = -1;
        public String Name = "";
        public String Values = "";
    }
    public class TFSFile {
        public TFSFile(String _Path = "", TFSTag _Tag = null) {
            ID = -1;
            Path = _Path;

            Tags = new List<TFSFileTag>();
            Tags.Add(new TFSFileTag(this, _Tag));
        }
        public TFSFile(String _Path = "", List<TFSTag> _Tags = null) {
            ID = -1;
            Path = _Path;

            Tags = new List<TFSFileTag>();
            foreach (TFSTag Tag in _Tags) {
                Tags.Add(new TFSFileTag(this, Tag));
            }
        }
        public TFSFile(String _Path = "", List<TFSFileTag> _Tags = null) {
            ID = -1;
            Path = _Path;
            Tags = _Tags;
        }
        public TFSFile(long _ID, String _Path = "", List<TFSFileTag> _Tags = null, List<TFSFileAttribute> _Attrs = null) {
            ID = _ID;
            Path = _Path;
            Tags = _Tags;
            Attrs = _Attrs;
        }

        public long ID;
        public String Path = "";

        public bool TagsSynchronized = false;
        public List<TFSFileTag> Tags = null;

        public bool AttrsSynchronized = false;
        public List<TFSFileAttribute> Attrs = null;
    }
    public class TFSFileTag {
        public TFSFileTag(TFSFile _File, TFSTag _Tag, Double _Accuracy = 1) {
            File = _File;
            Tag = _Tag;
            Accuracy = _Accuracy;
        }

        public Int64 ID = -1;
        public TFSFile File = null;
        public TFSTag Tag = null;
        public Double Accuracy = 1;
    }
    public class TFSFileAttribute {
        public TFSFileAttribute(TFSFile _File = null, TFSAttr _Attr = null, String _Value = "", Double _Accuracy = 1) {
            File = _File;
            Attr = _Attr;
            Value = _Value;
        }

        public Int64 ID = -1;
        public TFSFile File = null;
        public TFSAttr Attr = null;
        public String Value = "";
        public Double Accuracy = 1;
    }



    public class TagsDB {
        public TagsDB() {
            String Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Tags.db3";

            if (!System.IO.File.Exists(Path)) {
                Connection = new SQLiteConnection("URI=file:" + Path);
                CreateDB("URI=file:" + Path);
            }
            else
                Connection = new SQLiteConnection("URI=file:" + Path);

            OpenConnection();
        }

        public void OpenConnection() {
            if (Connection.State != ConnectionState.Open) {
                Connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                    cmd.CommandText = "PRAGMA synchronous = 0;";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "PRAGMA journal_mode = OFF;";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "PRAGMA cache_size = 8000;";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "PRAGMA recursive_triggers = ON;";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void CloseConnection() {
            if (Connection.State != ConnectionState.Closed) Connection.Close();
        }

        
        public void CheckTags(ref List<TFSTag> Tags) {
            ConnectionState prevConnectionState = Connection.State; OpenConnection();

            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                cmd.CommandText = "SELECT * FROM tags WHERE";

                foreach (TFSTag Tag in Tags) {
                    cmd.CommandText += " (name = '" + Tag.Name.Replace("'", "''") + "' AND parentid = " + Tag.ParentID + ") OR";
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
                cmd.CommandText = "";
                var tr = Connection.BeginTransaction();
                foreach (TFSTag Tag in Tags) {
                    if (Tag.ID == -1) {
                        n++;
                        cmd.CommandText += "INSERT INTO tags(name, parentid) VALUES('" + Tag.Name.Replace("'", "''") + "', '" + Tag.ParentID + "');\n";
                    }
                }
                cmd.ExecuteNonQuery();
                tr.Commit();

                long id = Connection.LastInsertRowId - n;
                foreach (TFSTag Tag in Tags) {
                    if (Tag.ID == -1) {
                        id++;
                        Tag.ID = id;
                    }
                }
            }

            if (prevConnectionState == ConnectionState.Closed) Connection.Close();
        }

        public void CheckFiles(ref List<TFSFile> Files) {
            ConnectionState prevConnectionState = Connection.State; OpenConnection();

            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                cmd.CommandText = "SELECT * FROM files WHERE";

                foreach (TFSFile File in Files) {
                    cmd.CommandText += " path = '" + File.Path.Replace("'", "''") + "' OR";
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
                cmd.CommandText = "";
                var tr = Connection.BeginTransaction();
                foreach (TFSFile File in Files) {
                    if (File.ID == -1) {
                        n++;
                        cmd.CommandText += "INSERT INTO files(path, name) VALUES('" + File.Path.Replace("'", "''") + "', '" + System.IO.Path.GetFileName(File.Path.Replace("'", "''")) + "');\n";
                    }
                }
                cmd.ExecuteNonQuery();
                tr.Commit();

                long id = Connection.LastInsertRowId - n;
                foreach (TFSFile File in Files) {
                    if (File.ID == -1) {
                        id++;
                        File.ID = id;
                    }
                }
            }

            if (prevConnectionState == ConnectionState.Closed) Connection.Close();
        }

        public void CheckFilesTags(List<TFSFileTag> FilesTags) {
            ConnectionState prevConnectionState = Connection.State; OpenConnection();

            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                cmd.CommandText = "SELECT * FROM filestags WHERE";

                foreach (TFSFileTag FileTag in FilesTags) {
                    if (FileTag.ID == -1)
                        cmd.CommandText += " (fileid = " + FileTag.File.ID.ToString() + " AND tagid = " + FileTag.Tag.ID.ToString() + " AND accuracy = " + FileTag.Accuracy.ToString() + ") OR";
                    else
                        cmd.CommandText += " (id = " + FileTag.ID.ToString() + ") OR";
                }
                cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 3) + ";";

                String newcmdtext = "";
                using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                    foreach (DbDataRecord record in reader) {
                        foreach (TFSFileTag FileTag in FilesTags) {
                            if (FileTag.ID == -1) {
                                if (FileTag.File.ID == record.GetInt64(1) && FileTag.Tag.ID == record.GetInt64(2) && FileTag.Accuracy == record.GetDouble(3))
                                    FileTag.ID = record.GetInt64(0);
                            }
                            else
                                if (FileTag.ID == record.GetInt64(0))
                                    if (FileTag.File.ID != record.GetInt64(1) || FileTag.Tag.ID != record.GetInt64(2))
                                        newcmdtext += "UPDATE filestags SET fileid = " + FileTag.File.ID.ToString() + ", tagid = " + FileTag.Tag.ID.ToString() + ", accuracy = " + FileTag.Accuracy.ToString() + " WHERE id = " + FileTag.ID.ToString() + ";\n";
                        }
                    }
                }

                int n = 0;
                var tr = Connection.BeginTransaction();
                cmd.CommandText = newcmdtext;
                //cmd.ExecuteNonQuery();
                //cmd.CommandText = "";
                foreach (TFSFileTag FileTag in FilesTags) {
                    if (FileTag.ID == -1) {
                        n++;
                        cmd.CommandText += "INSERT INTO filestags(fileid, tagid, accuracy) VALUES(" + FileTag.File.ID.ToString() + ", " + FileTag.Tag.ID.ToString() + ", " + FileTag.Accuracy.ToString() + ");\n";
                        //if (n % 50 == 0) {
                        //    cmd.ExecuteNonQuery();
                        //    cmd.CommandText = "";
                        //}
                    }
                }
                cmd.ExecuteNonQuery();
                tr.Commit();

                long id = Connection.LastInsertRowId - n;
                foreach (TFSFileTag FileTag in FilesTags) {
                    if (FileTag.ID == -1) {
                        id++;
                        FileTag.ID = id;
                    }
                }
            }

            if (prevConnectionState == ConnectionState.Closed) Connection.Close();
        }
        //public void AddFile(String File) {
        //    long ID = -1;
        //    String DirectoryName = System.IO.Path.GetDirectoryName(File);

        //    Connection.Open();

        //    using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
        //        cmd.CommandText = "SELECT * FROM files WHERE path = '" + File + "';";

        //        using (SQLiteDataReader reader = cmd.ExecuteReader()) {
        //            foreach (DbDataRecord record in reader) {
        //                ID = record.GetInt64(0);
        //                break;
        //            }
        //        }

        //        if (ID == -1) {
        //            cmd.CommandText = "INSERT INTO files(path, name) VALUES('" + File + "', '" + System.IO.Path.GetFileName(File) + "')";
        //            cmd.ExecuteNonQuery();

        //            cmd.CommandText = "SELECT * FROM files WHERE path = '" + File + "';";

        //            using (SQLiteDataReader reader = cmd.ExecuteReader()) {
        //                foreach (DbDataRecord record in reader) {
        //                    ID = record.GetInt64(0);
        //                    break;
        //                }
        //            }
        //        }
        //    }
            
        //    Connection.Close();

        //    long PrevID = -1;
        //    foreach (String tag in DirectoryName.Split('\\')) {
        //        PrevID = AddTag(tag, PrevID).ID;
        //    }

        //    Connection.Open();

        //    using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
        //        cmd.CommandText = "INSERT INTO filestags(fileid, tagid, accuracy) VALUES(" + ID.ToString() + ", " + PrevID.ToString() + ", " + "100" + ")";
        //        cmd.ExecuteNonQuery();
        //    }

        //    Connection.Close();
        //}
        //public void AddFiles(List<String> Files) {
            
        //}


        public List<TFSTag> FindTagsLike(String Name) {
            ConnectionState prevConnectionState = Connection.State; OpenConnection();

            List<TFSTag> result = new List<TFSTag>();

            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                cmd.CommandText = "SELECT * FROM tags WHERE name LIKE '" + Name + "';";

                using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                    foreach (DbDataRecord record in reader) {
                        result.Add(new TFSTag(record.GetInt64(0), record.GetString(1), record.GetInt64(2)));
                    }
                }
            }

            if (prevConnectionState == ConnectionState.Closed) Connection.Close();

            return result;
        }

        public List<TFSFile> FindFilesLike(String Name) {
            ConnectionState prevConnectionState = Connection.State; OpenConnection();

            List<TFSFile> result = new List<TFSFile>();

            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                cmd.CommandText = "SELECT * FROM files WHERE path LIKE '" + Name + "';";

                using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                    foreach (DbDataRecord record in reader) {
                        result.Add(new TFSFile(record.GetInt64(0), record.GetString(1)));
                    }
                }
            }

            if (prevConnectionState == ConnectionState.Closed) Connection.Close();

            return result;
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
            ConnectionState prevConnectionState = Connection.State; OpenConnection();

            long ID = -1;
            
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

            if (prevConnectionState == ConnectionState.Closed) Connection.Close();

            return new TFSTag(ID, _Name, _PTagID);
        }


        public List<TFSTag> GetTagHierarchy(TFSTag Tag) {
            ConnectionState prevConnectionState = Connection.State; OpenConnection();
            
            List<TFSTag> Hierarchy = new List<TFSTag>();

            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                Hierarchy.Add(Tag);

                while (Tag.ParentID != -1) {
                    cmd.CommandText = "SELECT * FROM tags WHERE id = '" + Tag.ParentID.ToString() + "';";

                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                        if (reader.HasRows) {
                            reader.Read();
                            Tag.ParentTag = new TFSTag(reader.GetInt64(0), reader.GetString(1), reader.GetInt64(2));
                            Tag = Tag.ParentTag;
                            Hierarchy.Add(Tag);
                        }
                        else {
                            Tag.ParentID = -1;
                        }
                    }
                }
            }

            if (prevConnectionState == ConnectionState.Closed) Connection.Close();

            return Hierarchy;
        }


        private void CreateDB(String Path) {
            String StrCommand = "";
            StrCommand += "CREATE TABLE files       (id INTEGER PRIMARY KEY ASC, path TEXT, name TEXT);\n";
            StrCommand += "CREATE TABLE tags        (id INTEGER PRIMARY KEY ASC, name TEXT, parentid INTEGER);\n";
            StrCommand += "CREATE TABLE attributes  (id INTEGER PRIMARY KEY ASC, name TEXT, [values] TEXT);\n";

            StrCommand += "CREATE TABLE filestags         (id INTEGER PRIMARY KEY ASC, fileid INTEGER, tagid INTEGER, accuracy REAL);\n";
            StrCommand += "CREATE TABLE filesattributes   (id INTEGER PRIMARY KEY ASC, fileid INTEGER, attributeid INTEGER, value TEXT, accuracy REAL);\n";
            StrCommand += "CREATE TABLE tagssynonyms      (id INTEGER PRIMARY KEY ASC, tagida INTEGER, tagidb INTEGER, accuracy REAL);\n";
            StrCommand += "CREATE TABLE tagsattributes (id INTEGER PRIMARY KEY ASC, tagid INTEGER, attributeid INTEGER, value TEXT, accuracy INTEGER);\n";

            SQLiteCommand Command = new SQLiteCommand(StrCommand, Connection);
            Connection.Open();
            Command.ExecuteNonQuery();

            Command.CommandText = "";
            foreach (StandartAttr a in (StandartAttr[])Enum.GetValues(typeof(StandartAttr))) {
                TFSAttr Attr = new TFSAttr(a);
                Command.CommandText += "INSERT INTO attributes   (id, text)    VALUES(" + Attr.ID.ToString() + ", '" + Attr.Name + "');\n";
            }

            Connection.Close();
        }


        private SQLiteConnection Connection;

        internal List<TFSTag> FindAllTagChildren(TFSTag _Tag) {
            ConnectionState prevConnectionState = Connection.State; OpenConnection();

            List<TFSTag> Result = new List<TFSTag>();

            using (SQLiteCommand cmd = new SQLiteCommand(Connection)) {
                cmd.CommandText = "CREATE TEMPORARY TABLE alltagschildren (id INTEGER)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TRIGGER alltagschildren AFTER INSERT " +
                                  "ON alltagschildren " +
                                  "BEGIN " +
                                    "INSERT INTO alltagschildren(id) select id from tags where parentid = new.id; " +
                                  "END;";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO alltagschildren VALUES(" + _Tag.ID + ");";
                cmd.ExecuteNonQuery();

                //long liri = -1;
                //while (liri != Connection.LastInsertRowId) {
                //    liri = Connection.LastInsertRowId;
                //    cmd.CommandText = "INSERT INTO alltagschildren (id) SELECT id FROM tags WHERE parentid IN (SELECT id FROM alltagschildren) AND id NOT IN (SELECT id FROM alltagschildren);";
                //    cmd.ExecuteNonQuery();
                //}

                cmd.CommandText = "SELECT * FROM tags WHERE id IN (SELECT id FROM alltagschildren);";
                using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                    foreach (DbDataRecord record in reader) {
                        Result.Add(new TFSTag(record.GetInt64(0), record.GetString(1), record.GetInt64(2)));
                    }
                }

                cmd.CommandText = "DROP TABLE alltagschildren;";
                cmd.ExecuteNonQuery();
            }

            if (prevConnectionState == ConnectionState.Closed) Connection.Close();

            return Result;
        }
    }


}
