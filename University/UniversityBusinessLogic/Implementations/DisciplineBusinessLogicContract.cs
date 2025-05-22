using Microsoft.Extensions.Logging;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicsContracts;
using UniversityContracts.SearchModels;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.BusinessLogics
{
    public class DisciplineBusinessLogicContract: IDisciplineBusinessLogicContract
    {
        private readonly ILogger _logger;
        private readonly IDisciplineStorageContract _disciplineStorage;
        public DisciplineBusinessLogicContract(ILogger<DisciplineBusinessLogicContract> logger, IDisciplineStorageContract disciplineStorage)
        {
            _logger = logger;
            _disciplineStorage = disciplineStorage;
        }
        public List<DisciplineViewModel>? ReadList(DisciplineSearchModel? model)
        {
            _logger.LogInformation("ReadList. Name: {Name}.Id:{Id} ",
                model?.Name, model?.Id);
            var list = model == null ? _disciplineStorage.GetFullList() :
                _disciplineStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }
        public DisciplineViewModel? ReadElement(DisciplineSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. Name:{Name}.Id:{Id}",
                model.Name, model.Id);
            var element = _disciplineStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }
        public bool Create(DisciplineBindingModel model)
        {
            CheckModel(model);
            if (_disciplineStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }
        public bool Update(DisciplineBindingModel model)
        {
            CheckModel(model);
            if (_disciplineStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }
        public bool Delete(DisciplineBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_disciplineStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }
        private void CheckModel(DisciplineBindingModel model, bool withParams = true)
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
                throw new ArgumentNullException("Нет названия дисциплины",
               nameof(model.Name));
            }
            if (string.IsNullOrEmpty(model.Description))
            {
                throw new ArgumentNullException("Должна быть дисциплина", nameof(model.Description));
            }
            if (model.UserId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор пользователя", nameof(model.UserId));
            }
            _logger.LogInformation("Discipline. Name:{Name}.Description:{Description}. UserId: {UserId}. Id: {Id}",
                model.Name, model.Description, model.UserId, model.Id);
            var element = _disciplineStorage.GetElement(new DisciplineSearchModel
            {
                Name = model.Name
            });
            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Данная дисциплина уже существует");
            }
        }
    }
}
