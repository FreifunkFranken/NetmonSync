using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetmonSync
{
    /// <summary>
    /// Basis-Klasse die als Grundlage für jede Liste für die Netmon API vererbt wird
    /// </summary>
    public abstract class ClassNetmonList
    {
        protected List<XNode> items = new List<XNode>();
        protected int count;

        public ClassNetmonList(string url)
        {
            XDocument doc = XDocument.Parse(ClassHttp.HttpGet(Properties.Settings.Default.Netmon + url));
            XNode routerlist = doc.Root.LastNode;

            count = int.Parse((routerlist as XElement).Attribute("total_count").Value);
            foreach (var router in (routerlist as XElement).Nodes())
            {
                items.Add(router);
            }

            if (count > 50)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("offset", "50");
                parameters.Add("limit", (count - 50).ToString());

                doc = XDocument.Parse(ClassHttp.HttpGet(Properties.Settings.Default.Netmon + url, parameters));
                routerlist = doc.Root.LastNode;

                foreach (var router in (routerlist as XElement).Nodes())
                {
                    items.Add(router);
                }
            }
        }
    }
}
