using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityContracts.ViewModels
{
    public class ReportDisciplineViewModel
    {
        public string DisciplineName { get; set; } = string.Empty;
        public List<string> PlanOfStudys { get; set; } = new();
        public List<string> Statements { get; set; } = new();
    }
}