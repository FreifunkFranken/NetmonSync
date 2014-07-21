using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetmonSync
{
    class ClassNetmonNetworklist : ClassNetmonList
    {
        public ClassNetmonNetworklist() : base("api/rest/networklist/")
        { }
    }
}
