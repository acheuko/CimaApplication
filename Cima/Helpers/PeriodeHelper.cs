using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cima.Helpers
{
    public static class PeriodeHelper
    {
        public static string GetLibelleLong(string libcourt, string periode)
        {
            string libellelong = "";

            if (periode == "Mensuel")
            {
                GetLibelleLongMois(libcourt);
            }
            else if(periode == "Trimestriel")
            {
                GetLibelleLongTrimestre(libcourt);
            }
            else if(periode == "Semestriel")
            {
                GetLibelleLongSemestre(libcourt);
            }


            return libellelong;
        }

        private static string GetLibelleLongMois(string libcourt)
        {
            string libellelong = "";
            switch (libcourt)
            {
                case "Jan":
                    libellelong = "Janvier";
                    break;
                case "Fev":
                    libellelong = "Février";
                    break;
                case "Mar":
                    libellelong = "Mars";
                    break;
                case "Avr":
                    libellelong = "Avril";
                    break;
                case "Mai":
                    libellelong = "Mai";
                    break;
                case "Jun":
                    libellelong = "Juin";
                    break;
                case "Jui":
                    libellelong = "Juillet";
                    break;
                case "Aou":
                    libellelong = "Août";
                    break;
                case "Sep":
                    libellelong = "Septembre";
                    break;
                case "Oct":
                    libellelong = "Octobre";
                    break;
                case "Nov":
                    libellelong = "Novembre";
                    break;
                case "Dec":
                    libellelong = "Décembre";
                    break;
                default:
                    break;
            }
            
            return libellelong;
        }

        private static string GetLibelleLongTrimestre(string libcourt)
        {
            string libellelong = "";

            switch (libcourt)
            {
                case "Q1":
                    libellelong = "Trimestre 1";
                    break;
                case "Q2":
                    libellelong = "Trimestre 2";
                    break;
                case "Q3":
                    libellelong = "Trimestre 3";
                    break;
                case "Q4":
                    libellelong = "Trimestre 4";
                    break;
                default:
                    break;
            }

            return libellelong;
        }

        private static string GetLibelleLongSemestre(string libcourt)
        {
            string libellelong = "";

            switch (libcourt)
            {
                case "S1":
                    libellelong = "Semestre 1";
                    break;
                case "S2":
                    libellelong = "Semestre 2";
                    break;
                default:
                    break;
            }

            return libellelong;
        }
    }
}