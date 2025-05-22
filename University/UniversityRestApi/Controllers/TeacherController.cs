using Microsoft.AspNetCore.Mvc;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.BusinessLogicsContracts;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;

namespace UniversityRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeacherController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITeacherBusinessLogicContract _logic;
        private readonly IReportContract _reportLogic;
        public TeacherController(ITeacherBusinessLogicContract logic, ILogger<TeacherController> logger, IReportContract reportLogic)
        {
            _logic = logic;
            _logger = logger;
            _reportLogic = reportLogic;
        }

        [HttpGet]
        public TeacherViewModel? GetTeacher(int userId, int id)
        {
            try
            {
                return _logic.ReadElement(new TeacherSearchModel { UserId = userId, Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения teacher user {Id}", userId);
                throw;
            }
        }

        [HttpGet]
        public List<TeacherViewModel>? GetTeachers(int userId)
        {
            try
            {
                return _logic.ReadList(new TeacherSearchModel { UserId = userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка преподавателей пользователя id={Id}", userId);
                throw;
            }
        }
		[HttpGet]
		public List<TeacherViewModel>? GetAllTeachers()
		{
			try
			{
				return _logic.ReadList(null);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка получения списка преподавателей");
				throw;
			}
		}
		[HttpPost]
        public void CreateTeacher(TeacherBindingModel model)
        {
            try
            {
                _logic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания преподавателя");
                throw;
            }
        }
        [HttpPost]
        public void UpdateTeacher(TeacherBindingModel model)
        {
            try
            {
                _logic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления преподавателя");
                throw;
            }
        }
        [HttpPost]
        public void DeleteTeacher(TeacherBindingModel model)
        {
            try
            {
                _logic.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления преподавателя");
                throw;
            }
        }

        [HttpGet]
        public List<ReportTeacherViewModel>? GetTeachersReport(int userId)
        {
            try
            {
                return _reportLogic.GetTeachers(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка планов обучения");
                throw;
            }
        }

        [HttpPost]
        public void LoadReportToWord(ReportBindingModel model)
        {
            try
            {
                _reportLogic.SaveTeachersToWord(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
        }
        [HttpPost]
        public void LoadReportToExcel(ReportBindingModel model)
        {
            try
            {
                _reportLogic.SaveTeachersToExcel(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
        }
    }
}
