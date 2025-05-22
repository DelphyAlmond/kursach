using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniversityDataModels.Models;

namespace UniversityContracts.ViewModels
{
    public class DisciplineViewModel : IDisciplineModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        [DisplayName("Название дисциплины")]
        public string Name { get; set; } = string.Empty;
        [DisplayName("Описание дисциплины")]
        public string Description { get; set; } = string.Empty;
        [DisplayName("Дата cоздания")]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public Dictionary<int, IStudentModel> StudentDisciplines
        {
            get;
            set;
        } = new();

        public DisciplineViewModel() { }

        [JsonConstructor]
        public DisciplineViewModel(Dictionary<int, StudentViewModel> disciplineStudents) {
            this.StudentDisciplines = disciplineStudents.ToDictionary(x => x.Key, x => x.Value as IStudentModel);
        }
    }
}
