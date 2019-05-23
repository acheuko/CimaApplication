using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvcApplication1.Models.Shared;
using MvcApplication1.Models.TestModel;
using System.Linq;
using System.Web;

namespace MvcApplication1.Repository.TestData
{
    public class REPO_Paie : IREPO_Paie
    {

        private static REPO_Paie repo = new REPO_Paie();
        public static IREPO_Paie getRepository()
        {
            return repo;
        } 

        public Dictionary<string, ObservableCollection<PaieCriteria>> GetFilterData(FiltreDashboard filtre)
        {
            return new _REPO_PaieCriteria().GetFilterData(filtre);
        }

        public ObservableCollection<Effectif> GetNumberofEmployeesbyYear(FiltreDashboard filtre)
        {
                return new _REPO_NumberEmployeeByYear().GetNumberofEmployeesbyYearData(filtre);
        }

        /// <summary>
        /// cette déclaration expose la méthode du service qui va contenir les données pour le control HeadCount
        /// </summary>
        /// <param name="filtre">le premier parametre contient les valeurs du filtre lorsque la requête  renvoyant les données sera construite</param>
        /// <param name="bloc">utiliser par exemple lors de l'utilisation d'un dictionnaire</param>
        /// <returns> Expose le service</returns>
        public ObservableCollection<Effectif> GetHeadcount(FiltreDashboard filtre)
        {
            return new _REPO_TopFiveEntiteAdmin().GetHeadCountData(filtre);
           
        }

        /// <summary>
        /// cette déclaration expose la méthode du service qui va contenir les données pour le control GetSalaryGraph
        /// </summary>
        /// <param name="filtre">le premier parametre contient les valeurs du filtre lorsque la requête  renvoyant les données sera construite</param>
        /// <param name="bloc">utiliser par exemple lors de l'utilisation d'un dictionnaire</param>
        /// <returns> Expose le service</returns>
        public ObservableCollection<Montant> GetSalaryGraph(FiltreDashboard filtre)
        {
            return new _REPO_EvolutionMasseSalariale().GetSalaryGraphData(filtre);
        }

        /// <summary>
        /// cette déclaration expose la méthode du  service qui va contenir les données pour le control NumberofEmployeesbySalary
        /// </summary>
        /// <param name="filtre">le premier parametre contient les valeurs du filtre lorsque la requête  renvoyant les données sera construite</param>
        /// <param name="bloc">utiliser par exemple lors de l'utilisation d'un dictionnaire</param>
        /// <returns> Expose le service</returns>
        public ObservableCollection<Effectif> GetNumberofEmployeesbySalary(FiltreDashboard filtre)
        {
            return new _REPO_EffectifCatSocioPro().GetNumberofEmployeesbySalaryData(filtre);
        }

        /// <summary>
        /// cette déclaration expose la méthode du service qui va contenir les données pour le control PayrollBreakdown
        /// </summary>
        /// <param name="filtre">le premier parametre contient les valeurs du filtre lorsque la requête  renvoyant les données sera construite</param>
        /// <param name="bloc">utiliser par exemple lors de l'utilisation d'un dictionnaire</param>
        /// <returns> Expose le service</returns>
        public ObservableCollection<Montant> GetPayrollBreakdown(FiltreDashboard filtre)
        {
            return new _REPO_SalaireEntiteAdmin().GetPayrollBreakDownData(filtre);
        }

        /// <summary>
        /// cette déclaration expose la méthode du service qui va contenir les données pour le control TaxDeductionGraph
        /// </summary>
        /// <param name="filtre">le premier parametre contient les valeurs du filtre lorsque la requête  renvoyant les données sera construite</param>
        /// <param name="bloc">utiliser par exemple lors de l'utilisation d'un dictionnaire</param>
        /// <returns> Expose le service</returns>
        public ObservableCollection<Montant> GetTaxDeductionGraph(FiltreDashboard filtre)
        {
                return new _REPO_TaxDeductionGraph().GetTaxDeductionGraphData(filtre);
        }

    }
}