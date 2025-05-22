using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;
using UniversityDataModels.Enums;
using UniversityDataModels.Models;

namespace UniversityDatabaseImplement.Models
{
    public class Attestation : IAttestationModel
    {
        public int Id { get; private set; }
        [Required]
        public int UserId { get; private set; }
        [Required]
        public int StudentId { get; private set; }
        public string StudentName { get; private set; } = string.Empty;
        [Required]
        public string FormOfEvaluation { get; private set; } = string.Empty;
        [Required]
        public AttestationScore Score { get; private set; } = AttestationScore.Неявка;
        public virtual Student Student { get; set; } = new();
        public virtual User User { get; set; } = new();
        public static Attestation? Create(UniversityDbContext context, AttestationBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Attestation()
            {
                Id = model.Id,
                UserId = model.UserId,
                User = context.Users.First(x => x.Id == model.UserId),
                StudentId = model.StudentId,
                Student = context.Students.First(x => x.Id == model.StudentId),
                StudentName = model.StudentName,
                FormOfEvaluation = model.FormOfEvaluation,
                Score = model.Score
            };
        }
        public void Update(AttestationBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            StudentId = model.StudentId;
            StudentName = model.StudentName;
            FormOfEvaluation = model.FormOfEvaluation;
            Score = model.Score;
        }
        public AttestationViewModel GetViewModel => new()
        {
            Id = Id,
            StudentId = StudentId,
            StudentName = StudentName,
            FormOfEvaluation = FormOfEvaluation,
            Score = Score
        };
    }
}
