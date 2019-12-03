using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cima.Models
{
    public class BatchModel
    {
        private string batchNumber;
        public string BatchNumber
        {
            get { return batchNumber; }
            set { batchNumber = value; }
        }

        private string idCompany;
        public string IdCompany
        {
            get { return idCompany; }
            set { idCompany = value; }
        }

        private int nbFiles;
        public int NbFiles
        {
            get { return nbFiles; }
            set { nbFiles = value; }
        }

        private char batchClosed;
        public char BatchClosed
        {
            get { return batchClosed; }
            set { batchClosed = value; }
        }

        private DateTime dateUploadBatch;
        public DateTime DateUploadBatch
        {
            get { return dateUploadBatch; }
            set { dateUploadBatch = value; }
        }

        private int idCampagne;
        public int IdCampagne
        {
            get { return idCampagne; }
            set { idCampagne = value; }
        }

    }
}