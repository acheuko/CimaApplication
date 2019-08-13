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

            // Get les fichiers dans le temporaire pour un userId
            ObservableCollection<UploadingFile> Files = repoUploadFile.GetTmpFileByUserId("iduser");

            if(Files != null)
            {
                string path = @"C:\Files\Pfn\Landing";

                // génération du numéro de batch
                string batchNumber = RandomNumber(0,9999999);

                // copie des fichiers PFN et VER dans le repertoire Landing
                foreach (var f in Files)
                {
                    string filename = batchNumber + "_" + f.FileName;
                    string pfnFullPath = path + "\\" + filename;
                    try
                    {
                        using (var pfn = new FileStream(pfnFullPath, FileMode.Create, FileAccess.Write))
                        {

                            string verFullPath = path + "\\" + filename.Replace("PFN", "VER");

                            FileStream ver = new FileStream(verFullPath, FileMode.Create, FileAccess.Write);
                            ver.Close();

                            using (var sw = new StreamWriter(verFullPath))
                                sw.Write(f.FileSize);

                            pfn.Write(f.File, 0, f.File.Length);

                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Error Copy files Generated. Details: " + e.ToString());
                        throw e;
                    } 
                }

                // création du batch
                try
                {
                    BatchModel batchFiles = new BatchModel()
                    {
                        BatchNumber = batchNumber,
                        IdCompany = "idcompany",
                        NbFiles = Files.Count
                    };
                    repoUploadFile.SaveBatchFiles(batchFiles);
                    Status = JSON_RESULT_SUCCESS;
                }
                catch (Exception e)
                {
                    string searchPattern = batchNumber + "*.PFN";
                    DeleteFileInLanding(path, searchPattern);
                    Console.WriteLine("Error Copy files Generated. Details: " + e.ToString());
                    Status = JSON_RESULT_FAILURE;

                }

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
                string[] fArray = fileName.Split('_');
                string fileMask = fArray.Length >= 2?fArray[2].Split('.')[0]:"";
                UploadingFile uploadingFile = new UploadingFile
                {
                    FileName = fileName,
                    FileMask = fileMask,
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


        // Generate a random number between two numbers    
        private string RandomNumber(int min, int max)
        {
            Random random = new Random();
            string randomNumber = random.Next(min, max).ToString();

            return randomNumber.Length<7? randomNumber.PadLeft(7,'0'):randomNumber;
        }

        private static void DeleteFileInLanding(string path, string searchPattern)
        {
            try
            {
                string[] pfnList = Directory.GetFiles(path, searchPattern);

                string[] verList = Directory.GetFiles(path, searchPattern.Replace("PFN","VER"));

                // Delete pfn files that were copied.
                foreach (string f in pfnList)
                {
                    System.IO.File.Delete(f);
                }
                // Delete ver files that were copied
                foreach (string f in verList)
                {
                    System.IO.File.Delete(f);
                }
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }
        }

    }
}
