using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDataModels.Enums;
using UniversityDataModels.Models;

namespace UniversityContracts.ViewModels
{
    public class AttestationViewModel : IAttestationModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StudentId { get; set; }
        [DisplayName("ФИО студента")]
        public string StudentName { get; set; } = string.Empty;
        [DisplayName("Форма оценивания")]
        public string FormOfEvaluation { get; set; } = string.Empty;
        [DisplayName("Оценка")]
        public AttestationScore Score { get; set; } = AttestationScore.Неявка;
    }
}
