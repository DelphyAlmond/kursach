namespace UniversityDataModels.Models
{
    public interface IPlanOfStudyModel : IId
    {
        int UserId { get; }
        string Profile { get; }
        string FormOfStudy { get; }
    }
}
