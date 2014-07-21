using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetmonSync
{
    /// <summary>
    /// Klasse zur Auflistung aller Networkinterfaces für einen Router
    /// </summary>
    class ClassNetmonNetworkinterfacelist : ClassNetmonList
    {
        private Dictionary<string, string> interfaces = null;
        //https://netmon.freifunk-franken.de/api/rest/router/32/networkinterfacelist/
        public ClassNetmonNetworkinterfacelist(int RouterID)
            : base("api/rest/router/" + RouterID.ToString() + "/networkinterfacelist/")
        { }

        public Dictionary<string, string> Adresses
        {
            get {
                if (this.interfaces == null)
                {
                    interfaces = new Dictionary<string,string>();
                    foreach (XNode intface in items)
                    {
                        string name = (intface as XElement).Element("name").Value;
                        string mac = "";
                        mac = (intface as XElement).Element("mac_addr").Value;
                        if (mac == "")
                        {
                            XNode statusdata = (intface as XElement).Element("statusdata");
                            mac = (statusdata as XElement).Element("mac_addr").Value;
                        }
                        if (mac != "") interfaces.Add(name, mac);
                    }
                }
                return interfaces;
            }
        }
    }
}
