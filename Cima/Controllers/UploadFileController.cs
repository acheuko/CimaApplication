using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.IO;
using Cima.Models;
using Cima.Repository;
using System.Text;
using System.Configuration;
using Cima.Helpers;
using Cima.Repository.Shared;

namespace Cima.Controllers
{
    [Authorize]
    public class UploadFileController : Controller
    {
        protected const string JSON_RESULT_FAILURE = "FAILURE";
        protected const string JSON_RESULT_SUCCESS = "SUCCESS";
        protected const string PFN = "PFN";
        protected const string VER = "VER";

        private readonly UnitOfWork unitOfWork = new UnitOfWork();
        private readonly REPO_FichierRejete repoFichierRejete;

        private static readonly IREPO_UploadFile repoUploadFile = new REPO_UploadFile();
        private static readonly REPO_Campaign repoCampaign = new REPO_Campaign();
        private static readonly REPO_CampaignCampaignControl repoCampaignControl = new REPO_CampaignCampaignControl();
        private static readonly REPO_ContratInterface repoContratInterface = new REPO_ContratInterface();

        private static ObservableCollection<FichierRejete> fichierRejetes = new ObservableCollection<FichierRejete>();
        private static List<string> listeFichiersRejetes = new List<string>();

        private static List<string> FichiersRestants { get; set; }

        public UploadFileController()
        {
            repoFichierRejete = (REPO_FichierRejete)unitOfWork.FichierRejeteRepository;
        }

        private string GetSessionCompanyId()
        {
            string IdCompany;
            if (Session["Company"] != null)
                IdCompany = Session["Company"].ToString();
            else
                IdCompany = "";

            return IdCompany; 
        }

        // GET: /UploadedFile/
        [Authorize(Roles = Profil.ADMIN + "," + Profil.ENTREPRISE + "," + Profil.ENTREPRISE_IARD + "," + Profil.ENTREPRISE_VIE)]
        public ActionResult Index()
        {
            if (Session["Profils"] !=  null && Session["Profils"].ToString() != "")
            {
                //récupérer toutes les campagnes ouvertes
                ObservableCollection<Campaign> listCampaign = repoCampaign.GetCampaignByEndDate();

                if(listCampaign.Count == 0) // pas de campagne ouverte
                {
                    ViewBag.CampaignItems = repoCampaign.GetCampaigns();
                    return View("CampaignUnavailable");
                }
               
                ObservableCollection<UploadingFile> uploadingFiles = repoUploadFile.GetTmpFileNameByIdCompany(GetSessionCompanyId(), listCampaign[0].CampaignId);
                ViewBag.CampaignList = listCampaign;
                return View("UploadFile", uploadingFiles);
            }

            return RedirectToAction("Login", "Account");

        }

