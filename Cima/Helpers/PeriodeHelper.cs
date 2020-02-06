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
                libellelong = GetLibelleLongMois(libcourt);
            }
            else if(periode == "Trimestriel")
            {
                libellelong = GetLibelleLongTrimestre(libcourt);
            }
            else if(periode == "Semestriel")
            {
                libellelong = GetLibelleLongSemestre(libcourt);
            }


            return libellelong;
        }

        public static string GetLibelleCourt(string liblong, string periode)
        {
            string libellelong = "";

            if (periode == "Mensuel")
            {
                libellelong = GetLibelleCourtMois(liblong);
            }
            else if (periode == "Trimestriel")
            {
                libellelong = GetLibelleCourtTrimestre(liblong);
            }
            else if (periode == "Semestriel")
            {
                libellelong = GetLibelleCourtSemestre(liblong);
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

        private static string GetLibelleCourtMois(string libLong)
        {
            string libelleCourt = "";
            switch (libLong)
            {
                case "Janvier":
                    libelleCourt = "Jan";
                    break;
                case "Février":
                    libelleCourt = "Fev";
                    break;
                case "Mars":
                    libelleCourt = "Mar";
                    break;
                case "Avril":
                    libelleCourt = "Avr";
                    break;
                case "Mai":
                    libelleCourt = "Mai";
                    break;
                case "Juin":
                    libelleCourt = "Jun";
                    break;
                case "Juillet":
                    libelleCourt = "Jui";
                    break;
                case "Août":
                    libelleCourt = "Aou";
                    break;
                case "Septembre":
                    libelleCourt = "Sep";
                    break;
                case "Octobre":
                    libelleCourt = "Oct";
                    break;
                case "Novembre":
                    libelleCourt = "Nov";
                    break;
                case "Décembre":
                    libelleCourt = "Dec";
                    break;
                default:
                    break;
            }

            return libelleCourt;
        }

        private static string GetLibelleCourtTrimestre(string libLong)
        {
            string libellecourt = "";

            switch (libLong)
            {
                case "Trimestre 1":
                    libellecourt = "Q1";
                    break;
                case "Trimestre 2":
                    libellecourt = "Q2";
                    break;
                case "Trimestre 3":
                    libellecourt = "Q3";
                    break;
                case "Trimestre 4":
                    libellecourt = "Q4";
                    break;
                default:
                    break;
            }

            return libellecourt;
        }

        private static string GetLibelleCourtSemestre(string liblong)
        {
            string libellecourt = "";

            switch (liblong)
            {
                case "Semestre 1":
                    libellecourt = "S1";
                    break;
                case "Semestre 2":
                    libellecourt = "S2";
                    break;
                default:
                    break;
            }

            return libellecourt;
        }



    }
}