using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.StorageContracts
{
    public interface ITeacherStorageContract
    {
        List<TeacherViewModel> GetFullList();
        List<TeacherViewModel> GetFilteredList(TeacherSearchModel model);
        TeacherViewModel? GetElement(TeacherSearchModel model);
        TeacherViewModel? Insert(TeacherBindingModel model);
        TeacherViewModel? Update(TeacherBindingModel model);
        TeacherViewModel? Delete(TeacherBindingModel model);
    }
}
