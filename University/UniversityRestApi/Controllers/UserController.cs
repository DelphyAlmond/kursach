using Microsoft.AspNetCore.Mvc;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicsContracts;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;
using UniversityDataModels.Enums;

namespace UniversityRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUserBusinessLogicContract _logic;

        public UserController(IUserBusinessLogicContract logic, ILogger<UserController> logger)
        {
            _logic = logic;
            _logger = logger;
        }

        [HttpGet]
        public UserViewModel? LoginWorker(string login, string password)
        {
            try
            {
                return _logic.ReadElement(new UserSearchModel
                {
                    Login = login,
                    Password = password,
                    Role = UserRole.Работник
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка входа в систему");
                throw;
            }
        }

        [HttpGet]
        public UserViewModel? LoginStorekeeper(string login, string password)
        {
            try
            {

                //
                var x = _logic.ReadElement(new UserSearchModel
				{
					Login = login,
					Password = password,
					Role = UserRole.Кладовщик
				});
                //
               
				return _logic.ReadElement(new UserSearchModel
                {
                    Login = login,
                    Password = password,
                    Role = UserRole.Кладовщик
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка входа в систему");
                throw;
            }
        }
        [HttpGet]
        public List<UserViewModel>? GetAll()
        {
            try
            {
                return _logic.ReadList(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка");
                throw;
            }
        }
        [HttpPost]
        public void RegisterWorker(UserBindingModel model)
        {
            try
            {
                model.Role = UserRole.Работник;
                _logic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка регистрации");
                throw;
            }
        }

        [HttpPost]
        public void RegisterStorekeeper(UserBindingModel model)
        {
            try
            {
                model.Role = UserRole.Кладовщик;
                _logic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка регистрации");
                throw;
            }
        }

        [HttpPost]
        public void UpdateData(UserBindingModel model)
        {
            try
            {
                _logic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных");
                throw;
            }
        }
    }
}
