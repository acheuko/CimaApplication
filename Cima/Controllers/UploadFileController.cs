using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.IO;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Cima.Models;
using Cima.Repository;
using System.Text;

namespace Cima.Controllers
{
    public class UploadFileController : Controller
    {
        protected const string JSON_RESULT_FAILURE = "FAILURE";
        protected const string JSON_RESULT_SUCCESS = "SUCCESS";

        private static readonly IREPO_UploadFile repoUploadFile = new REPO_UploadFile();

        //
        // GET: /UploadedFile/
        public ActionResult Index()
        {
            ObservableCollection<UploadingFile> uploadingFiles = repoUploadFile.GetTmpFileNameByUserId("idusers");

            return View("UploadFile", uploadingFiles);
        }


        public JsonResult SaveFileToLanding()
        {
            String Status;
            ObservableCollection<UploadingFile> Files = repoUploadFile.GetTmpFileByUserId("iduser");

            if(Files != null)
            {
                string path = @"C:\Files\Pfn\Landing";

                foreach (var f in Files)
                {
                    string pfnFullPath = path + "\\" + f.FileName;
                    using (var pfn = new FileStream(pfnFullPath, FileMode.Create, FileAccess.Write))
                    {
                        string verFullPath = path + "\\" + f.FileName.Replace("PFN","VER");

                        FileStream ver = new FileStream(verFullPath, FileMode.Create, FileAccess.Write);
                        ver.Close();
                        
                        using (var sw = new StreamWriter(verFullPath))
                            sw.Write(f.FileSize);
                        
                            
                        pfn.Write(f.File, 0, f.File.Length);
                    }
                       
                }
                Status = JSON_RESULT_SUCCESS;
            }else
                Status = JSON_RESULT_FAILURE;

            return Json(Status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadingFile(string fileName, int filesize, string file)
        {

            String Status;

            string FileExtension = fileName.Split('.')[1];
            // Vérifier l'extension du fichier chargé
            if (FileExtension.Equals("PFN"))
            {
                UploadingFile uploadingFile = new UploadingFile
                {
                    FileName = fileName,
                    FileSize = filesize,
                    IdCompany = "idcompany",
                    UserId = "iduser",
                    File = Encoding.ASCII.GetBytes(file)
                };

                int response = repoUploadFile.SaveTmpFile(uploadingFile);

                if (response == 0)
                    Status = JSON_RESULT_FAILURE;
                else
                    Status = JSON_RESULT_SUCCESS;
            }
            else
                Status = JSON_RESULT_FAILURE;

            return Json(Status, JsonRequestBehavior.AllowGet); 
        }

        public JsonResult DeleteUploadedFile(string filename)
        {
            String StatusReponse;

            int response = repoUploadFile.DeleteTmpFileByFileNameAndUserId(filename, "iduser");

            if (response == 0)
                StatusReponse = JSON_RESULT_FAILURE;
            else
                StatusReponse = JSON_RESULT_SUCCESS;

            return Json(StatusReponse, JsonRequestBehavior.AllowGet);
        }

    }
}
