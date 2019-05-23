using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvcApplication1.Models.TestModel;
using MvcApplication1.Models.Shared;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.Repository.TestData
{
    public interface IREPO_Paie
    {
        Dictionary<string, ObservableCollection<PaieCriteria>> GetFilterData(FiltreDashboard filtre);

        ObservableCollection<Effectif> GetNumberofEmployeesbyYear(FiltreDashboard filtre);

        ObservableCollection<Effectif> GetHeadcount(FiltreDashboard filtre);

        ObservableCollection<Montant> GetSalaryGraph(FiltreDashboard filtre);

        ObservableCollection<Effectif> GetNumberofEmployeesbySalary(FiltreDashboard filtre);

      
        ObservableCollection<Montant> GetPayrollBreakdown(FiltreDashboard filtre);

      
        ObservableCollection<Montant> GetTaxDeductionGraph(FiltreDashboard filtre);
    }
}
