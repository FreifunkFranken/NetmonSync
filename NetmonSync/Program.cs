using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NetmonSync
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            //ClassNetmonRouterlist rl = new ClassNetmonRouterlist();
            //ClassNetmonOriginatorStatusList nil = new ClassNetmonOriginatorStatusList(32);

            //var tmp = rl.Routers[100].aliases.Adresses.Count;
            int wartezeit = 600000;  //1000 = 1sek
            while (true)
            {
                Console.WriteLine("Start");
                WriteNetmonToCouch();
                Console.WriteLine("Ende");
                Console.WriteLine();
                Console.WriteLine("Warte " + wartezeit / 60000 + " Minuten für nächsten Durchlauf...");
                System.Threading.Thread.Sleep(wartezeit);
            }
            //ClearCouch();
        }

        static void WriteNetmonToCouch()
        {
            ClassNetmonRouterlist rl = new ClassNetmonRouterlist();

            ClassCouchDB db = new ClassCouchDB();
            foreach (ClassRouter router in rl.Routers)
            {
                db.addDoc(router.Json);
            }
        }

        static void ClearCouch()
        {
            ClassCouchDB db = new ClassCouchDB();
            db.deleteAllDocs();
        }
    }
}
