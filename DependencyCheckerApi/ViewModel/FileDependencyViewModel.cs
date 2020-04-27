using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyCheckerApi.ViewModel
{
    public class FileDependencyViewModel
    {
        public string Message { get; set; }
        public string FileName { get; set; }

        public string DependencyName {get;set;}

    }
}
