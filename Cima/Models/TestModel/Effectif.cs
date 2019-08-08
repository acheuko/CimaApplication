using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cima.Models.TestModel
{
    public class Effectif
    {
        private int nbreEmploye;
        public int NbreEmploye
        {
            get { return nbreEmploye; }
            set { nbreEmploye = value; }
        }

        private int nbreEmployePermanent;
        public int NbreEmployePermanent
        {
            get { return nbreEmployePermanent; }
            set { nbreEmployePermanent = value; }
        }

        private int nbreEmployeTemporaire;
        public int NbreEmployeTemporaire
        {
            get { return nbreEmployeTemporaire; }
            set { nbreEmployeTemporaire = value; }
        }

        private string mois;
        public string Mois
        {
            get { return mois; }
            set { mois = value; }
        }

        private string moisSubstring;
        public string MoisSubstring
        {
            get { return moisSubstring; }
            set { moisSubstring = value; }
        }

        private string entiteAdmin;
        public string EntiteAdmin
        {
            get { return entiteAdmin; }
            set { entiteAdmin = value; }
        }

        private string catSocioPro;
        public string CatSocioPro
        {
            get { return catSocioPro; }
            set { catSocioPro = value; }
        }
    }
}