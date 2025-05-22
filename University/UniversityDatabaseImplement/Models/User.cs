using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;
using UniversityDataModels.Models;
using UniversityDataModels.Enums;

namespace UniversityDatabaseImplement.Models
{
    public class User : IUserModel
    {
        public int Id { get; private set; }

        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public UserRole Role { get; set; }
        [ForeignKey("UserId")]
        public virtual List<Statement> Statements { get; set; } = new();
        [ForeignKey("UserId")]
        public virtual List<Discipline> Disciplines { get; set; } = new();
        [ForeignKey("UserId")]
        public virtual List<Teacher> Teachers { get; set; } = new();
        [ForeignKey("UserId")]
        public virtual List<Attestation> Attestations { get; set; } = new();
        [ForeignKey("UserId")]
        public virtual List<PlanOfStudy> PlanOfStudys { get; set; } = new();
        [ForeignKey("UserId")]
        public virtual List<Student> Students{ get; set; } = new();
        public static User Create(UserBindingModel model)
        {
            return new User
            {
                Id = model.Id,
                Login = model.Login,
                Password = model.Password,
                Email = model.Email,
                Role = model.Role
            };
        }

        public void Update(UserBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            Login = model.Login;
            Password = model.Password;
            Email = model.Email;
        }
        public UserViewModel GetViewModel => new()
        {
            Id = Id,
            Login = Login,
            Password = Password,
            Email = Email,
            Role = Role
        };
    }
}
