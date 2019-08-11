using Cima.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cima.Repository
{
    public interface IREPO_UploadFile
    {
        int SaveTmpFile(UploadingFile uploadingFile);

        void GetTmpFileByUserId(string UserId);

        int DeleteTmpFileByFileNameAndUserId(string FileName, string UserId);

        
    }
}