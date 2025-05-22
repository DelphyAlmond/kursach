using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityDatabaseImplement.Models
{
    public class PlanOfStudyTeacher
    {
        public int Id { get; set; }
        [Required]
        public int PlanOfStudyId { get; set; }
        [Required]
        public int TeacherId { get; set; }
        public virtual PlanOfStudy PlanOfStudy { get; set; } = new();
        public virtual Teacher Teacher { get; set; } = new();
    }
}
