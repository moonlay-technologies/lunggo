using Lunggo.Configuration.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Lunggo.Configuration
{
    class Program
    {
        
        static void Main(string[] args)
        {
            new ConfigGenerator().startConfig();
        }
        
    }
}
