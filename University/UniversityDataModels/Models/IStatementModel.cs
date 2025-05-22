namespace UniversityDataModels.Models
{
    public interface IStatementModel : IId
    {
        int UserId { get; }
        int TeacherId { get; }
        string Name { get; }
        DateTime Date { get; }
    }
}
