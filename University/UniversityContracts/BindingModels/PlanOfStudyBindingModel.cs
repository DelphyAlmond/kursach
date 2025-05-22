using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDataModels.Models;

namespace UniversityContracts.BindingModels
{
    public class PlanOfStudyBindingModel : IPlanOfStudyModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Profile { get; set; } = string.Empty;
        public string FormOfStudy { get; set; } = string.Empty;
        public int WorkerId { get; set; }
        public Dictionary<int, ITeacherModel> PlanOfStudyTeachers { get; set; } = new();
    }
}
