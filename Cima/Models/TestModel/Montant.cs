using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cima.Models.TestModel
{
    public class Montant
    {
        private double montantSalaire;
        public double MontantSalaire
        {
            get { return montantSalaire; }
            set { montantSalaire = value; }
        }

        private double montantSalaireN_1;
        public double MontantSalaireN_1
        {
            get { return montantSalaireN_1; }
            set { montantSalaireN_1 = value; }
        }

        private double montantSalaireN_2;
        public double MontantSalaireN_2
        {
            get { return montantSalaireN_2; }
            set { montantSalaireN_2 = value; }
        }

        private double montantSalaireN_3;
        public double MontantSalaireN_3
        {
            get { return montantSalaireN_3; }
            set { montantSalaireN_3 = value; }
        }

        private double montantSalaireN_4;
        public double MontantSalaireN_4
        {
            get { return montantSalaireN_4; }
            set { montantSalaireN_4 = value; }
        }

        private double montantDeduction;
        public double MontantDeduction
        {
            get { return montantDeduction; }
            set { montantDeduction = value; }
        }

        private double prime;
        public double Prime
        {
            get { return prime; }
            set { prime = value; }
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

    }
}