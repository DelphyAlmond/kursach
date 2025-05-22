using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDataModels.Models;

namespace UniversityContracts.ViewModels
{
    public class StatementViewModel : IStatementModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        [DisplayName("Название ведомости")]
        public string Name { get; set; } = string.Empty;
        [DisplayName("Дата оформления ведомости")]
        public DateTime Date { get; set; }
    }
}
