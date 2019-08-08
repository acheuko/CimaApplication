using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cima.Models.TestModel
{
    public class GridTotal
    {
        private string callCategory;
        public string CallCategory
        {
            get { return callCategory; }
            set { callCategory = value; }
        }

        private string site;
        public string Site
        {
            get { return site; }
            set { site = value; }
        }

        private double monthlyTotal;
        public double MonthlyTotal
        {
            get { return monthlyTotal; }
            set { monthlyTotal = value; }
        }

        private double avgCallDurationIn;
        public double AvgCallDurationIn
        {
            get { return avgCallDurationIn; }
            set { avgCallDurationIn = value; }
        }

        private double avgCallDurationOut;
        public double AvgCallDurationOut
        {
            get { return avgCallDurationOut; }
            set { avgCallDurationOut = value; }
        }

        private double avgNumberCallIn;
        public double AvgNumberCallIn
        {
            get { return avgNumberCallIn; }
            set { avgNumberCallIn = value; }
        }

        private double avgNumberCallOut;
        public double AvgNumberCallOut
        {
            get { return avgNumberCallOut; }
            set { avgNumberCallOut = value; }
        }
    }
}