using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityDatabaseImplement.Models
{
    public class StudentDiscipline
    {
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int DisciplineId { get; set; }
        public virtual Student Student { get; set; } = new ();
        public virtual Discipline Discipline { get; set; } = new();
    }
}
