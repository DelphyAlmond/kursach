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
    public class UserBusinessLogicContract : IUserBusinessLogicContract
    {
        private readonly ILogger _logger;
        private readonly IUserStorageContract _userStorage;

        public UserBusinessLogicContract(ILogger<IUserBusinessLogicContract> logger, IUserStorageContract userStorage)
        {
            _logger = logger;
            _userStorage = userStorage;
        }
        public List<UserViewModel>? ReadList(UserSearchModel? model)
        {
            _logger.LogInformation("User ReadList. Login:{Login}  Emain:{Email}  Id:{Id}", model?.Login, model?.Email, model?.Id);
            var list = model == null ? _userStorage.GetFullList() : _userStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }

        public UserViewModel? ReadElement(UserSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. Login:{Login}. Email:{Email}.  Id:{Id}", model?.Login, model?.Email, model?.Id);
            var element = _userStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }

        public bool Create(UserBindingModel model)
        {
            CheckModel(model);
            if (_userStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public bool Update(UserBindingModel model)
        {
            CheckModel(model);
            if (_userStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        public bool Delete(UserBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_userStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }
        private void CheckModel(UserBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.Login))
            {
                throw new ArgumentNullException("Нет логина пользователя", nameof(model.Login));
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                throw new ArgumentNullException("Нет пароля пользователя", nameof(model.Password));
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                throw new ArgumentNullException("Нет почты пользователя", nameof(model.Email));
            }
            var user1 = _userStorage.GetElement(new UserSearchModel
            {
                Login = model.Login
            });

            if (user1 != null && user1.Id != model.Id)
            {
                throw new InvalidOperationException("Пользователь с таким логином уже есть");
            }
            var user2 = _userStorage.GetElement(new UserSearchModel
            {
                Email = model.Email
            });

            if (user2 != null && user2.Id != model.Id)
            {
                throw new InvalidOperationException("Пользователь с такой почтой уже есть");
            }
        }
    }
}
