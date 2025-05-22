using Microsoft.AspNetCore.Mvc;
using UniversityBusinessLogic.MailWorker;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.BusinessLogicsContracts;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;

namespace UniversityRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PlanOfStudysController : Controller
    {
        private readonly ILogger _logger;
        private readonly IPlanOfStudyBusinessLogicContract _logic;
        private readonly IReportContract _reportLogic;
        private readonly AbstractMailWorker _mailWorker;
        public PlanOfStudysController(IPlanOfStudyBusinessLogicContract logic, ILogger<PlanOfStudysController> logger, IReportContract reportLogic, AbstractMailWorker mailWorker)
        {
            _logic = logic;
            _logger = logger;
            _reportLogic = reportLogic;
            _mailWorker = mailWorker;
        }
        [HttpGet]
        public List<PlanOfStudyViewModel>? GetPlanOfStudys(int userId)
        {
            try
            {
                return _logic.ReadList(new PlanOfStudySearchModel { UserId = userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка планов обучения");
                throw;
            }
        }
		[HttpGet]
		public PlanOfStudyViewModel? GetPlanOfStudy(int id, int userId)
		{
			try
			{
				return _logic.ReadElement(new PlanOfStudySearchModel { Id = id, UserId = userId });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка получения списка планов обучения");
				throw;
			}
		}
        [HttpGet]
        public List<ReportPlanOfStudyViewModel>? GetPlanOfStudyAndDisciplines(int userId)
        {
            try
            {
                return _reportLogic.GetPlanOfStudyAndDisciplines(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка планов обучения");
                throw;
            }
        }
        [HttpGet]
        public List<ReportPlanOfStudyAndStudentViewModel>? GetPlanOfStudyAndStudents()
        {
            try
            {
                var report = _reportLogic.GetPlanOfStudyAndStudents();
                return report;
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
                _reportLogic.SavePlanOfStudyToWord(model);
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
                _reportLogic.SavePlanOfStudyToExcel(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
        }
        [HttpPost]
        public void CreateReportToPDFFile(ReportBindingModel model)
        {
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                _reportLogic.SendPlanOfStudyToEmail(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
        }
        [HttpPost]
        public void SendPDFToMail(MailSendInfoBindingModel model)
        {
            try
            {
                _mailWorker.MailSendAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка отправки письма");
                throw;
            }
        }
        [HttpPost]
        public void CreatePlanOfStudy(PlanOfStudyBindingModel model)
        {
            try
            {
                _logic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания плана обучения");
                throw;
            }
        }
        [HttpPost]
        public void UpdatePlanOfStudy(PlanOfStudyBindingModel model)
        {
            try
            {
                _logic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления плана обучения");
                throw;
            }
        }
        [HttpPost]
        public void DeletePlanOfStudy(PlanOfStudyBindingModel model)
        {
            try
            {
                _logic.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления плана обучения");
                throw;
            }
        }
	}
}
