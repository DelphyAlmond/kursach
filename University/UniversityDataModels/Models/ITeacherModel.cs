namespace UniversityDataModels.Models
{
    public interface ITeacherModel : IId
    {
        int UserId { get; }
        string Name { get; }
        string AcademicDegree { get; }
        string Position { get; }
    }
}
