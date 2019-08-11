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

        private static IREPO_UploadFile repoUploadFile = new REPO_UploadFile();

        //
        // GET: /UploadFile/

        public ActionResult Index([DataSourceRequest]DataSourceRequest request)
        {
            return View("UploadFile");
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
                    Status = "FAILURE";
                else
                    Status = "SUCCESS";
            }
            else
                Status = "FAILURE";

            return Json(Status, JsonRequestBehavior.AllowGet); 
        }

        public JsonResult DeleteUploadedFile(string filename)
        {
            String StatusReponse = "SUCCESS";

            int response = repoUploadFile.DeleteTmpFileByFileNameAndUserId(filename, "iduser");

            if (response == 0)
                StatusReponse = "FAILURE";
            else
                StatusReponse = "SUCCESS";

            return Json(StatusReponse, JsonRequestBehavior.AllowGet);
        }

    }
}
