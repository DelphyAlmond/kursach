using UniversityBusinessLogic.OfficePackage;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.SearchModels;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using DocumentFormat.OpenXml.EMMA;
using UniversityBusinessLogic.OfficePackage.HelperModels;
using System.Reflection;

namespace UniversityBusinessLogics.BusinessLogics;

public class ReportContract : IReportContract
{

    /*private readonly AbstractSaveToWord _saveToWord;
    private readonly AbstractSaveToExcel _saveToExcel;
    private readonly AbstractSaveToPdf _saveToPdf;*/

    private readonly IAttestationStorage _attestationStorage;
    private readonly ITeacherStorageContract _teacherStorage;
    private readonly IDisciplineStorageContract _disciplineStorage;
    private readonly IStudentStorageContract _studentStorage;
    private readonly IStatementStorageContract _statementStorage;
    private readonly IPlanOfStudyStorageContract _planOfStudyStorage;

    private readonly AbstractSaveToExcelWorker _saveToExcelWorker;
    private readonly AbstractSaveToWordWorker _saveToWordWorker;
    private readonly AbstractSaveToPdfWorker _saveToPdfWorker;

    private readonly AbstractSaveToExcelStorekeeper _saveToExcelStorekeeper;
    private readonly AbstractSaveToPdfStorekeeper _saveToPdfStorekeeper;
    private readonly AbstractSaveToWordStorekeeper _saveToWordStorekeeper;
    public ReportContract (IAttestationStorage attestationStorage, ITeacherStorageContract teacherStorage, IDisciplineStorageContract
	   disciplineStorage, IStudentStorageContract studentStorage, IStatementStorageContract statementStorage,
        IPlanOfStudyStorageContract planOfStudyStorage, AbstractSaveToExcelWorker saveToExcelWorker, AbstractSaveToWordWorker saveToWordWorker
        ,AbstractSaveToPdfWorker saveToPdfWorker, AbstractSaveToWordStorekeeper saveToWordStorekeeper,
        AbstractSaveToExcelStorekeeper saveToExcelStorekeeper, AbstractSaveToPdfStorekeeper saveToPdfStorekeeper)
        {
        _attestationStorage = attestationStorage;
		_teacherStorage = teacherStorage;
		_disciplineStorage = disciplineStorage;
        _studentStorage = studentStorage;
        _statementStorage = statementStorage;
        _planOfStudyStorage = planOfStudyStorage;

		_saveToExcelWorker = saveToExcelWorker;
        _saveToWordWorker = saveToWordWorker;
        _saveToPdfWorker = saveToPdfWorker;

        _saveToWordStorekeeper = saveToWordStorekeeper;
        _saveToExcelStorekeeper = saveToExcelStorekeeper;
        _saveToPdfStorekeeper = saveToPdfStorekeeper;
        }
    public List<ReportTeacherViewModel> GetTeachers(int userId)
    {
        var teachers = _teacherStorage.GetFilteredList(new TeacherSearchModel
        {
            UserId = userId
        });

        // Создаем список для результатов
        var result = new List<ReportTeacherViewModel>();

        foreach (var teacher in teachers)
        {
            // Получаем список дисциплин, связанных с учителем
            var disciplines = _disciplineStorage.GetFilteredList(new DisciplineSearchModel
            {
                TeacherId = teacher.Id,
            });

            // Получаем список студентов, связанных с дисциплинами
            var students = new List<string>();
            foreach (var discipline in disciplines)
            {
                var studentDisciplines = _disciplineStorage.GetStudentsForDiscipline(new DisciplineSearchModel
                {
                    Id = discipline.Id,
                });

                foreach (var studentDiscipline in studentDisciplines)
                {
                    var studentList = _studentStorage.GetFilteredList(new StudentSearchModel
                    {
                        Id = studentDiscipline.Id,
                    });
                    foreach(var st in studentList){
                        students.Add(st.Name + " " + st.PhoneNumber);
                    }
                }
            }

            // Добавляем учителя и его студентов в результат
            result.Add(new ReportTeacherViewModel
            {
                TeacherId = teacher.Id,
                TeacherName = teacher.Name,
                Students = students.Distinct().ToList() // Убираем дубликаты, если они есть
            });
        }

        return result;
    }

    public List<ReportDisciplineViewModel> GetDisciplines(ReportDateRangeBindingModel model)
    {
        var disciplines = _disciplineStorage.GetFilteredList(new DisciplineSearchModel { DateFrom = model.DateFrom, DateTo = model.DateTo});

        var reportDisciplineViewModels = new List<ReportDisciplineViewModel>();

        foreach (var discipline in disciplines)
        {
            // Получаем список студентов, связанных с дисциплинами
            var studentDisciplines = _disciplineStorage.GetStudentsForDiscipline(new DisciplineSearchModel
            {
                Id = discipline.Id,
            });

            var planOfStudys = new List<string>();
            foreach (var studentDiscipline in studentDisciplines)
            {
                var student = _studentStorage.GetElement(new StudentSearchModel
                {
                    Id = studentDiscipline.Id,
                });

                if (student != null)
                {
                    var planOfStudy = _planOfStudyStorage.GetElement(new PlanOfStudySearchModel
                    {
                        Id = student.PlanOfStudyId,
                    });

                    if (planOfStudy != null)
                    {
                        planOfStudys.Add(planOfStudy.Profile);
                    }
                }
            }

            // Получаем список заявлений преподавателя в указанном диапазоне дат
            var statements = _statementStorage.GetFilteredList(new StatementSearchModel
            {
                TeacherId = discipline.TeacherId
            });

            // Создаем ReportDisciplineViewModel и добавляем его в список
            reportDisciplineViewModels.Add(new ReportDisciplineViewModel
            {
                DisciplineName = discipline.Name,
                PlanOfStudys = planOfStudys.Distinct().ToList(), // Убираем дубликаты, если они есть
                Statements = statements.Select(s => s.Name).ToList()
            });
        }

        return reportDisciplineViewModels;
    }

