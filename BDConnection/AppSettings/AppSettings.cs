using System;
using System.Collections.Generic;
using System.Text;

namespace BDConnection.AppSettings
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public SQLServer SQLServer { get; set; }
    }
}
