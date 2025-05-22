using Microsoft.Extensions.Logging;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicsContracts;
using UniversityContracts.SearchModels;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;


namespace UniversityBusinessLogic.BusinessLogics
{
    public class StudentBusinessLogicContract : IStudentBusinessLogicContract
    {
        private readonly ILogger _logger;
        private readonly IStudentStorageContract _studentStorage;
        public StudentBusinessLogicContract(ILogger<StudentBusinessLogicContract> logger, IStudentStorageContract studentStorage)
        {
            _logger = logger;
            _studentStorage = studentStorage;
        }
        public List<StudentViewModel>? ReadList(StudentSearchModel? model)
        {
            _logger.LogInformation("ReadList. Name: {Name}.Id:{Id} ",
                model?.Name, model?.Id);
            var list = model == null ? _studentStorage.GetFullList() :
                _studentStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }
        public StudentViewModel? ReadElement(StudentSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. Name:{Name}.Id:{Id}",
                model.Name, model.Id);
            var element = _studentStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }
        public bool Create(StudentBindingModel model)
        {
            CheckModel(model);
            if (_studentStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }
        public bool Update(StudentBindingModel model)
        {
            CheckModel(model);
            if (_studentStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }
        public bool Delete(StudentBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_studentStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }
        private void CheckModel(StudentBindingModel model, bool withParams = true)
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
                throw new ArgumentNullException("Нет имени студента",
               nameof(model.Name));
            }
            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                throw new ArgumentNullException("Должен быть номер телефона", nameof(model.PhoneNumber));
            }
            if (model.UserId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор пользователя", nameof(model.UserId));
            }
            _logger.LogInformation("Student. Name:{Name}.PhoneNumber:{PhoneNumber}. UserId: {UserId}. Id: {Id}",
                model.Name, model.PhoneNumber, model.UserId, model.Id);
            var element = _studentStorage.GetElement(new StudentSearchModel
            {
                Name = model.Name
            });
            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Данный студент уже существует");
            }
        }
    }
}
