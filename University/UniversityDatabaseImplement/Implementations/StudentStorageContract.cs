using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.SearchModels;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;
using UniversityDatabaseImplement.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UniversityDatabaseImplement.Implements
{
    public class StudentStorageContract : IStudentStorageContract
    {
        public List<StudentViewModel> GetFullList()
        {
            using var context = new UniversityDbContext();

            return context.Students.Select(x => x.GetViewModel).ToList();
        }

        public List<StudentViewModel> GetFilteredList(StudentSearchModel model)
        {
            if (model == null)
            {
                return new List<StudentViewModel>();
            }

            using var context = new UniversityDbContext();

            // Начальный запрос без фильтрации
            var query = context.Students
                .Include(x => x.User)
                .Include(x => x.PlanOfStudy)
                .AsQueryable();

            // Если в модели поиска указан Id, добавляем условие фильтрации
            if (model.Id.HasValue)
            {
                query = query.Where(x => x.Id == model.Id.Value);
            }

            // Выполняем запрос и получаем результаты
            var results = query
                .Select(x => x.GetViewModel)
                .ToList();

            return results;
        }

        public StudentViewModel? GetElement(StudentSearchModel model)
        {
            if (string.IsNullOrEmpty(model.Name) && !model.Id.HasValue)
            {
                return null;
            }

            using var context = new UniversityDbContext();

            return context.Students.FirstOrDefault(x => (!string.IsNullOrEmpty(model.Name) 
                && x.Name == model.Name) || (model.Id.HasValue && x.Id == model.Id))?.GetViewModel;
        }

        public StudentViewModel? Insert(StudentBindingModel model)
        {
            using var context = new UniversityDbContext();

            var newStudent = Student.Create(context, model);

            if (newStudent == null)
            {
                return null;
            }

            context.Students.Add(newStudent);
            context.SaveChanges();

            return newStudent.GetViewModel;
        }

        public StudentViewModel? Update(StudentBindingModel model)
        {
            using var context = new UniversityDbContext();

            var student = context.Students.FirstOrDefault(x => x.Id == model.Id);

            if (student == null)
            {
                return null;
            }

            student.Update(model);
            context.SaveChanges();

            return student.GetViewModel;
        }

        public StudentViewModel? Delete(StudentBindingModel model)
        {
            using var context = new UniversityDbContext();

            var element = context.Students.FirstOrDefault(rec => rec.Id == model.Id);

            if (element != null)
            {
                context.Students.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }

            return null;
        }
    }
}
