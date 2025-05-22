using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDataModels.Enums;

namespace UniversityDataModels.Models
{
    public interface IUserModel : IId
    {
        public string Login { get; }
        public string Password { get; }

        public string Email { get; }
        public UserRole Role { get; }
    }
}
