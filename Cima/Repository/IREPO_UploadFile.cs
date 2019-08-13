using Cima.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Cima.Repository
{
    public interface IREPO_UploadFile
    {
        int SaveTmpFile(UploadingFile uploadingFile);

        int DeleteTmpFileByFileNameAndUserId(string FileName, string UserId);

        int DeleteTmpFileByUserId(string UserId);

        void SaveBatchFiles(BatchModel batchModel);

        ObservableCollection<UploadingFile> GetTmpFileNameByUserId(string UserId);

        ObservableCollection<UploadingFile> GetTmpFileByUserId(string UserId);
    }
}