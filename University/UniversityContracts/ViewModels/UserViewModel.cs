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
    public class UserViewModel : IUserModel
    {
        public int Id { get; set; }
        [DisplayName("Логин")]
        public string Login { get; set; } = string.Empty;

        [DisplayName("Пароль")]
        public string Password { get; set; } = string.Empty;

        [DisplayName("Почта")]
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
