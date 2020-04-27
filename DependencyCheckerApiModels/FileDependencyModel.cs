using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyCheckerApiModels
{
    public class FileDependencyModel
    {
        public long FileDependencyId { get; set; }
        public long FileId { get; set; }
        public long DependencyId { get; set; }
    }
}
