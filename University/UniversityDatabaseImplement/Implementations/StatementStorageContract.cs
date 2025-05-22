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
    public class StatementStorageContract: IStatementStorageContract
    {
        public StatementViewModel? GetElement(StatementSearchModel model)
        {
            if (!model.Id.HasValue)
            {
                return null;
            }
            using var context = new UniversityDbContext();
            return context.Statements.Include(x => x.Teacher).FirstOrDefault(x => (model.Id.HasValue && x.Id == model.Id))?.GetViewModel;
        }

		public List<StatementViewModel> GetFilteredList(StatementSearchModel model)
		{
			using var context = new UniversityDbContext();

			// Фильтр по Id
			IQueryable<Statement> query = context.Statements;
			if (model.Id.HasValue)
			{
				query = query.Where(x => x.Id == model.Id.Value);
			}

			// Фильтр по Date
			if (model.Date.HasValue)
			{
				query = query.Where(x => x.Date <= model.Date.Value);
			}

			// Загрузка связанных данных
			return query.Include(x => x.Teacher).Select(x => x.GetViewModel).ToList();
		}

		public List<StatementViewModel> GetFullList()
        {
            using var context = new UniversityDbContext();
            return context.Statements.Include(x => x.Teacher).Select(x => x.GetViewModel).ToList();
        }

        public StatementViewModel? Insert(StatementBindingModel model)
        {
            using var context = new UniversityDbContext();
            var newStatement = Statement.Create(context, model);
            if (newStatement == null)
            {
                return null;
            }
            context.Statements.Add(newStatement);
            context.SaveChanges();
            return context.Statements.Include(x => x.Teacher).FirstOrDefault(x => x.Id == newStatement.Id)?.GetViewModel;
        }

        public StatementViewModel? Update(StatementBindingModel model)
        {
            using var context = new UniversityDbContext();
            var order = context.Statements.FirstOrDefault(x => x.Id == model.Id);
            if (order == null)
            {
                return null;
            }
            order.Update(model);
            context.SaveChanges();
            return context.Statements.Include(x => x.Teacher).FirstOrDefault(x => x.Id == model.Id)?.GetViewModel;
        }
        public StatementViewModel? Delete(StatementBindingModel model)
        {
            using var context = new UniversityDbContext();
            var element = context.Statements.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Statements.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }
    }
}
