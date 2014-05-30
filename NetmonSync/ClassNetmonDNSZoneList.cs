using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetmonSync
{
    class ClassNetmonDNSZoneList : ClassNetmonList
    {
        private string _apiUrl;
        public ClassNetmonDNSZoneList()
            : base("api/rest/dns_zone_list/")
        {
            _apiUrl = "api/rest/dns_zone_list/";
        }
        public ClassNetmonDNSZoneList(string UserID)
            : base("api/rest/user/" + UserID + "/dns_zone_list/")
        {
            _apiUrl = "api/rest/user/" + UserID + "/dns_zone_list/";
        }
    }
}
