using UniversityContracts.BindingModels;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicsContracts
{
    public interface IStatementBusinessLogicContract
    {
        List<StatementViewModel>? ReadList(StatementSearchModel? model);
        StatementViewModel? ReadElement(StatementSearchModel model);
        bool Create(StatementBindingModel model);
        bool Update(StatementBindingModel model);
        bool Delete(StatementBindingModel model);
    }
}
