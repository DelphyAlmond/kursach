using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityContracts.SearchModels
{
    public class TeacherSearchModel
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? AcademicDegree { get; set; }
        public string? Position { get; set; }
    }
}
