using Microsoft.AspNetCore.Mvc;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicsContracts;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;

namespace UniversityRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly ILogger _logger;
        private readonly IStudentBusinessLogicContract _logic;
        public StudentController(IStudentBusinessLogicContract logic, ILogger<StudentController> logger)
        {
            _logic = logic;
            _logger = logger;
        }
        [HttpGet]
        public List<StudentViewModel>? GetStudents(int userId)
        {
            try
            {
                return _logic.ReadList(new StudentSearchModel { UserId = userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка студентов пользователя id={Id}", userId);
                throw;
            }
        }
        [HttpGet]
        public List<StudentViewModel>? GetAllStudents()
        {
            try
            {
                return _logic.ReadList(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка ");
                throw;
            }
        }
        [HttpGet]
        public StudentViewModel? GetStudent(int userId, int studentId)
        {
            try
            {
                return _logic.ReadElement(new StudentSearchModel { UserId = userId, Id = studentId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка студентов пользователя id={Id}", userId);
                throw;
            }
        }
        [HttpPost]
        public void CreateStudent(StudentBindingModel model)
        {
            try
            {
                _logic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания студента");
                throw;
            }
        }
        [HttpPost]
        public void UpdateStudent(StudentBindingModel model)
        {
            try
            {
                _logic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления студента");
                throw;
            }
        }
        [HttpPost]
        public void DeleteStudent(StudentBindingModel model)
        {
            try
            {
                _logic.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления студента");
                throw;
            }
        }
    }
}
