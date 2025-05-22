using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityContracts.ViewModels
{
    public class ReportPlanOfStudyViewModel
    {
        public int Id { get; set; }
        public string PlanOfStudyName { get; set; } = string.Empty;
        public string FormOfStudy { get; set; } = string.Empty;
        public List<string> Disciplines { get; set; } = new();
    }
}
