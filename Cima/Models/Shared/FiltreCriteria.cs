using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cima.Models.Shared
{
    public class FiltreCriteria
    {
        public int ValueField { get; set; }
        public string TextField { get; set; }

        public int ParentField { get; set; }
    }
}