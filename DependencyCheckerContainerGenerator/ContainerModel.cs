using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyCheckerContainerGenerator
{
    class ContainerModel
    {
        public string ContainerID { get; set; }

        public string Image { get; set; }

        public string Command { get; set; }

        public string Created { get; set; }

        public string Status { get; set; }

        public string Ports { get; set; }

        public string Names { get; set; }

        public string PortSSL { get; set; }

        public string Port { get; set; }


    }
}
