using UniversityContracts.BindingModels;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicsContracts
{
    public interface IStudentBusinessLogicContract
    {
        List<StudentViewModel>? ReadList(StudentSearchModel? model);
        StudentViewModel? ReadElement(StudentSearchModel model);
        bool Create(StudentBindingModel model);
        bool Update(StudentBindingModel model);
        bool Delete(StudentBindingModel model);
    }
}
