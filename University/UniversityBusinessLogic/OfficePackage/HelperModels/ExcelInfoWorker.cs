using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.OfficePackage.HelperModels
{
    public class ExcelInfoWorker
    {
        public string? FileName { get; set; }

        public Stream? Stream { get; set; }

        public string Title { get; set; } = string.Empty;
        public List<object> ReportObjects
        {
            get;
            set;
        } = new();
        public List<ReportPlanOfStudyViewModel> PlanOfStudys { get; set; } = new();
        public List<string> Headers { get; set; } = new();
    }
}
