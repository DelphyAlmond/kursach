using UniversityContracts.BindingModels;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicsContracts
{
    public interface ITeacherBusinessLogicContract
    {
        List<TeacherViewModel>? ReadList(TeacherSearchModel? model);
        TeacherViewModel? ReadElement(TeacherSearchModel model);
        bool Create(TeacherBindingModel model);
        bool Update(TeacherBindingModel model);
        bool Delete(TeacherBindingModel model);
    }
}
