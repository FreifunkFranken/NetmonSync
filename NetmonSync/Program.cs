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
            int wartezeit; //= Properties.Settings.Default.Delay;
            if (args.Length != 3 || !int.TryParse(args[2], out wartezeit))
            {
                Console.WriteLine("Falsche Parameter!");
                Console.WriteLine("Example: https://netmon.freifunk-franken.de/ http://95.85.40.145:5984/libremap-dev/ 600000");
                return;
            }

            Properties.Settings.Default.Netmon = args[0];
            Properties.Settings.Default.CouchDB = args[1];
            Properties.Settings.Default.Delay = wartezeit;

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
