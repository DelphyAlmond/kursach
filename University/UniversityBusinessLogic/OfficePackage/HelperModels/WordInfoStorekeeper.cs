using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.OfficePackage.HelperModels
{
    public class WordInfoStorekeeper
    {
        public string? FileName { get; set; }
        public Stream? Stream { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<object> ReportObjects { get; set; } = new();
        public List<ReportTeacherViewModel> TeacherInfo{ get; set; } = new();
    }
}
