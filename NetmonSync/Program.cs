using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections;

namespace NetmonSync
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            while(i < args.Length)
            {
                switch (args[i])
                {
                    case "--netmon":
                        i++;
                        Properties.Settings.Default.Netmon = args[i];
                        break;
                    case "--libremap":
                        i++;
                        Properties.Settings.Default.CouchDB = args[i];
                        break;
                    case "--delay":
                        i++;
                        Properties.Settings.Default.Delay = int.Parse(args[i]);
                        break;
                    case "-d":
                        Properties.Settings.Default.DeamonMode = true;
                        break;
                    case "--log":
                        i++;
                        Properties.Settings.Default.LogPath = args[i];
                        break;
                }
                i++;
            }

            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            if (!System.IO.Path.IsPathRooted(Properties.Settings.Default.LogPath))
            {
                Properties.Settings.Default.LogPath = System.IO.Path.Combine(Environment.CurrentDirectory, Properties.Settings.Default.LogPath);
            }

            FileStream filestream = new FileStream(Properties.Settings.Default.LogPath, FileMode.Append);
            var streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);

            if (Properties.Settings.Default.DeamonMode)
            {
                while (true)
                {
                    Console.WriteLine(DateTime.UtcNow.ToString() + " Start");
                    WriteNetmonToCouch();
                    Console.WriteLine(DateTime.UtcNow.ToString() + " Ende");
                    Console.WriteLine();
                    Console.WriteLine("Warte " + Properties.Settings.Default.Delay + " Minuten für nächsten Durchlauf...");
                    System.Threading.Thread.Sleep(Properties.Settings.Default.Delay * 60000);
                }
            }
            else
            {
                Console.WriteLine(DateTime.UtcNow.ToString() + " Start");
                WriteNetmonToCouch();
                Console.WriteLine(DateTime.UtcNow.ToString() + " Ende");
            }
            //ClearCouch();
            streamwriter.Close();
        }

        static void WriteNetmonToCouch()
        {
            ClassNetmonRouterlist rl = new ClassNetmonRouterlist();

            ClassCouchDB db = new ClassCouchDB();
            foreach (ClassRouter router in rl.Routers)
            {
                if (router.online)
                {
                    db.addDoc(router.Json);
                }
            }
        }

        static void ClearCouch()
        {
            ClassCouchDB db = new ClassCouchDB();
            db.deleteAllDocs();
        }
    }
}
