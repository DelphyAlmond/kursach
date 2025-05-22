using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityContracts.SearchModels
{
    public class StatementSearchModel
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int TeacherId { get; set; }
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
    }
}
