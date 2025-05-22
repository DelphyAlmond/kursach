using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityContracts.SearchModels
{
    public class PlanOfStudySearchModel
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string? Profile { get; set; }
    }
}
