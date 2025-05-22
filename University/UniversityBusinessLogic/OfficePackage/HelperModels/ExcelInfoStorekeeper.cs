using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.OfficePackage.HelperModels
{
    public class ExcelInfoStorekeeper
    {
        public string? FileName { get; set; }

        public Stream? Stream { get; set; }

        public string Title { get; set; } = string.Empty;
        public List<object> ReportObjects
        {
            get;
            set;
        } = new();
        public List<ReportTeacherViewModel> Teachers { get; set; } = new();
        public List<string> Headers { get; set; } = new();
    }
}
