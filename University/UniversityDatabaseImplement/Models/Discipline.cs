using System;
using System.Collections.Generic;
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
    public class Discipline : IDisciplineModel
    {
        public int Id { get; private set; }
        [Required]
        public int UserId { get; private set; }

        [Required]
        public int TeacherId { get; private set; }
        [Required]
        public string Name { get; private set; } = string.Empty;
        [Required]
        public string Description { get; private set; } = string.Empty;
        public DateOnly Date { get; private set; }
        public virtual User User { get; set; } = new();
        public virtual Teacher Teacher { get; set; } = new();
        private Dictionary<int, IStudentModel>? _studentDisciplines = null;
        [NotMapped]
        public Dictionary<int, IStudentModel> StudentDisciplines
        {
            get
            {
                if (_studentDisciplines == null)
                {
                    _studentDisciplines = Students
                    .ToDictionary(recPC => recPC.StudentId, recPC => recPC.Student as IStudentModel);
                }
                return _studentDisciplines;
            }
        }
        [ForeignKey("DisciplineId")]
        public virtual List<StudentDiscipline> Students { get; set; } = new();
        public static Discipline Create(UniversityDbContext context, DisciplineBindingModel model)
        {
            return new Discipline()
            {
                Id = model.Id,
                UserId = model.UserId,
                User = context.Users.First(x => x.Id == model.UserId),
                TeacherId = model.TeacherId,
                Teacher = context.Teachers.First(x => x.Id == model.TeacherId),
                Name = model.Name,
                Description = model.Description,
                Students = model.StudentDisciplines.Select(x => new
                StudentDiscipline
                {
                    Student = context.Students.First(y => y.Id == x.Key)
                }).ToList()
            };
        }
        public void Update(DisciplineBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            Id = model.Id;
            TeacherId = model.TeacherId;
            Name = model.Name;
            Description = model.Description;
        }
        public void UpdateStudents(UniversityDbContext context,
      DisciplineBindingModel model)
        {
            var studentDisciplines = context.StudentDisciplines.Where(rec => rec.DisciplineId == model.Id).ToList();
            if (studentDisciplines != null && studentDisciplines.Count > 0)
            { // удалили те, которых нет в модели
                context.StudentDisciplines.RemoveRange(studentDisciplines.Where(rec
               => !model.StudentDisciplines.ContainsKey(rec.StudentId)));
                context.SaveChanges();
                // обновили количество у существующих записей
                foreach (var updateStudent in studentDisciplines)
                {
                    model.StudentDisciplines.Remove(updateStudent.StudentId);
                }
                context.SaveChanges();
            }
            var discipline = context.Disciplines.First(x => x.Id == Id);
            foreach (var pc in model.StudentDisciplines)
            {
                context.StudentDisciplines.Add(new StudentDiscipline
                {
                    Discipline = discipline,
                    Student = context.Students.First(x => x.Id == pc.Key)
                });
                context.SaveChanges();
            }
            _studentDisciplines = null;
        }
        public DisciplineViewModel GetViewModel => new()
        {
            Id = Id,
            UserId = UserId,
            TeacherId = TeacherId,
            TeacherName = Teacher.Name,
            Name = Name,
            Description = Description,
            StudentDisciplines = StudentDisciplines,
        };
    }
}
