using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityContracts.BindingModels
{
    public class ReportDateRangeBindingModel
    {
        public string? FileName { get; set; }
        public Stream? Stream { get; set; }
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
    }
}
