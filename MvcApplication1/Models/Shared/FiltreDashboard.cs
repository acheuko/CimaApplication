using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models.Shared
{
    public class FiltreDashboard
    {
        private Dictionary<string, FiltreElement> dicoFiltres;
        public Dictionary<string, FiltreElement> DicoFiltres
        {
            get { return dicoFiltres; }
            set { dicoFiltres = value; }
        }

        public FiltreDashboard()
        {
            dicoFiltres = new Dictionary<string, FiltreElement>();
        }

        public FiltreDashboard(Dictionary<string, FiltreElement> dicoFiltres)
        {
            this.dicoFiltres = dicoFiltres;
        }

        public Dictionary<string, FiltreElement> getAllFiltres()
        {
            return this.dicoFiltres;
        }

        public FiltreElement getFiltre(string NomFiltre)
        {

            return this.dicoFiltres[NomFiltre];
        }

        public FiltreDashboard setFiltre(string NomFiltre, FiltreElement FiltreAFixer)
        {
            dicoFiltres.Remove(NomFiltre);
            dicoFiltres.Add(NomFiltre, FiltreAFixer);
            return this;
        }
    }
}