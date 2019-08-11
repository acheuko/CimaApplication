using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cima.Models
{
    public class UploadingFile
    {
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private int fileSize;
        public int FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        private string idCompany;
        public string IdCompany
        {
            get { return idCompany; }
            set { idCompany = value; }
        }

        private string userId;
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private byte[] file;
        public byte[] File
        {
            get { return file; }
            set
            {
                file = value;
            }
        }
        
    }
}