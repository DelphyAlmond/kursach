using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.Metadata;
using UniversityClientApp.Models;
using UniversityClientAppStorekeeper;
using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;
using UniversityDatabaseImplement;
using UniversityDataModels.Enums;
using UniversityDataModels.Models;

namespace UniversityClientApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
			if (APIStorekeeper.Client == null)
			{
				return Redirect("~/Home/Enter");
			}
			return View();
		}

        public IActionResult Privacy()
        {
            return View();
        }

		[HttpGet]
		public IActionResult Enter()
		{
			return View();
		}
		[HttpPost]
		public void Enter(string login, string password)
		{
			if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
			{
				throw new Exception("Введите логин и пароль");
			}
			APIStorekeeper.Client = APIStorekeeper.GetRequest<UserViewModel>($"api/user/loginstorekeeper?login={login}&password={password}");
			if (APIStorekeeper.Client == null)
			{
				throw new Exception("Неверный логин/пароль");
			}
			Response.Redirect("Index");
		}
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public void Register(string login, string password, string email)
		{
			if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
			{
				throw new Exception("Введите логин, пароль и ФИО");
			}
			APIStorekeeper.PostRequest("api/user/registerstorekeeper", new UserBindingModel
			{
				Login = login,
				Email = email,
				Password = password,
				Role = UserRole.Кладовщик
			});
			Response.Redirect("Enter");
			return;
		}

        [HttpGet]
        public async Task<IActionResult> Disciplines()
        {
            if (APIStorekeeper.Client == null)
            {
                return Redirect("~/Home/Enter");
            }

            ViewBag.Teachers = APIStorekeeper.GetRequest<List<TeacherViewModel>>($"api/teacher/getteachers?userId={APIStorekeeper.Client.Id}");
            ViewBag.Students = APIStorekeeper.GetRequest<List<StudentViewModel>>($"api/student/getstudents?userId={APIStorekeeper.Client.Id}");

            // Ожидаем завершения асинхронной операции
            var disciplines = await APIStorekeeper.GetRequestDisciplineAsync<List<DisciplineViewModel>>($"api/discipline/getdisciplines");

            // Теперь мы можем передать результат в представление
            return View(disciplines);
        }
        [HttpPost]
        public void Disciplines(string name, string description, DateOnly date, int teacher, List<int> studentIds)
        {
            if (APIStorekeeper.Client == null)
            {
                Redirect("~/Home/Enter");
            }

            var disciplineModel = new DisciplineBindingModel
            {
                UserId = APIStorekeeper.Client.Id,
                Name = name,
                Description = description,
                Date = date,
                TeacherId = teacher,
                StudentDisciplines = studentIds.ToDictionary(id => id, id => (IStudentModel)null)
            };

            APIStorekeeper.PostRequest("api/discipline/creatediscipline", disciplineModel);

            Response.Redirect("Disciplines");
        }



        [HttpGet]
        public IActionResult Statements()
        {
            if (APIStorekeeper.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Teachers = APIStorekeeper.GetRequest<List<TeacherViewModel>>($"api/teacher/getteachers?userId={APIStorekeeper.Client.Id}");
            return View(APIStorekeeper.GetRequest<List<StatementViewModel>>($"api/statement/getstatements?teacherId={0}"));
        }
        [HttpPost]
        public void Statements(string name, DateTime date, int teacher)
        {
            if (APIStorekeeper.Client == null)
            {
                Redirect("~/Home/Enter");
            }
            APIStorekeeper.PostRequest("api/statement/createstatement", new StatementBindingModel
            {
                UserId = APIStorekeeper.Client.Id,
                Name = name,
                Date = date,
                TeacherId = teacher,
            });
            Response.Redirect("Statements");
        }

        [HttpGet]
        public IActionResult Teachers()
		{
            if (APIStorekeeper.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIStorekeeper.GetRequest<List<TeacherViewModel>>($"api/teacher/getteachers?userId={APIStorekeeper.Client.Id}"));
        }
        [HttpPost]
        public void Teachers(string name, string position, string academicDegree)
        {
            if (APIStorekeeper.Client == null)
            {
                Redirect("~/Home/Enter");
            }
            APIStorekeeper.PostRequest("api/teacher/createteacher", new TeacherBindingModel
            {
                UserId = APIStorekeeper.Client.Id,
                Name = name,
                Position = position,
                AcademicDegree = academicDegree
            });
            Response.Redirect("Teachers");
        }
        [HttpGet]
        public IActionResult Report() {
            if (APIStorekeeper.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View("Report", APIStorekeeper.GetRequest<List<ReportTeacherViewModel>>($"api/teacher/getteachersreport?userId={APIStorekeeper.Client.Id}"));
        }

        [HttpPost]
        public IActionResult Report(string type)
        {
            if (APIStorekeeper.Client == null)
            {
                Redirect("~/Home/Enter");
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new Exception("Error, wrong type");
            }

            if (type == "docx")
            {
                APIStorekeeper.PostRequest("api/teacher/loadreporttoword", new ReportBindingModel
                {
                    FileName = $"C:\\Users\\Имя\\Desktop\\Отчёты\\TeachersAndStudents.docx"
                });
				return GetWordFile();
			}

            if (type == "xlsx")
            {
                APIStorekeeper.PostRequest("api/teacher/loadreporttoexcel", new ReportBindingModel
                {
					FileName = $"C:\\Users\\Имя\\Desktop\\Отчёты\\TeachersAndStudents.xlsx"
				});
				return GetExcelFile();
			}
			return Redirect("Report");
		}

        [HttpPost]
        public void ReportDisciplines(string type, DateOnly dateFrom, DateOnly dateTo)
        {
            if (APIStorekeeper.Client == null)
            {
                Redirect("~/Home/Enter");
                throw new Exception("Вход только авторизованным");
            }
            if (dateFrom == DateOnly.MinValue || dateTo == DateOnly.MaxValue)
            {
                throw new Exception();
            }
            if (type == "pdf")
            {
                APIStorekeeper.PostRequest($"api/discipline/createreporttopdffile?dateFrom={dateFrom:yyyy-MM-dd}&dateTo={dateTo:yyyy-MM-dd}", new ReportDateRangeBindingModel
                {
                    FileName = "C:\\Users\\Имя\\Desktop\\Отчёты\\Сведения по планам обучения.pdf"
                });
                APIStorekeeper.PostRequest("api/discipline/sendpdftomail", new MailSendInfoBindingModel
                {
                    MailAddress = APIStorekeeper.Client.Email,
                    Subject = "Отчет",
                    Text = "Сведения по дисциплинам"
                });
            }
            Response.Redirect("Index");
            return;
        }

        [HttpGet]
        public IActionResult ReportDisciplines(DateOnly dateFrom, DateOnly dateTo)
        {
            if (APIStorekeeper.Client == null)
            {
                return Redirect("~/Home/Enter");
            }

            // Передаем данные в частичное представление
            if (dateFrom == DateOnly.MinValue || dateTo == DateOnly.MaxValue)
            {
                return View("ReportDisciplines", null);
            }
            var reportData = APIStorekeeper.GetRequest<List<ReportDisciplineViewModel>>($"api/discipline/getreportdisciplines?datefrom={dateFrom:yyyy-MM-dd}&dateto={dateTo:yyyy-MM-dd}");

            return View("ReportDisciplines", reportData);
        }


		public IActionResult GetWordFile()
		{
			return PhysicalFile($"C:\\Users\\Имя\\Desktop\\Отчёты\\TeachersAndStudents.xlsx",
				"application/vnd.openxmlformats-officedocument.wordprocessingml.document",
				"TeachersAndStudents.docx");
		}
		public IActionResult GetExcelFile()
		{
			return PhysicalFile($"C:\\Users\\Имя\\Desktop\\Отчёты\\TeachersAndStudents.xlsx",
				"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
				"TeachersAndStudents.xlsx");
		}


        // UDS


        // Преподаватель

        [HttpPost]
        public void DeleteTeacher(int id)
        {
            if (id == 0)
            {
                throw new Exception("id не может быть равен 0");
            }
            APIStorekeeper.PostRequest("api/teacher/deleteteacher", new TeacherBindingModel
            {
                Id = id
            });
            Response.Redirect("Teachers");
        }

        [HttpGet]
        public IActionResult InfoTeacher(int id)
        {
            if (APIStorekeeper.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            var obj = APIStorekeeper.GetRequest<TeacherViewModel>($"api/teacher/getteacher?userId={APIStorekeeper.Client.Id}&id={id}");
            return View(obj);
        }

        [HttpPost]
        public void UpdateTeacher(int id, string name, string academicdegree, string position)
        {
            if (APIStorekeeper.Client == null)
            {
                throw new Exception("Вход только авторизованным");
            }
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(academicdegree) || string.IsNullOrEmpty(position))
            {
                throw new Exception("Введите");
            }

            APIStorekeeper.PostRequest("api/teacher/updateteacher", new TeacherBindingModel
            {
                Id = id,
                Name = name,
                AcademicDegree = academicdegree,
                Position = position
            });
            Response.Redirect("Teachers");
        }


        // Ведомость

        [HttpPost]
        public void DeleteStatement(int id)
        {
            if (id == 0)
            {
                throw new Exception("id не может быть равен 0");
            }
            APIStorekeeper.PostRequest("api/statement/deletestatement", new StatementBindingModel
            {
                Id = id
            });
            Response.Redirect("Statements");
        }

        [HttpGet]
        public IActionResult InfoStatement(int id)
        {
            if (APIStorekeeper.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Teachers = APIStorekeeper.GetRequest<List<TeacherViewModel>>($"api/teacher/getteachers?userId={APIStorekeeper.Client.Id}");
            var obj = APIStorekeeper.GetRequest<StatementViewModel>($"api/statement/getstatement?userId={APIStorekeeper.Client.Id}&id={id}");
            return View(obj);
        }

        [HttpPost]
        public void UpdateStatement(int id, string name, DateTime date, int teacher)
        {
            if (APIStorekeeper.Client == null)
            {
                throw new Exception("Вход только авторизованным");
            }
            if (string.IsNullOrEmpty(name) || teacher == 0)
            {
                throw new Exception("Введите");
            }

            APIStorekeeper.PostRequest("api/statement/updatestatement", new StatementBindingModel
            {
                Id = id,
                Name = name,
                Date = date,
                TeacherId = teacher
            });
            Response.Redirect("Statements");
        }


        // Дисциплина

        [HttpGet]
        public async Task<IActionResult> InfoDiscipline(int id)
        {
            if (APIStorekeeper.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            var obj1 = APIStorekeeper.GetRequest<List<StudentViewModel>>($"api/student/getallstudents");
            ViewBag.Students = obj1;
            var obj2 = APIStorekeeper.GetRequest<List<TeacherViewModel>>($"api/teacher/getteachers?userId={APIStorekeeper.Client.Id}");
            ViewBag.Teachers = obj2;
            var obj = await APIStorekeeper.GetRequestDisciplineAsync<DisciplineViewModel>($"api/discipline/getdiscipline?id={id}&userId={APIStorekeeper.Client.Id}");
            return View(obj);
        }

        [HttpPost]
        public void DeleteDiscipline(int id)
        {
            if (id == 0)
            {
                throw new Exception("id не может быть равен 0");
            }
            APIStorekeeper.PostRequest("api/discipline/deletediscipline", new DisciplineBindingModel
            {
                Id = id
            });
            Response.Redirect("Disciplines");
        }
        [HttpPost]
        public void UpdateDiscipline(int id, string name, string description, int teacher, List<int> studentIds, DateOnly date)
        {
            if (id == 0)
            {
                throw new Exception("id не может быть равен 0");
            }
            var disciplineStudents = studentIds.ToDictionary(id => id, id => (IStudentModel)null);
            APIStorekeeper.PostRequest("api/discipline/updatediscipline", new DisciplineBindingModel
            {
                Id = id,
                Name = name,
                Description = description,
                TeacherId = teacher,
                StudentDisciplines = disciplineStudents,
                Date = date
            });
            Response.Redirect("Disciplines");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
