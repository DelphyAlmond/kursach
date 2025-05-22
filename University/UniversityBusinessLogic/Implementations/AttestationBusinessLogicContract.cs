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
    public class AttestationBusinessLogicContract : IAttestationBusinessLogicContract
    {
        private readonly ILogger _logger;
        private readonly IAttestationStorage _attestationStorage;
        public AttestationBusinessLogicContract(ILogger<AttestationBusinessLogicContract> logger, IAttestationStorage
       attestationStorage)
        {
            _logger = logger;
            _attestationStorage = attestationStorage;
        }
        public List<AttestationViewModel>? ReadList(AttestationSearchModel? model)
        {
            _logger.LogInformation("ReadList.AttestationId:{Id} ",
               model?.Id);
            var list = model == null ? _attestationStorage.GetFullList() :
                    _attestationStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }
        public AttestationViewModel? ReadElement(AttestationSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement.Id:{Id}",
                model.Id);
            var element = _attestationStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }
        public bool CreateAttestation(AttestationBindingModel model)
        {
            CheckModel(model);

            if (_attestationStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }
        public bool DeleteAttestation(AttestationBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_attestationStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete attestation operation failed");
                return false;
            }
            return true;
        }
        public bool UpdateAttestation(AttestationBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Update. Id:{Id}", model.Id);
            if (_attestationStorage.Update(model) == null)
            {
                _logger.LogWarning("Update attestation operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(AttestationBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.FormOfEvaluation))
            {
                throw new ArgumentNullException("Не выбрана форма оценивания", nameof(model.FormOfEvaluation));
            }
            if (model.UserId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор пользователя", nameof(model.UserId));
            }

            if (model.StudentId <= 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор студента", nameof(model.StudentId));
            }
            _logger.LogInformation("Attestation. AttestationId:{Id}.FormOfEvaluation:{FormOfEvaluation}. StudentId: {StudentId}. UserId: {UserId}", 
                model.Id, model.FormOfEvaluation, model.StudentId, model.UserId);
        }
    }
}
