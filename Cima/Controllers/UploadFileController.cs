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
using System.Configuration;

namespace Cima.Controllers
{
    [Authorize]
    public class UploadFileController : Controller
    {
        protected const string JSON_RESULT_FAILURE = "FAILURE";
        protected const string JSON_RESULT_SUCCESS = "SUCCESS";
        protected const string PFN = "PFN";
        protected const string VER = "VER";

        private static readonly IREPO_UploadFile repoUploadFile = new REPO_UploadFile();

        private string SessionCompanyId()
        {
            string IdCompany;
            if (Session["Company"] != null)
                IdCompany = Session["Company"].ToString();
            else
                IdCompany = "";

            return IdCompany; 
        }

        // GET: /UploadedFile/
        [Authorize(Roles = "Entreprise")]
        public ActionResult Index()
        {
            
            ObservableCollection<UploadingFile> uploadingFiles = repoUploadFile.GetTmpFileNameByIdCompany(SessionCompanyId());

            return View("UploadFile", uploadingFiles);
        }

        /**
         *  Copie des fichiers d'états dans le repertoire de chargement
         **/
        [Authorize(Roles = "Entreprise")]
        public JsonResult SaveFileToLanding()
        {
            String Status;

            // Get les fichiers dans le temporaire pour un IdCompany
            ObservableCollection<UploadingFile> Files = repoUploadFile.GetTmpFileByIdCompany(SessionCompanyId());

            if(Files != null)
            {
                string path = ConfigurationManager.AppSettings["LandingPath"];

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

                            string verFullPath = path + "\\" + filename.Replace(PFN, VER);

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
                        IdCompany = SessionCompanyId(),
                        NbFiles = Files.Count
                    };
                    repoUploadFile.SaveBatchFiles(batchFiles);
                    Status = JSON_RESULT_SUCCESS;
                }
                catch (Exception e)
                {
                    string searchPattern = batchNumber + "*."+PFN;
                    DeleteFileInLanding(path, searchPattern);
                    Console.WriteLine("Error Copy files Generated. Details: " + e.ToString());
                    Status = JSON_RESULT_FAILURE;

                }

            }else
                Status = JSON_RESULT_FAILURE;

            return Json(Status, JsonRequestBehavior.AllowGet);
        }

        /**
         * Sauvegarde des fichiers d'état dans la table temporaire
         **/ 
        [Authorize(Roles = "Entreprise")]
        public JsonResult UploadingFile(string fileName, int filesize, string file)
        {

            String Status;

            string FileExtension = fileName.Split('.')[1];
            // Vérifier l'extension du fichier chargé
            if (PFN.Equals(FileExtension))
            {
                string[] fArray = fileName.Split('_');
                string fileMask = fArray.Length >= 2?fArray[2].Split('.')[0]:"";
                UploadingFile uploadingFile = new UploadingFile
                {
                    FileName = fileName,
                    FileMask = fileMask,
                    FileSize = filesize,
                    IdCompany = SessionCompanyId(),
                    UserId = Session["Username"].ToString(),
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

        /**
         * Suppression des fichiers d'état de la table temporaire
         **/ 
        [Authorize(Roles = "Entreprise")]
        public JsonResult DeleteUploadedFile(string filename)
        {
            String StatusReponse;

            int response = repoUploadFile.DeleteTmpFileByFileNameAndIdCompany(filename, SessionCompanyId());

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

        // Suppression des fichiers dans le répertoire de chargement
        private static void DeleteFileInLanding(string path, string searchPattern)
        {
            try
            {
                string[] pfnList = Directory.GetFiles(path, searchPattern);

                string[] verList = Directory.GetFiles(path, searchPattern.Replace(PFN,VER));

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
