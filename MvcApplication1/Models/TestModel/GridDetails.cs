using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models.TestModel
{
    public class GridDetails
    {
        private string callCategory;
        public string CallCategory
        {
            get { return callCategory; }
            set { callCategory = value; }
        }

        private string bSC_LN;
        public string BSC_LN
        {
            get { return bSC_LN; }
            set { bSC_LN = value; }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        private string country;
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        private string origin;
        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        private string duration;
        public string Duration
        {
            get { return duration; }
            set { duration = value; }
        }
    }
}