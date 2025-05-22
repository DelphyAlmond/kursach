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
    public class PlanOfStudyBusinessLogicContract : IPlanOfStudyBusinessLogicContract
    {
        private readonly ILogger _logger;
        private readonly IPlanOfStudyStorageContract _planOfStudyStorage;
        public PlanOfStudyBusinessLogicContract(ILogger<StudentBusinessLogicContract> logger, IPlanOfStudyStorageContract
            planOfStudyStorage)
        {
            _logger = logger;
            _planOfStudyStorage = planOfStudyStorage;
        }
        public List<PlanOfStudyViewModel>? ReadList(PlanOfStudySearchModel? model)
        {
            _logger.LogInformation("ReadList. Profile: {Profile}.Id:{Id} ",
                model?.Profile, model?.Id);
            var list = model == null ? _planOfStudyStorage.GetFullList() :
                _planOfStudyStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }
        public PlanOfStudyViewModel? ReadElement(PlanOfStudySearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. Profile:{Profile}.Id:{Id}",
                model.Profile, model.Id);
            var element = _planOfStudyStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }
        public bool Create(PlanOfStudyBindingModel model)
        {
            CheckModel(model);
            if (_planOfStudyStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }
        public bool Update(PlanOfStudyBindingModel model)
        {
            CheckModel(model);
            if (_planOfStudyStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }
        public bool Delete(PlanOfStudyBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_planOfStudyStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }
		private void CheckModel(PlanOfStudyBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.Profile))
            {
                throw new ArgumentNullException("Нет названия профиля",
               nameof(model.Profile));
            }
            if (string.IsNullOrEmpty(model.FormOfStudy))
            {
                throw new ArgumentNullException("Не указана форма обучения", nameof(model.FormOfStudy));
            }
            if (model.UserId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор пользователя", nameof(model.UserId));
            }
            _logger.LogInformation("Student. Profile:{Profile}.FormOfStudy:{FormOfStudy}. UserId: {UserId}. Id: {Id}",
                model.Profile, model.FormOfStudy, model.UserId, model.Id);
            var element = _planOfStudyStorage.GetElement(new PlanOfStudySearchModel
            {
                Profile = model.Profile
            });
            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Данный план обучения уже существует");
            }
        }
    }
}
