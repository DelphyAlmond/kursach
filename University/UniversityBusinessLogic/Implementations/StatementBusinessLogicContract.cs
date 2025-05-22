using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicsContracts;
using UniversityContracts.SearchModels;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using UniversityDataModels.Enums;

namespace UniversityBusinessLogic.BusinessLogics
{
    public class StatementBusinessLogicContract : IStatementBusinessLogicContract
    {
        private readonly ILogger _logger;
        private readonly IStatementStorageContract _statementStorage;
        public StatementBusinessLogicContract(ILogger<StatementBusinessLogicContract> logger, IStatementStorageContract
       statementStorage)
        {
            _logger = logger;
            _statementStorage = statementStorage;
        }
        public List<StatementViewModel>? ReadList(StatementSearchModel? model)
        {
            _logger.LogInformation("ReadList.StatementId:{Id} ",
               model?.Id);
            var list = model == null ? _statementStorage.GetFullList() :
                    _statementStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }
        public StatementViewModel? ReadElement(StatementSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement.Id:{Id}",
                model.Id);
            var element = _statementStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }
        public bool Create(StatementBindingModel model)
        {
            CheckModel(model);

            if (_statementStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public bool Update(StatementBindingModel model)
        {
            CheckModel(model);
            if (_statementStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }
        public bool Delete(StatementBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_statementStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(StatementBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Не выбрано название ведомости", nameof(model.Name));
            }

            if (model.TeacherId <= 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор преподавателя", nameof(model.TeacherId));
            }
            if (model.UserId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор пользователя", nameof(model.UserId));
            }
            _logger.LogInformation("Statement. StatementId:{Id}.Name:{Name}. TeacherId: {TeacherId}. UserId: {UserId}", 
                model.Id, model.Name, model.TeacherId, model.UserId);
        }
    }
}
