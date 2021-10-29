using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Get(object id);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null,Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,string includeProperties = null);
        T GetFirstOrDefaultExp(Expression<Func<T, bool>> filter = null,string includeProperties = null);
        void Add(T entity);
        Task<bool> Create(T entity);
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> filter = null);
        void Remove(T entity);
        void Remove(object id);
        Task<IEnumerable<T>> GetAllRecordsOrderByAsc(Expression<Func<T, int>> orderClause);
        Task<IEnumerable<T>> GetAllRecordsOrderByDesc(Expression<Func<T, int>> orderClause);
        Task<IEnumerable<T>> GetListByParameterNOrderByDesc(Expression<Func<T, bool>> wherePredict, Expression<Func<T, int>> orderClause);
        IQueryable<T> GetAllRecordsIQueryable();
        int GetAllRecordsCount();
        void Update(T tbl);
        void UpdateWherByClause(Expression<Func<T, bool>> wherePredict, Action<T> ForEachPredict);
        Task<T> GetFirstorDefault(int recordId);
        void RemoveWhereByClause(Expression<Func<T, bool>> wherePredict);
        void RemoveRangeWhereByClause(Expression<Func<T, bool>> wherePredict);
        void InactiveAndDeleteMarkByWhereClause(Expression<Func<T, bool>> wherePredict, Action<T> ForEachPredict);
        T GetFirstorDefaultByParameter(Expression<Func<T, bool>> wherePredict);
        Task<IEnumerable<T>> GetListByParameter(Expression<Func<T, bool>> wherePredict);
        Task<IEnumerable<T>> GetRecordsToShow(int pageNo, int pageSize, int CurrentPage, Expression<Func<T, bool>> wherePredict, Expression<Func<T, int>> orderByPredict);
        //IEnumerable<T> GetResultBySqlProcedure(string query, params object[] parameters);
    }
}
