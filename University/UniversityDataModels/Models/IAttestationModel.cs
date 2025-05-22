using UniversityDataModels.Enums;

namespace UniversityDataModels.Models
{
    public interface IAttestationModel : IId
    {
        int UserId { get; }
        string FormOfEvaluation { get; }
        AttestationScore Score { get; }
        int StudentId { get; }
    }
}
