using Microsoft.AspNetCore.Mvc;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicsContracts;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;

namespace UniversityRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttestationController : Controller
    {
        private readonly ILogger _logger;
        private readonly IAttestationBusinessLogicContract _logic;
        public AttestationController(IAttestationBusinessLogicContract logic, ILogger<AttestationController> logger)
        {
            _logic = logic;
            _logger = logger;
        }

        [HttpGet]
        public List<AttestationViewModel>? GetAttestations(int userId)
        {
            try
            {
                return _logic.ReadList(new AttestationSearchModel { UserId = userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения аттестаций пользователя id={Id}", userId);
                throw;
            }
        }
        [HttpGet]
        public AttestationViewModel? GetAttestation(int userId, int id)
        {
            try
            {
                return _logic.ReadElement(new AttestationSearchModel { UserId = userId, Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения аттестаций пользователя id={Id}", userId);
                throw;
            }
        }
        [HttpPost]
        public void CreateAttestation(AttestationBindingModel model)
        {
            try
            {
                _logic.CreateAttestation(model);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания аттестации");
                throw;
            }
        }
        [HttpPost]
        public void UpdateAttestation(AttestationBindingModel model)
        {
            try
            {
                _logic.UpdateAttestation(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления аттестации");
                throw;
            }
        }
        [HttpPost]
        public void DeleteAttestation(AttestationBindingModel model)
        {
            try
            {
                _logic.DeleteAttestation(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления аттестации");
                throw;
            }
        }
    }
}
