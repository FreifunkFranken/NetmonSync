using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NetmonSync
{
    class ClassNetmonRouterlist : ClassNetmonList
    {
        private string _apiUrl;
        public ClassNetmonRouterlist()
            : base("api/rest/routerlist/")
        {
            _apiUrl = "api/rest/routerlist/";
        }
        public ClassNetmonRouterlist(string UserID)
            : base("api/rest/user/" + UserID + "/routerlist/")
        {
            _apiUrl = "api/rest/user/" + UserID + "/routerlist/";
        }

        public List<ClassRouter> Routers
        {
            get
            {
                List<ClassRouter> routerlist = new List<ClassRouter>();
                foreach (var router in items)
                {
                    routerlist.Add(new ClassRouter(router));
                }
                return routerlist;
            }
        }
    }
}
