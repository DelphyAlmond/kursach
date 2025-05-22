using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.SearchModels;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;

namespace UniversityDatabaseImplement.Implements
{
    public class DisciplineStorageContract : IDisciplineStorageContract
    {
        public List<StudentViewModel> GetStudentsForDiscipline(DisciplineSearchModel model)
        {
            using var context = new UniversityDbContext();

            var discipline = context.Disciplines
                .Include(d => d.Students)
                .ThenInclude(sd => sd.Student)
                .FirstOrDefault(d => d.Id == model.Id);

            if (discipline == null)
            {
                return new List<StudentViewModel>(); // Если дисциплина не найдена, возвращаем пустой список
            }

            return discipline.Students
                .Select(sd => sd.Student.GetViewModel)
                .ToList();
        }

        public DisciplineViewModel? Delete(DisciplineBindingModel model)
        {
            using var context = new UniversityDbContext();
            var element = context.Disciplines.Include(x => x.Students).FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Disciplines.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }

        public DisciplineViewModel? GetElement(DisciplineSearchModel model)
        {
            if (string.IsNullOrEmpty(model.Name) && !model.Id.HasValue)
            {
                return null;
            }
            using var context = new UniversityDbContext();
            return context.Disciplines.Include(x => x.Students).ThenInclude(x => x.Student)
            .Include(x => x.Teacher)
            .FirstOrDefault(x =>
           (!string.IsNullOrEmpty(model.Name) && x.Name == model.Name) || (model.Id.HasValue && x.Id == model.Id)) ?.GetViewModel;

        }


        //ешьте котиков в них витамин с
        public List<DisciplineViewModel> GetFilteredList(DisciplineSearchModel model)
        {
            CheckSearchModel(model);

            using var context = new UniversityDbContext();
            var query = context.Disciplines
                .Include(x => x.Students)
                .ThenInclude(x => x.Student)
                .Include(x => x.Teacher)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.Name))
            {
                query = query.Where(x => x.Name.Contains(model.Name));
            }

            if (!string.IsNullOrEmpty(model.Description))
            {
                query = query.Where(x => x.Description.Contains(model.Description));
            }

            if (model.Id.HasValue)
            {
                query = query.Where(x => x.Id == model.Id.Value);
            }

            if (model.TeacherId.HasValue)
            {
                query = query.Where(x => x.TeacherId == model.TeacherId.Value);
            }



            if (model.DateFrom.HasValue && model.DateTo.HasValue)
            {
                //query = query.Where(x => model.DateFrom.Value <= x.Date && x.Date <= model.DateTo.Value);
            

            var x = query.Select(x => x.GetViewModel).ToList();
            var res = new List<DisciplineViewModel>();

            foreach (var item in x) { 
                if (item.Date > model.DateFrom && item.Date < model.DateTo)
                {
                    res.Add(item);
                }
            }
            return res;
            }
            return query.Select(x => x.GetViewModel).ToList();
        }


        public List<DisciplineViewModel> GetFullList()
        {
            using var context = new UniversityDbContext();
            return context.Disciplines
            .Include(x => x.Students)
            .ThenInclude(x => x.Student)
			.Include(x => x.Teacher)
			.ToList()
            .Select(x => x.GetViewModel)
           .ToList();
        }

        public DisciplineViewModel? Insert(DisciplineBindingModel model)
        {
            using var context = new UniversityDbContext();
            var newDiscipline = Discipline.Create(context, model);
            if (newDiscipline == null)
            {
                return null;
            }
            context.Disciplines.Add(newDiscipline);
            context.SaveChanges();
            return newDiscipline.GetViewModel;
        }

        public DisciplineViewModel? Update(DisciplineBindingModel model)
        {
            using var context = new UniversityDbContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                    var discipline = context.Disciplines.FirstOrDefault(x => x.Id == model.Id);
                if (discipline == null)
                {
                    return null;
                }
                discipline.Update(model);
                context.SaveChanges();
                discipline.UpdateStudents(context, model);
                transaction.Commit();
                return discipline.GetViewModel;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private void CheckSearchModel(DisciplineSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException("Передаваемая модель пуста", nameof(model));
            if (model.DateFrom.HasValue != model.DateTo.HasValue)
                throw new ArgumentException($"Не указано начало {model.DateFrom} или конец {model.DateTo} периода.");
        }
    }
}
