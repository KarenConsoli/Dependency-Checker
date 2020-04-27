using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BDConnection.AppSettings
{
    public class LogLevel
    {

        public string Default { get; set; }
        public string Microsoft { get; set; }

        [JsonProperty(PropertyName = "Microsoft.Hosting.Lifetime")]
        public string MicrosoftHostingLifetime { get; set; }

     

    }
}
