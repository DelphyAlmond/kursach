namespace UniversityDataModels.Models
{
    public interface IStudentModel : IId
    {
        int UserId { get; }
        string Name { get; }
        string PhoneNumber { get; }
        int PlanOfStudyId { get; }
    }
}