    public List<ReportPlanOfStudyViewModel> GetPlanOfStudyAndDisciplines(int userId)
    {
        var planOfStudies = _planOfStudyStorage.GetFullList();
        var reportPlanOfStudyViewModels = new List<ReportPlanOfStudyViewModel>();

        foreach (var planOfStudy in planOfStudies)
        {
            // Получаем список дисциплин для текущего плана обучения
            var disciplines = _planOfStudyStorage.GetDisciplineFromStudentsFromPlanOfStudys(new PlanOfStudySearchModel { Id = planOfStudy.Id, UserId = userId });

            // Создаем ReportPlanOfStudyViewModel и добавляем его в список
            reportPlanOfStudyViewModels.Add(new ReportPlanOfStudyViewModel
            {
                Id = planOfStudy.Id,
                PlanOfStudyName = planOfStudy.Profile,
				FormOfStudy = planOfStudy.FormOfStudy,
                Disciplines = disciplines.Select(d => d.Name).ToList() // Получаем только имена дисциплин
            });
        }

        return reportPlanOfStudyViewModels;
    }

    public List<ReportPlanOfStudyAndStudentViewModel> GetPlanOfStudyAndStudents()
    {
        var planOfStudies = _planOfStudyStorage.GetFullList();
        var reportPlanOfStudyAndStudentViewModels = new List<ReportPlanOfStudyAndStudentViewModel>();

        foreach (var planOfStudy in planOfStudies)
        {
            // Получаем список студентов для текущего плана обучения
            var students = _disciplineStorage.GetFilteredList(new DisciplineSearchModel { Id = planOfStudy.Id });

            // Создаем списки имен студентов и дисциплин
            var studentNames = students.Select(student => student.Name).ToList();
            var disciplineNames = new List<string>();

            foreach (var student in students)
            {
                // Получаем список дисциплин для текущего студента
                var disciplines = _attestationStorage.GetFilteredList(new AttestationSearchModel { Id = student.Id });

                // Добавляем имена дисциплин в общий список
                disciplineNames.AddRange(disciplines.Select(discipline => discipline.FormOfEvaluation));
            }

            // Создаем ReportPlanOfStudyAndStudentViewModel и добавляем его в список
            reportPlanOfStudyAndStudentViewModels.Add(new ReportPlanOfStudyAndStudentViewModel
            {
                Id = planOfStudy.Id, // Добавляем идентификатор плана обучения
                PlanOfStudyName = planOfStudy.Profile,
                StudentName = studentNames,
                DisciplineName = disciplineNames
            });
        }

        return reportPlanOfStudyAndStudentViewModels;
    }

    public void SaveTeachersToExcel(ReportBindingModel option)
    {
        _saveToExcelStorekeeper.CreateReport(new ExcelInfoStorekeeper
        {
            FileName = option.FileName,
            Title = "Список преподавателей и студентов",
            Teachers = GetTeachers(0)
        });
    }
    public void SavePlanOfStudyToExcel(ReportBindingModel option)
    {
        _saveToExcelWorker.CreateReport(new ExcelInfoWorker
        {
            FileName = option.FileName,
            Title = "Список планов обучения и дисциплин",
            PlanOfStudys = GetPlanOfStudyAndDisciplines(0)
        });
    }

    public void SavePlanOfStudyToWord(ReportBindingModel option)
    {
        _saveToWordWorker.CreateDoc(new WordInfoWorker
        {
            FileName = option.FileName,
            Title = "Список планов обучения и дисциплин",
            PlanOfStudys = GetPlanOfStudyAndDisciplines(0)
        });
    }

    public void SaveTeachersToWord(ReportBindingModel option)
    {
        _saveToWordStorekeeper.CreateDoc(new WordInfoStorekeeper
        {
            FileName = option.FileName,
            Title = "Список преподавателей и студентов",
            TeacherInfo = GetTeachers(0)
        });
    }

    public void SendDisciplinesToEmail(ReportDateRangeBindingModel option)
    {
        _saveToPdfStorekeeper.CreateDoc(new PdfInfoStorekeeper
        {
            FileName = option.FileName,
            Title = "Отчёт по дисциплинам",
            Disciplines = GetDisciplines(option)
        });
    }

    public void SendPlanOfStudyToEmail(ReportBindingModel option)
    {
        _saveToPdfWorker.CreateDoc(new PdfInfoWorker
        {
            FileName = option.FileName,
            Title = "Отчёт по планам обучения",
            PlanOfStudyAndStudent = GetPlanOfStudyAndStudents()
        });
    }
}