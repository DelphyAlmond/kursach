using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDataModels.Enums;
using UniversityDataModels.Models;

namespace UniversityContracts.BindingModels
{
    public class UserBindingModel : IUserModel
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Неизвестная;
    }
}
