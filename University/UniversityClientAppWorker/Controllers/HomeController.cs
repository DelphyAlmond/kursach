using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UniversityClientAppWorker.Models;
using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;
using UniversityDataModels.Enums;
using UniversityDataModels.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UniversityClientAppWorker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Teachers = APIClient.GetRequest<List<TeacherViewModel>>($"api/teacher/getallteachers");
            var planOfStudys = await APIClient.GetRequestPlanOfStudyAsync<List<PlanOfStudyViewModel>>($"api/planofstudys/getplanofstudys?userId={APIClient.User.Id}");

            return View(planOfStudys);
        }
		[HttpGet]
		public async Task<IActionResult> InfoPlanOfStudy(int id)
		{
			if (APIClient.User == null)
			{
				return Redirect("~/Home/Enter");
			}
            var obj1 = APIClient.GetRequest<List<TeacherViewModel>>($"api/teacher/getallteachers");
			ViewBag.Teachers = obj1;

			var obj = await APIClient.GetRequestPlanOfStudyAsync<PlanOfStudyViewModel>($"api/planofstudys/getplanofstudy?id={id}&userId={APIClient.User.Id}");
			return View(obj);
		}
		[HttpPost]
        public void CreatePlanOfStudy(string profile, string formOfStudy, List<int> teacherIds)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вход только авторизованным пользователям");
            }
            if (string.IsNullOrEmpty(formOfStudy) || string.IsNullOrEmpty(profile))
            {
                throw new Exception("Введите данные профиля и формы обучения");
            }
            APIClient.PostRequest("api/planofstudys/createplanofstudy", new PlanOfStudyBindingModel
            {
                Profile = profile,
                FormOfStudy = formOfStudy,
                UserId = APIClient.User.Id,
				PlanOfStudyTeachers = teacherIds.ToDictionary(id => id, id => (ITeacherModel)null)
			});
            Response.Redirect("Index");
        }
        [HttpPost]
        public void DeletePlanOfStudy(int id)
        {
            if (id == 0)
            {
                throw new Exception("id не может быть равен 0");
            }
            APIClient.PostRequest("api/planofstudys/deleteplanofstudy", new PlanOfStudyBindingModel
            {
                Id = id
            });
            Response.Redirect("Index");
        }
		[HttpPost]
        public void UpdatePlanOfStudy(int id, string profile, string formOfStudy, List<int> teacherIds)
        {
            if (id == 0)
            {
                throw new Exception("id не может быть равен 0");
            }
            var planOfStudyTeachers = teacherIds.ToDictionary(id => id, id => (ITeacherModel)null);
            APIClient.PostRequest("api/planofstudys/updateplanofstudy", new PlanOfStudyBindingModel
            {
                Id = id,
                Profile = profile,
                FormOfStudy = formOfStudy,
                PlanOfStudyTeachers = planOfStudyTeachers
            });
            Response.Redirect("Index");
        }
		[HttpGet]
		public IActionResult Privacy()
        {
			if (APIClient.User == null)
			{
				return Redirect("~/Home/Enter");
			}
			return View(APIClient.User);
        }
        [HttpPost]
        public void Privacy(string login, string password, string email)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вход только авторизованным пользователям");
            }
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                throw new Exception("Введите логин, пароль и почту");
            }
            APIClient.PostRequest("api/user/updatedata", new UserViewModel
            {
                Id = APIClient.User.Id,
                Email = email,
                Login = login,
                Password = password
            });

            APIClient.User.Login = login;
            APIClient.User.Email = email;
            APIClient.User.Password = password;
            Response.Redirect("Index");
        }
        [HttpGet]
		public IActionResult Attestations()
        {
			if (APIClient.User == null)
			{
				return Redirect("~/Home/Enter");
			}
            ViewBag.Students = APIClient.GetRequest<List<StudentViewModel>>($"api/student/getstudents?userId={APIClient.User.Id}");
            ViewBag.AttestationScore = Enum.GetValues(typeof(AttestationScore)).Cast<AttestationScore>();
            var obj = APIClient.GetRequest<List<AttestationViewModel>>($"api/attestation/getattestations?userId={APIClient.User.Id}");
            return View(obj);
        }
        [HttpPost]
        public void CreateAttestation(string formOfEvaluation, int student, AttestationScore score)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вход только авторизованным");
            }
            if (string.IsNullOrEmpty(formOfEvaluation) || student == 0)
            {
                throw new Exception("Введите форму оценивания и выберите студента");
            }
            var Student = APIClient.GetRequest<StudentViewModel>($"api/student/getstudent?userId={APIClient.User.Id}&studentId={student}");

            if(Student == null)
            {
                throw new Exception("Студент не найден");
            }
            APIClient.PostRequest("api/attestation/createattestation", new AttestationBindingModel
            {
                UserId = APIClient.User.Id,
                FormOfEvaluation = formOfEvaluation,
                StudentId = student,
                StudentName = Student.Name,
                Score = score
            });
            Response.Redirect("Attestations");
        }
        [HttpGet]
        public IActionResult InfoAttestation(int id)
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Students = APIClient.GetRequest<List<StudentViewModel>>($"api/student/getstudents?userId={APIClient.User.Id}");
            ViewBag.AttestationScore = Enum.GetValues(typeof(AttestationScore)).Cast<AttestationScore>();
            var obj = APIClient.GetRequest<AttestationViewModel>($"api/attestation/getattestation?userId={APIClient.User.Id}&id={id}");
            return View(obj);
        }
        [HttpPost]
        public void UpdateAttestation(int id, string formOfEvaluation, int student, AttestationScore score)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вход только авторизованным");
            }
            if (string.IsNullOrEmpty(formOfEvaluation) || student == 0)
            {
                throw new Exception("Введите форму оценивания и выберите студента");
            }
            var Student = APIClient.GetRequest<StudentViewModel>($"api/student/getstudent?userId={APIClient.User.Id}&studentId={student}");

            if (Student == null)
            {
                throw new Exception("Студент не найден");
            }
            APIClient.PostRequest("api/attestation/updateattestation", new AttestationBindingModel
            {
                Id = id,
                FormOfEvaluation = formOfEvaluation,
                StudentId = student,
                StudentName = Student.Name,
                Score = score
            });
            Response.Redirect("Attestations");
        }
        [HttpPost]
        public void DeleteAttestation(int id)
        {
            if (id == 0)
            {
                throw new Exception("id не может быть равен 0");
            }
            APIClient.PostRequest("api/attestation/deleteattestation", new PlanOfStudyBindingModel
            {
                Id = id
            });
            Response.Redirect("Attestations");
        }
        [HttpGet]
		public async Task<IActionResult> Students()
        {
			if (APIClient.User == null)
			{
				return Redirect("~/Home/Enter");
			}
            var planOfStudys = await APIClient.GetRequestPlanOfStudyAsync<List<PlanOfStudyViewModel>>
                ($"api/planofstudys/getplanofstudys?userId={APIClient.User.Id}");
            ViewBag.PlanOfStudys = planOfStudys;
            return View(APIClient.GetRequest<List<StudentViewModel>>($"api/student/getstudents?userId={APIClient.User.Id}"));
        }
        [HttpPost]
        public async Task<IActionResult> CreateStudent(string name, int planOfStudy, string phoneNumber)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вход только авторизованным");
            }
            if (string.IsNullOrEmpty(name) || planOfStudy == 0 || string.IsNullOrEmpty(phoneNumber))
            {
                throw new Exception("Введите ФИО, план обучения и телефон");
            }
            var PlanOfStudy = await APIClient.GetRequestPlanOfStudyAsync<PlanOfStudyViewModel>($"api/planofstudys/getplanofstudy?id={planOfStudy}");
            if(PlanOfStudy == null)
            {
                throw new Exception("План обучения не найден");
            }
            APIClient.PostRequest("api/student/createstudent", new StudentBindingModel
            {
                UserId = APIClient.User.Id,
                Name = name,
                PlanOfStudyId = planOfStudy,
                PlanOfStudyProfile = PlanOfStudy.Profile,
                PhoneNumber = phoneNumber,
                
            });
            return Redirect("/Home/Students");
        }
        [HttpGet]
        public async Task<IActionResult> InfoStudent(int id)
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            if(id == 0)
            {
                throw new Exception("id не может быть 0");
            }
            var planOfStudys = await APIClient.GetRequestPlanOfStudyAsync<List<PlanOfStudyViewModel>>
                ($"api/planofstudys/getplanofstudys?userId={APIClient.User.Id}");
            ViewBag.PlanOfStudys = planOfStudys;
            var obj = APIClient.GetRequest<StudentViewModel>($"api/student/getstudent?userId={APIClient.User.Id}&studentId={id}");
            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStudent(int id, string name, int planOfStudy, string phoneNumber)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вход только авторизованным");
            }
            if(id == 0)
            {
                throw new Exception("id не может быть 0");
            }
            if (string.IsNullOrEmpty(name) || planOfStudy == 0 || string.IsNullOrEmpty(phoneNumber))
            {
                throw new Exception("Введите ФИО, план обучения и телефон");
            }
            var PlanOfStudy = await APIClient.GetRequestPlanOfStudyAsync<PlanOfStudyViewModel>($"api/planofstudys/getplanofstudy?id={planOfStudy}");
            if (PlanOfStudy == null)
            {
                throw new Exception("План обучения не найден");
            }
            APIClient.PostRequest("api/student/updatestudent", new StudentBindingModel
            {
                Id = id,
                Name = name,
                PlanOfStudyId = planOfStudy,
                PlanOfStudyProfile = PlanOfStudy.Profile,
                PhoneNumber = phoneNumber,
            });
            return Redirect("/Home/Students");
        }
        [HttpPost]
        public void DeleteStudent(int id)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вход только авторизованным");
            }
            if (id == 0)
            {
                throw new Exception("id не может быть 0");
            }
            APIClient.PostRequest("api/student/deletestudent", new StudentBindingModel
            {
                Id = id
            });
            Response.Redirect("Students");
        }
        [HttpGet]
		public IActionResult Enter()
        {
            return View();
        }
		[HttpGet]
		public IActionResult Register()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
		[HttpPost]
		public void Enter(string login, string password)
		{
			if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
			{
				throw new Exception("Введите логин и пароль");
			}
			APIClient.User = APIClient.GetRequest<UserViewModel>($"api/user/loginworker?login={login}&password={password}");
			if (APIClient.User == null)
			{
				throw new Exception("Неверный логин/пароль");
			}
			Response.Redirect("Index");
		}
		[HttpPost]
        public void Register(string login, string password, string email)
        {
			if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
			{
				throw new Exception("Введите логин, пароль и почту");
			}
			APIClient.PostRequest("api/user/registerworker", new UserBindingModel
			{
				Email = email,
				Login = login,
				Password = password
			});
			Response.Redirect("Enter");
			return;
		}
        [HttpGet] // ЭТО ОТЧЕТЫ КОТОРЫЕ БЕРУТСЯ ИЗ ПАПКИ (МОЖНО БРАТЬ УЖЕ ПРЕДВАРИТЕЛЬНО ГОТОВЫЕ)
        public IActionResult GetWordFile()
        {
            return PhysicalFile($"C:\\Users\\Имя\\Desktop\\Отчёты\\Планы обучений по дисциплинам1.docx", // ВОТ ЗДЕСЬ МЕНЯЕШЬ НА СВОЙ ФАЙЛ (который создала заранее (чтобы он открывался на сайте))
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "Планы обучений по дисциплинам и аттестациям.docx");
        }
        public IActionResult GetExcelFile()
        {
            return PhysicalFile($"C:\\Users\\Имя\\Desktop\\Отчёты\\Планы обучений по дисциплинам1.xlsx", // ВОТ ЗДЕСЬ МЕНЯЕШЬ НА СВОЙ ФАЙЛ (который создала заранее (чтобы он открывался на сайте))
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Планы обучений по дисциплинам и аттестациям.xlsx");
        }
        //public IActionResult GetTeachersFile()
        //{
        //    return PhysicalFile($"C:\\Users\\Имя\\Desktop\\Отчёты\\TeachersAndStudents1.xlsx",
        //        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //        "Преподаватели по студентам.xlsx");
        //}
        public IActionResult GetTeachersWordFile()
        {
            return PhysicalFile($"C:\\Users\\Имя\\Desktop\\Отчёты\\TeachersAndStudents.xlsx", // ВОТ ЗДЕСЬ МЕНЯЕШЬ НА СВОЙ ФАЙЛ (который создала заранее (чтобы он открывался на сайте))
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "TeachersAndStudents.docx");
        }
        public IActionResult GetTeachersExcelFile()
        {
            return PhysicalFile($"C:\\Users\\Имя\\Desktop\\Отчёты\\TeachersAndStudents.xlsx", // ВОТ ЗДЕСЬ МЕНЯЕШЬ НА СВОЙ ФАЙЛ (который создала заранее (чтобы он открывался на сайте))
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "TeachersAndStudents.xlsx");
        }
        //[HttpGet]
        //public IActionResult ReportTeachers()
        //{
        //    if (APIClient.User == null)
        //    {
        //        return Redirect("~/Home/Enter");
        //    }
        //    return GetTeachersFile();
        //}
        [HttpGet]
        public IActionResult ReportTeachers() {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View("ReportTeacher", APIClient.GetRequest<List<ReportTeacherViewModel>>($"api/teacher/getteachersreport?userId={APIClient.User.Id}"));
        }

        [HttpPost]
        public IActionResult ReportTeachers(string type)
        {
            if (APIClient.User == null)
            {
                Redirect("~/Home/Enter");
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new Exception("Error, wrong type");
            }

            if (type == "docx")
            {
                APIClient.PostRequest("api/teacher/loadreporttoword", new ReportBindingModel
                {
                    FileName = $"C:\\Users\\Имя\\Desktop\\Отчёты\\TeachersAndStudents.docx" // ЭТО ОТЧЁТЫ КОТОРЫЕ СОЗДАЕТ САМА ПРОГРАММА
                });
				return GetWordFile();
			}

            if (type == "xlsx")
            {
                APIClient.PostRequest("api/teacher/loadreporttoexcel", new ReportBindingModel
                {
					FileName = $"C:\\Users\\Имя\\Desktop\\Отчёты\\TeachersAndStudents.xlsx" // ЭТО ОТЧЁТЫ КОТОРЫЕ СОЗДАЕТ САМА ПРОГРАММА
                });
				return GetExcelFile();
			}
			return Redirect("ReportTeacher");
		}
        [HttpGet]
        public IActionResult ReportPlanOfStudys()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View("ReportPlanOfStudys", APIClient.GetRequest<List<ReportPlanOfStudyViewModel>>($"api/planofstudys/getplanofstudyanddisciplines?userId={APIClient.User.Id}"));
        }
        [HttpPost]
        public IActionResult ReportPlanOfStudys(string type)
        {
            if (APIClient.User == null)
            {
                Redirect("~/Home/Enter");
                throw new Exception("Вход только авторизованным");
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new Exception("Неверный тип отчета");
            }

            if (type == "docx")
            {
                APIClient.PostRequest("api/planofstudys/loadreporttoword", new ReportBindingModel
                {
                    FileName = "C:\\Users\\Имя\\Desktop\\Отчёты\\Планы обучений по дисциплинам.docx"
                });
                return GetWordFile();
            }

            if (type == "xlsx")
            {
                APIClient.PostRequest("api/planofstudys/loadreporttoexcel", new ReportBindingModel
                {
                    FileName = "C:\\Users\\Имя\\Desktop\\Отчёты\\Планы обучений по дисциплинам.xlsx"
                });
                return GetExcelFile();
            }
            return Redirect("Index");
        }
        [HttpGet]
        public IActionResult ReportPlanOfStudyAndStudents()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View("ReportPlanOfStudyAndStudents", APIClient.GetRequest<List<ReportPlanOfStudyAndStudentViewModel>>($"api/planofstudys/getplanofstudyandstudents"));
        }
        [HttpPost]
        public void ReportPlanOfStudyAndStudents(string type)
        {
            if (APIClient.User == null)
            {
                Redirect("~/Home/Enter");
                throw new Exception("Вход только авторизованным");
            }
            if (type == "pdf")
            {
                APIClient.PostRequest("api/planofstudys/createreporttopdffile", new ReportBindingModel
                {
                    FileName = "C:\\Users\\Имя\\Desktop\\Отчёты\\Сведения по планам обучения.pdf"
                });
                APIClient.PostRequest("api/planofstudys/sendpdftomail", new MailSendInfoBindingModel
                {
                    MailAddress = "crazyandroed@gmail.com", // МЕНЯЕШЬ НА СВОЮ ПОЧТУ, КУДА НУЖНО ОТПРАВИТЬ
                    Subject = "Отчет",
                    Text = "Сведения по планам обучения"
                });
            }
            Response.Redirect("Index");
            return;
        }
    }
}
