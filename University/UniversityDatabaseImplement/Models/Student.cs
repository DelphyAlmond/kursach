using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;
using UniversityDataModels.Models;

namespace UniversityDatabaseImplement.Models
{
    public class Student : IStudentModel
    {
        public int Id { get; private set; }
        [Required]
        public int UserId { get; private set; }
        [Required]
        public int PlanOfStudyId { get; private set; }
        public string PlanOfStudyProfile { get; private set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; private set; } = string.Empty;
        [ForeignKey("StudentId")]
        public virtual List<StudentDiscipline> StudentDiscipline { get; set; } = new();
        [ForeignKey("StudentId")]
        public virtual List<Attestation> Attestations { get; set; } = new();
        public virtual PlanOfStudy PlanOfStudy { get; set; } = new();
        public virtual User User { get; set; } = new();
        public static Student? Create(UniversityDbContext context, StudentBindingModel model)
        {
            return new Student()
            {
                Id = model.Id,
                UserId = model.UserId,
                User = context.Users.First(x => x.Id == model.UserId),
                PlanOfStudyId = model.PlanOfStudyId,
                PlanOfStudy = context.PlanOfStudys.First(x => x.Id == model.PlanOfStudyId),
                PlanOfStudyProfile = model.PlanOfStudyProfile,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber
            };
        }
        public void Update(StudentBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            PhoneNumber = model.PhoneNumber;
            Name = model.Name;
            PlanOfStudyId = model.PlanOfStudyId;
            PlanOfStudyProfile = model.PlanOfStudyProfile;
        }
        public StudentViewModel GetViewModel => new()
        {
            Id = Id,
            PlanOfStudyId = PlanOfStudyId,
            PlanOfStudyProfile = PlanOfStudyProfile,
            UserId = UserId,
            Name = Name,
            PhoneNumber = PhoneNumber
        };
    }
}
