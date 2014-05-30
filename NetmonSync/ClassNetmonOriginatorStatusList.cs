using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetmonSync
{
    class ClassNetmonOriginatorStatusList : ClassNetmonList
    {
        private int routerID;
        public ClassNetmonOriginatorStatusList(int RouterID)
            : base("api/rest/router/" + RouterID.ToString() + "/originator_status_list/")
        { 
            this.routerID = RouterID; 
        }

        public List<ClassLink> Links
        {
            get {
                ClassNetmonNetworkinterfacelist infaces = new ClassNetmonNetworkinterfacelist(this.routerID);
                List<ClassLink> linklist = new List<ClassLink>();
                foreach (XNode originator in items)
                {
                    string alias_local = "";
                    string alias_remote = "";
                    double quality = 0;

                    alias_remote = (originator as XElement).Element("originator").Value;
                    quality = double.Parse((originator as XElement).Element("link_quality").Value) / 255;
                    string intface = (originator as XElement).Element("outgoing_interface").Value;

                    infaces.Adresses.TryGetValue(intface, out alias_local);

                    if (alias_local != "" && alias_remote != "" && quality <= 1 && quality >= 0)
                    {
                        linklist.Add(new ClassLink(alias_local, alias_remote, quality));
                    }
                }
                return linklist;
            }
        }
    }
}
