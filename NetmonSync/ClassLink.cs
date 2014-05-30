using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetmonSync
{
    /// <summary>
    /// Klasse zur Darstellung eines Links
    /// </summary>
    class ClassLink
    {
        public string lokal;
        public string remote;
        public double quality;

        public ClassLink(string lokal, string remote, double quality)
        {
            this.lokal = lokal;
            this.remote = remote;
            this.quality = quality;
        }
    }
}
