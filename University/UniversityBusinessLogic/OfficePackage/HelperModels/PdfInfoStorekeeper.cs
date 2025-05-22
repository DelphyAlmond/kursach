using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.OfficePackage.HelperModels
{
    public class PdfInfoStorekeeper
    {
        public string? FileName { get; set; }
        public Stream? Stream { get; set; }

        public string Title { get; set; } = string.Empty;
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public List<object> ReportObjects { get; set; } = new();
        public List<ReportDisciplineViewModel> Disciplines { get; set; } = new();
    }
}
