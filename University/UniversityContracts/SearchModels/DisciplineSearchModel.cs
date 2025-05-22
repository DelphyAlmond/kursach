using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityContracts.SearchModels
{
    public class DisciplineSearchModel
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int? TeacherId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateOnly? DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }
    }
}
