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
    public interface IDisciplineStorageContract
    {
        List<DisciplineViewModel> GetFullList();
        List<DisciplineViewModel> GetFilteredList(DisciplineSearchModel model);
        DisciplineViewModel? GetElement(DisciplineSearchModel model);
        DisciplineViewModel? Insert(DisciplineBindingModel model);
        DisciplineViewModel? Update(DisciplineBindingModel model);
        DisciplineViewModel? Delete(DisciplineBindingModel model);
        List<StudentViewModel> GetStudentsForDiscipline(DisciplineSearchModel model);
    }
}
