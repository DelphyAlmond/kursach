using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDataModels.Models;

namespace UniversityContracts.ViewModels
{
    public class TeacherViewModel : ITeacherModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [DisplayName("ФИО")]
        public string Name { get; set; } = string.Empty;
        [DisplayName("Учёная степень")]
        public string AcademicDegree { get; set; } = string.Empty;
        [DisplayName("Должность")]
        public string Position { get; set; } = string.Empty;

        public List<StudentViewModel> StudentViewModels { get; set; } = new();

        /// <summary>
        /// Для отчета по моделям
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new StringBuilder(
                    $"Преподаватель {Name} включает в себя группу студентов:");
            for (int i = 0; i < StudentViewModels.Count; i++)
            {
                var student = StudentViewModels[i];
                result.Append($"\n\t{i + 1}. {student.Name}" +
                              $"{student.PhoneNumber}");
            }
            return result.ToString();
        }
    }
}
