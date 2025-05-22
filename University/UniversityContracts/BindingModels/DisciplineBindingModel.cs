using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDataModels.Models;

namespace UniversityContracts.BindingModels
{
    public class DisciplineBindingModel : IDisciplineModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TeacherId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public Dictionary<int, IStudentModel> StudentDisciplines { get; set; } = new();
    }
}
