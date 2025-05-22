using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.SearchModels;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;

namespace UniversityDatabaseImplement.Implements
{
    public class PlanOfStudyStorageContract : IPlanOfStudyStorageContract
    {
        public List<PlanOfStudyViewModel> GetFullList()
        {
            using var context = new UniversityDbContext();

            return context.PlanOfStudys
                .Include(x => x.Teachers)
                .ThenInclude(t => t.Teacher)
                .ToList()
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<DisciplineViewModel> GetDisciplineFromStudentsFromPlanOfStudys(PlanOfStudySearchModel model)
        {
            using var context = new UniversityDbContext();

            var students = context.Students
                .Where(s => s.PlanOfStudyId == model.Id)
                .Include(s => s.StudentDiscipline)
                .ThenInclude(sd => sd.Discipline)
                .ToList();

            if(students == null)
            {
                return new List<DisciplineViewModel>();
            }

            // Получаем список дисциплин, которые соответствуют условиям поиска в модели PlanOfStudySearchModel
            var disciplines = students
                .SelectMany(s => s.StudentDiscipline)
                .Distinct()
                .ToList();

            if (disciplines == null)
            {
                return new List<DisciplineViewModel>();
            }

            // Преобразуем список дисциплин в список DisciplineViewModel и возвращаем его
            return disciplines
                .Select(d => d.Discipline.GetViewModel).ToList();
        }

        public List<PlanOfStudyViewModel> GetFilteredList(PlanOfStudySearchModel model)
        {
            if(model == null)
            {
                return new();
            }
            using var context = new UniversityDbContext();
            var query = context.PlanOfStudys
                .Include(x => x.Teachers)
                .ThenInclude(x => x.Teacher)
                .Include(x => x.Students)
                .Include(x => x.User)
                .AsQueryable();
            if (model.Id.HasValue)
            {
                query = query.Where(x => x.Id == model.Id.Value);
            }
            return query.Select(x => x.GetViewModel).ToList();
        }

        public PlanOfStudyViewModel? GetElement(PlanOfStudySearchModel model)
        {
            if (!model.Id.HasValue)
            {
                return null;
            }
            using var context = new UniversityDbContext();
            return context.PlanOfStudys
                .Include(x => x.User)
				.Include(x => x.Teachers)
				.ThenInclude(x => x.Teacher)
				.FirstOrDefault(x => (model.Id.HasValue && x.Id == model.Id))?
                .GetViewModel;
        }

        public PlanOfStudyViewModel? Insert(PlanOfStudyBindingModel model)
        {
            using var context = new UniversityDbContext();
            var newAttestation = PlanOfStudy.Create(context, model);

            if (newAttestation == null)
            {
                return null;
            }

            context.PlanOfStudys.Add(newAttestation);
            context.SaveChanges();

            return newAttestation.GetViewModel;
        }

        public PlanOfStudyViewModel? Update(PlanOfStudyBindingModel model)
        {
            using var context = new UniversityDbContext();
            var planOfStudy = context.PlanOfStudys.FirstOrDefault(x => x.Id == model.Id);
            if (planOfStudy == null)
            {
                return null;
            }
            planOfStudy.Update(model);
            planOfStudy.UpdateTeachers(context, model);
            context.SaveChanges();
            return context.PlanOfStudys
                .Include(x => x.User)
                .Include(x => x.Teachers)
                .ThenInclude(x => x.Teacher)
                .FirstOrDefault(x => x.Id == model.Id)?
                .GetViewModel;
        }
		public PlanOfStudyViewModel? Delete(PlanOfStudyBindingModel model)
        {
            using var context = new UniversityDbContext();
            var element = context.PlanOfStudys.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.PlanOfStudys.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }
    }
}
