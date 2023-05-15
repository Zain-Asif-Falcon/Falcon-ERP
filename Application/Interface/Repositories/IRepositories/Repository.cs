using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ERPDBContext context;
        internal DbSet<T> dbSet;
        public readonly ILogger _logger;
        public Repository(ERPDBContext context,ILogger logger)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
            _logger = logger;
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        public async Task<bool> Create(T entity){
            bool updateStatus = false;
            try
            {
                Add(entity);
                var create = await context.SaveChangesAsync();
                if (create > 0)
                {
                    updateStatus = true;
                }
                _logger.LogInformation("Log message in the {Repo} method", typeof(T));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "{Repo} function error", typeof(T));
            }
            
            return updateStatus;
        }
        public T Get(object id)
        {
            return dbSet.Find(id);
        }
        public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> filter = null)
        {
            try
            {
                return await context
                    .Set<T>().Where(filter)//.GetType().GetProperty("deleted_at").//.Where(c=>c.PropertyType.GetProperty("deleted_at") == null) //.Where(c=> c.GetType().GetProperty("deleted_at") == null)//.GetValue(this,null)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                return null;
            }

        }
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            try
            {
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                // include properties will be comma separated
                if (includeProperties != null)
                {
                    foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }
                if (orderBy != null)
                {
                    return await orderBy(query).ToListAsync();
                }
                _logger.LogInformation("Log message in the {Repo} method", typeof(T));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} function error", typeof(T));
            }
            return query;
        }

       

        public async Task<IEnumerable<T>> GetRecordsToShow(int pageNo, int pageSize, int CurrentPage, Expression<Func<T, bool>> wherePredict, Expression<Func<T, int>> orderByPredict)
        {
            if (wherePredict != null)
            {
                return await dbSet.OrderBy(orderByPredict).Where(wherePredict).ToListAsync();
            }
            else
            {
                return await dbSet.OrderBy(orderByPredict).ToListAsync();
            }
        }

        public void InactiveAndDeleteMarkByWhereClause(Expression<Func<T, bool>> wherePredict, Action<T> ForEachPredict)
        {
            dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);
        }

        public void Remove(T tbl)
        {
            try
            {
                if (context.Entry(tbl).State == EntityState.Detached)
                    dbSet.Attach(tbl);
                dbSet.Remove(tbl);
                context.SaveChanges();

                _logger.LogInformation("Log message in the {Repo} method", typeof(T));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} function error", typeof(T));
            }            
        }

        public void RemoveRangeWhereByClause(Expression<Func<T, bool>> wherePredict)
        {
            List<T> entity = dbSet.Where(wherePredict).ToList();
            foreach (var ent in entity)
            {
                Remove(ent);
            }
            context.SaveChanges();
        }

        public void RemoveWhereByClause(Expression<Func<T, bool>> wherePredict)
        {
            T entity = dbSet.Where(wherePredict).FirstOrDefault();
            Remove(entity);
            context.SaveChanges();
        }

       
        public void UpdateWherByClause(Expression<Func<T, bool>> wherePredict, Action<T> ForEachPredict)
        {
            dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);
            context.SaveChanges();
        }
        //public IEnumerable<T> GetResultBySqlProcedure(string query, params object[] parameters)
        //{
        //    if (parameters != null)
        //    {
        //        return context.Database.SqlQuery<T>(query, parameters).ToList();
        //    }
        //    else
        //    {
        //        return context.Database.SqlQuery<T>(query).ToList();
        //    }
        //}
    }
}
