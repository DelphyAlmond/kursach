using UniversityDataModels.Models;

namespace UniversityContracts.BindingModels
{
    public class StudentBindingModel : IStudentModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlanOfStudyId { get; set; }
        public string PlanOfStudyProfile { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
