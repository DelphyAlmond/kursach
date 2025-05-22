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

namespace UniversityBusinessLogic.BusinessLogics
{
    public class TeacherLogic : ITeacherBusinessLogicContract
    {
        private readonly ILogger _logger;
        private readonly ITeacherStorageContract _teacherStorage;
        public TeacherLogic(ILogger<StudentBusinessLogicContract> logger, ITeacherStorageContract
            teacherStorage)
        {
            _logger = logger;
            _teacherStorage = teacherStorage;
        }
        public List<TeacherViewModel>? ReadList(TeacherSearchModel? model)
        {
            _logger.LogInformation("ReadList. Name: {Name}.Id:{Id} ",
                model?.Name, model?.Id);
            var list = model == null ? _teacherStorage.GetFullList() :
                _teacherStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }
        public TeacherViewModel? ReadElement(TeacherSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. Name:{Name}.Id:{Id}",
                model.Name, model.Id);
            var element = _teacherStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }
        public bool Create(TeacherBindingModel model)
        {
            CheckModel(model);
            if (_teacherStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }
        public bool Update(TeacherBindingModel model)
        {
            CheckModel(model);
            if (_teacherStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }
        public bool Delete(TeacherBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_teacherStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }
        private void CheckModel(TeacherBindingModel model, bool withParams = true)
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
                throw new ArgumentNullException("Нет названия",
               nameof(model.Name));
            }
            if (string.IsNullOrEmpty(model.AcademicDegree))
            {
                throw new ArgumentNullException("Не указана учёная степень", nameof(model.AcademicDegree));
            }
            if (string.IsNullOrEmpty(model.Position))
            {
                throw new ArgumentNullException("Нет должности",
               nameof(model.Position));
            }
            if (model.UserId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор пользователя", nameof(model.UserId));
            }
            _logger.LogInformation("Teacher. Name:{Name}.AcademicDegree:{AcademicDegree}. Position:{Position}. UserId: {UserId}. Id: {Id}",
                model.Name, model.AcademicDegree, model.Position, model.UserId, model.Id);
            var element = _teacherStorage.GetElement(new TeacherSearchModel
            {
                Name = model.Name
            });
            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Данный преподаватель уже существует");
            }
        }
    }
}