        /**
         *  Copie des fichiers d'états dans le repertoire de chargement
         **/
        [Authorize(Roles = Profil.ADMIN + "," + Profil.ENTREPRISE + "," + Profil.ENTREPRISE_IARD + "," + Profil.ENTREPRISE_VIE)]
        public JsonResult SaveFileToLanding(int selectedCampaign)
        {
            String Status;

            // Get les fichiers dans le temporaire pour un IdCompany
            ObservableCollection<UploadingFile> Files = repoUploadFile.GetTmpFileByIdCompany(GetSessionCompanyId());

            if(Files != null && Files.Count > 0)
            {

                List<String> FichiersDeposes = CollectionToList(Files);
                
                // controle d'exhaustivite
                if (!ControlerExhaustivite(selectedCampaign, FichiersRestants, FichiersDeposes))
                {
                    return Json(JSON_RESULT_FAILURE, JsonRequestBehavior.AllowGet);
                }

                // Controle de conformite
                if (!ControlerConformite(selectedCampaign, Files))
                {
                    return Json(JSON_RESULT_FAILURE, JsonRequestBehavior.AllowGet);
                }

                // Controle de coherence
                if (!ControlerCoherence(selectedCampaign))
                {
                    return Json(JSON_RESULT_FAILURE, JsonRequestBehavior.AllowGet);
                }

                string path = ConfigurationManager.AppSettings["LandingPath"];

                // génération du numéro de batch
                string batchNumber = RandomNumber(0,9999999);

                // copie des fichiers PFN et VER dans le repertoire Landing
                foreach (var f in Files)
                {
                    // on déplace le fichier non rejeté
                    if (!listeFichiersRejetes.Contains(f.FileName))
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
                        catch (Exception e)
                        {
                            Console.WriteLine("Error Copy files Generated. Details: " + e.ToString());
                            throw e;
                        }
                    }else
                    {
                        // On enlève de la collection le fichier rejeté.
                        Files.Remove(f);
                    }
                    
                }

                // création du batch
                try
                {
                    BatchModel batchFiles = new BatchModel()
                    {
                        BatchNumber = batchNumber,
                        IdCompany = GetSessionCompanyId(),
                        NbFiles = Files.Count,
                        IdCampagne = selectedCampaign                        
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
        [Authorize(Roles = Profil.ADMIN + "," + Profil.ENTREPRISE + "," + Profil.ENTREPRISE_IARD + "," + Profil.ENTREPRISE_VIE)]
        public JsonResult UploadingFile(string fileName, int filesize, string file, int idCampagne)
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
                    IdCompany = GetSessionCompanyId(),
                    IdCampagne = idCampagne,
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
        [Authorize(Roles = Profil.ENTREPRISE + "," + Profil.ENTREPRISE_IARD + "," + Profil.ENTREPRISE_VIE)]
        public JsonResult DeleteUploadedFile(string filename)
        {
            String StatusReponse;

            int response = repoUploadFile.DeleteTmpFileByFileNameAndIdCompany(filename, GetSessionCompanyId());

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


        public JsonResult CancelUploadingTmpFile()
        {
            String StatusReponse;

            int response = repoUploadFile.DeleteTmpFileByIdCompany(GetSessionCompanyId());

            if (response == 0)
                StatusReponse = JSON_RESULT_FAILURE;
            else
                StatusReponse = JSON_RESULT_SUCCESS;

            return Json(StatusReponse, JsonRequestBehavior.AllowGet);
        }

        

        public JsonResult GetCampaignReport(string selectedCampaign)
        {
            ObservableCollection<String> FichiersAttendus = repoCampaign.GetCampaignFileById(selectedCampaign);

            ObservableCollection<String> FichiersEnvoyes = repoUploadFile.GetFileByCompanyAndCampagneId(selectedCampaign, GetSessionCompanyId());

            FichiersRestants = FichiersAttendus.Except(FichiersEnvoyes).ToList();
            
            ObservableCollection<UploadingFile> FichiersTemporaires = repoUploadFile.GetTmpFileNameByIdCompany(GetSessionCompanyId(), Int32.Parse(selectedCampaign));

            Dictionary<String, Object> response = new Dictionary<string, object>
            {
                ["fichiersAttendus"] = FichiersAttendus,
                ["fichiersEnvoyes"] = FichiersEnvoyes,
                ["fichiersRestants"] = FichiersRestants,
                ["fichiersTemporaires"] = FichiersTemporaires
            };
           
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private Boolean ControlerExhaustivite(int IdCampagne, List<string> FichiersRestants, List<String> FichiersDeposes)
        {
            var controlExhaustivite = repoCampaignControl.GetByCampaignIdAndControlType(IdCampagne, "E");

            if(controlExhaustivite != null)
            {
                var list = FichiersRestants.Except(FichiersDeposes);

                if (list != null && list.Count() > 0)
                {
                    foreach(string f in list)
                    {
                        FichierRejete fichierRejete = new FichierRejete
                        {
                            Filename = f,
                            Reason = "Echec du contrôle d'exhaustivité"
                        };
                        // Logger en base les fichiers rejetés
                        repoFichierRejete.Insert(fichierRejete);
                        unitOfWork.Save();
                        listeFichiersRejetes.Add(f);
                    }

                    var blocking = controlExhaustivite[0];
                    if (blocking == "Y")
                    {
                        return false;
                    }
                    else if(blocking == "N")
                    {
                        return true;
                    }
                }
            }           

            return true;
        }

        private Boolean ControlerConformite(int IdCampagne, ObservableCollection<UploadingFile> Files)
        {
            var controlConformite = repoCampaignControl.GetByCampaignIdAndControlType(IdCampagne, "CF");           

            if (controlConformite != null && controlConformite.Count() > 0)
            {
                foreach (UploadingFile file in Files)
                {
                    var stringFromByteArray = Encoding.UTF8.GetString(file.File).Split('\n');
                    var colonnesFichierDepose = stringFromByteArray[0].Split('|').ToList();

                    var colonnesCI = repoContratInterface.GetByFileMask(file.FileMask);

                    // récuperer la liste des colonnes qui sont dans le CI et non dans le fichier déposé
                    var list = colonnesCI.Except(colonnesFichierDepose);

                    if (list != null && list.Count() > 0)
                    {
                        FichierRejete fichierRejete = new FichierRejete
                        {
                            Filename = file.FileName,
                            Reason = "Echec du contrôle de conformité"
                        };
                        // Logger en base les fichiers rejetés
                        repoFichierRejete.Insert(fichierRejete);
                        unitOfWork.Save();
                        listeFichiersRejetes.Add(file.FileName);

                        var blocking = controlConformite[0];
                        if (blocking == "Y")
                        {
                            return false;
                        }
                        else if (blocking == "N")
                        {
                            return true;
                        }
                    }

                }               
            }

            return true;
        }

        private Boolean ControlerCoherence(int IdCampagne)
        {
            var controlCoherence = repoCampaignControl.GetByCampaignIdAndControlType(IdCampagne, "CH");

            if (controlCoherence != null && controlCoherence.Count() > 0)
            {
                return false;
            }

            return true;
        }

        private List<String> CollectionToList(ObservableCollection<UploadingFile> uploadFiles)
        {
            List<String> list = new List<string>();

            if(uploadFiles != null)
            {
                foreach (UploadingFile f in uploadFiles)
                {
                    list.Add(f.FileMask);
                }
            }
            
            return list;
        }

    }
}
