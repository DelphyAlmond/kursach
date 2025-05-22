using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicContracts
{
    public interface IReportContract
    {
        /// <summary>
        /// Часть кладовщика
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        List<ReportTeacherViewModel> GetTeachers(int userId);
        List<ReportDisciplineViewModel> GetDisciplines(ReportDateRangeBindingModel model);
        List<ReportPlanOfStudyViewModel> GetPlanOfStudyAndDisciplines(int userId);
        List<ReportPlanOfStudyAndStudentViewModel> GetPlanOfStudyAndStudents();

		void SaveTeachersToWord(ReportBindingModel option);
        void SavePlanOfStudyToWord(ReportBindingModel option);
		void SaveTeachersToExcel(ReportBindingModel option);
        void SavePlanOfStudyToExcel(ReportBindingModel option);
		void SendDisciplinesToEmail(ReportDateRangeBindingModel option);
        public void SendPlanOfStudyToEmail(ReportBindingModel option);
	}
}
