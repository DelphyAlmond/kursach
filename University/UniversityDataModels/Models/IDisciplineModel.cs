namespace UniversityDataModels.Models
{
    public interface IDisciplineModel : IId
    {
        int UserId { get; }
        int TeacherId { get; }
        string Name { get; }
        string Description { get; }
    }
}
