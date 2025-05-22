using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDataModels.Enums;
using UniversityDataModels.Models;

namespace UniversityContracts.BindingModels
{
    public class AttestationBindingModel : IAttestationModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FormOfEvaluation { get; set; } = string.Empty;
        public AttestationScore Score { get; set; } = AttestationScore.Неявка;
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
    }
}
