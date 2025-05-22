using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityContracts.ViewModels
{
    public class ReportPlanOfStudyAndStudentViewModel
    {
        public int Id { get; set; }
        public string PlanOfStudyName { get; set; } = string.Empty;
        public List<string> StudentName { get; set; } = new();
        public List<string> DisciplineName { get; set; } = new();
    }
}
