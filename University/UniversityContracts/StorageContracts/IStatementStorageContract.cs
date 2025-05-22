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
    public interface IStatementStorageContract
    {
        List<StatementViewModel> GetFullList();
        List<StatementViewModel> GetFilteredList(StatementSearchModel model);
        StatementViewModel? GetElement(StatementSearchModel model);
        StatementViewModel? Insert(StatementBindingModel model);
        StatementViewModel? Update(StatementBindingModel model);
        StatementViewModel? Delete(StatementBindingModel model);
    }
}
