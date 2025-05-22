using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.OfficePackage.HelperModels
{
    public class PdfInfoWorker
    {
        public string? FileName { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<ReportPlanOfStudyAndStudentViewModel> PlanOfStudyAndStudent { get; set; } = new();
    }
}
