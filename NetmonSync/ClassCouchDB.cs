using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;

namespace NetmonSync
{
    class ClassCouchDB
    {
        private string _server = Properties.Settings.Default.CouchDB;
        private Dictionary<string, string> _docIDs;
        private Dictionary<string, string> _docs;

        private Dictionary<string, string> allDocIDs()
        {
            if (this._docIDs == null)
            {
                this._docIDs = new Dictionary<string, string>();
                this._docs = new Dictionary<string, string>();
                string result = ClassHttp.HttpGet(this._server + "_all_docs?include_docs=true");
                JsonData d = JsonMapper.ToObject(result);

                //Console.WriteLine(result);
                foreach (JsonData row in d["rows"])
                {
                    try
                    {
                        this._docIDs.Add(
                            row["id"].ToString(),
                            (row["doc"])["hostname"].ToString()
                        );
                        this._docs.Add(
                            row["id"].ToString(),
                            row["doc"].ToJson()
                        );
                    }
                    catch { Console.WriteLine("Doc nicht hinzugefügt: " + row["id"].ToString()); };
                }
            }
            return this._docIDs;
        }

        private string getDocIDfromHostname(string hostname)
        {
            if (allDocIDs().ContainsValue(hostname))
            {
                return allDocIDs().FirstOrDefault(x => x.Value == hostname).Key;
            }
            else
            {
                return null;
            }
        }
        private Boolean updateDoc(string docID, string docContent)
        {
            JsonData OldDoc = JsonMapper.ToObject(this._docs[docID]);
            JsonData NewDoc = JsonMapper.ToObject(docContent);

            if (OldDoc["mtime"].ToString() != NewDoc["mtime"].ToString())
            {
                Console.WriteLine(DateTime.Now + ": " + NewDoc["hostname"].ToString() + " with id (CouchDB) " + docID + " updated.");

                NewDoc["_rev"] = OldDoc["_rev"].ToString();
                NewDoc["ctime"] = OldDoc["ctime"].ToString();
                //NewDoc["lat"] = OldDoc["lat"].ToString();
                //NewDoc["lon"] = OldDoc["lon"].ToString();

                var tmp = NewDoc["lat"];

                ClassHttp.HttpPut(this._server + docID, NewDoc.ToJson());
            }
            return true;
        }

        public Boolean addDoc(string docContent)
        {
            JsonData docContentJson = JsonMapper.ToObject(docContent);
            string hostname = docContentJson["hostname"].ToString();

            if (getDocIDfromHostname(hostname) == null)
            {
                try
                {
                    string response = ClassHttp.HttpPost(this._server, docContent);
                    JsonData responseJson = JsonMapper.ToObject(response);

                    string couchID = responseJson["id"].ToString();
                    this._docIDs.Add(
                                    couchID,
                                    hostname
                                );
                    Console.WriteLine(DateTime.Now + ": " + hostname + " with id (CouchDB) " + couchID + " added.");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                return updateDoc(getDocIDfromHostname(hostname), docContent);
            }
        }

        public void deleteAllDocs()
        {
            foreach (string docid in this.allDocIDs().Keys)
            {
                if (ClassHttp.HttpDelete(this._server + docid))
                {
                    Console.WriteLine("Doc " + docid + " gelöscht.");
                }
                else
                {
                    Console.WriteLine("Doc " + docid + " konnte nicht gelöscht werden.");
                }
            }
        }
    }
}
