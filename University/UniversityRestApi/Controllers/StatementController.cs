using Microsoft.AspNetCore.Mvc;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicsContracts;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;
using UniversityDataModels.Enums;

namespace UniversityRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatementController : Controller
    {
        private readonly ILogger _logger;
        private readonly IStatementBusinessLogicContract _logic;
        public StatementController(IStatementBusinessLogicContract logic, ILogger<StatementController> logger)
        {
            _logic = logic;
            _logger = logger;
        }

        [HttpGet]
        public List<StatementViewModel>? GetStatements(int teacherId, int userId)
        {
            try
            {
                if (teacherId == 0)
                {
                    return _logic.ReadList(null);
                }
                return _logic.ReadList(new StatementSearchModel { TeacherId = teacherId, UserId = userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения ведомости id={Id}", teacherId);
                throw;
            }
        }

        [HttpGet]
        public StatementViewModel? GetStatement(int userId, int id)
        {
            try
            {
                return _logic.ReadElement(new StatementSearchModel { UserId = userId, Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения ведомости id={Id}", userId);
                throw;
            }
        }

        [HttpPost]
        public void CreateStatement(StatementBindingModel model)
        {
            try
            {
                _logic.Create(model);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания ведомости");
                throw;
            }
        }
        [HttpPost]
        public void UpdateStatement(StatementBindingModel model)
        {
            try
            {
                _logic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления ведомости");
                throw;
            }
        }
        [HttpPost]
        public void DeleteStatement(StatementBindingModel model)
        {
            try
            {
                _logic.Delete(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления ведомости");
                throw;
            }
        }
    }
}
