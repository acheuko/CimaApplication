using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cima.Models.TestModel;
using Cima.Models.Shared;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cima.Repository.TestData
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
