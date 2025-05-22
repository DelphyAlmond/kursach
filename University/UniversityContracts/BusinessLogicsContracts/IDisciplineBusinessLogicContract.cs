using UniversityContracts.BindingModels;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicsContracts
{
    public interface IDisciplineBusinessLogicContract
    {
        List<DisciplineViewModel>? ReadList(DisciplineSearchModel? model);
        DisciplineViewModel? ReadElement(DisciplineSearchModel model);
        bool Create(DisciplineBindingModel model);
        bool Update(DisciplineBindingModel model);
        bool Delete(DisciplineBindingModel model);
    }
}
