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

        int DeleteTmpFileByFileNameAndIdCompany(string FileName, string IdCompany);

        int DeleteTmpFileByIdCompany(string IdCompany);

        void SaveBatchFiles(BatchModel batchModel);

        ObservableCollection<UploadingFile> GetTmpFileNameByIdCompany(string IdCompany, int IdCampagne);

        ObservableCollection<String> GetFileByCompanyAndCampagneId(string IdCampagne, string IdCompany);

        ObservableCollection<UploadingFile> GetTmpFileByIdCompany(string IdCopmpany);
    }
}