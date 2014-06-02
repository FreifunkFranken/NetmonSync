using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetmonSync
{
    class ClassRouter
    {
        private XNode _xml;
        public ClassRouter(XNode xml)
        {
            _xml = xml;
        }

        public string key
        {
            get
            {
                return hostname + ".freifunk-franken.de";
            }
        }
        public string routerID
        {
            get
            {
                return (_xml as XElement).Element("router_id").Value;
            }
        }

        public string hostname
        {
            get {
                return (_xml as XElement).Element("hostname").Value;
            }
        }

        public DateTime ctime
        {
            get {
                return UnixTimeStampToDateTime(int.Parse((_xml as XElement).Element("create_date").Value));
            }
        }

        public DateTime mtime
        {
            get
            {
                //return UnixTimeStampToDateTime(int.Parse((_xml as XElement).Element("update_date").Value));
                return UnixTimeStampToDateTime((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            }
        }
        public bool online
        {
            get
            {
                if ((_xml as XElement).Element("statusdata").Element("status").Value == "online") return true;
                else return false;
            }
        }

        private DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public string lat
        {
            get
            {
                string tmp = (_xml as XElement).Element("latitude").Value;
                if (tmp == "") return "0";
                else return tmp;
            }
        }

        public string lon
        {
            get
            {
                string tmp = (_xml as XElement).Element("longitude").Value;
                if (tmp == "") return "0";
                else return tmp;
            }
        }

        public string site
        {
            get
            {
                return (_xml as XElement).Element("location").Value;
            }
        }

        public ClassNetmonNetworkinterfacelist aliases
        {
            get
            {
                return new ClassNetmonNetworkinterfacelist(int.Parse(this.routerID));
            }
        }

        public ClassNetmonOriginatorStatusList links
        {
            get
            {
                return new ClassNetmonOriginatorStatusList(int.Parse(this.routerID));
            }
        }

        public string community = "Freifunk/Franken";
        public string baseURL = Properties.Settings.Default.Netmon;

        public string Json
        {
            get
            {
                string json;
                json = "{";
                json += "\"api_rev\":\"1.0\",";
                json += "\"type\":\"router\",";
                json += "\"hostname\":\"" + this.hostname + "\",";
                json += "\"ctime\":\"" + this.ctime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\",";
                json += "\"mtime\":\"" + this.mtime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\",";
                json += "\"lat\":" + this.lat + ",";
                json += "\"lon\":" + this.lon + ",";

                json += "\"aliases\": [";
                foreach (KeyValuePair<string, string> inface in this.aliases.Adresses)
                {
                    json += "{";
                    json += "\"type\": \"batman-adv\",";
                    json += "\"alias\": \"" + inface.Value + "\"";
                    json += "},";
                }
                if (this.aliases.Adresses.Count > 0) json = json.Substring(0, json.Length - 1);
                json += "],";
                json += "\"links\": [";
                foreach (ClassLink link in this.links.Links)
                {
                    json += "{";
                    json += "\"type\": \"batman-adv\",";
                    json += "\"alias_local\": \"" + link.lokal + "\",";
                    json += "\"alias_remote\": \"" + link.remote + "\",";
                    json += "\"quality\": " + link.quality.ToString("0.00");
                    json += "},";
                }
                if (this.links.Links.Count > 0) json = json.Substring(0, json.Length - 1);
                json += "],";

                json += "\"site\":\"" + this.site + "\",";
                json += "\"community\":\"" + this.community + "\",";

                json += "\"attributes\":{\"netmon\":{";
                json += "\"id\":\"" + this.routerID + "\",";
                json += "\"url\":\"" + this.baseURL + "router.php?router_id=" + this.routerID + "\"";
                json += "}}}";

                return json;
            }
        }
    }
}
