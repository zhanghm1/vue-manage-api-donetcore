using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VueManage.Domain.Base
{
    public interface IRepository<T> where T : IEntityBase
    {

        Task AddAsync(T t);
        Task AddAsync(IEnumerable< T> listT);

        Task UpdateAsync(T t);
        Task UpdateAsync(IEnumerable<T> listT);
        /// <summary>
        /// 假删除
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task DeleteAsync<TD>(TD t) where TD : class, IEntityBase, IEntityDeletedBase;
        /// <summary>
        /// 假删除
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task DeleteAsync<TD>(IEnumerable<TD> t) where TD : class, IEntityBase, IEntityDeletedBase;
        /// <summary>
        /// 真删除
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task RealDeleteAsync(T t);
        /// <summary>
        /// 真删除
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task RealDeleteAsync(IEnumerable<T> t);



        Task<bool> AnyAsync(Expression<Func<T, bool>> where);
        Task<T> FindAsync(int Id);
        Task<T> FindAsync(Expression<Func<T, bool>> where);
        Task<T> FindNoTrackingAsync(int Id);
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> ListNoTrackingAsync(Expression<Func<T, bool>> where);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TKey">排序字段</typeparam>
        /// <param name="where"></param>
        /// <param name="request"></param>
        /// <param name="Asc"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<PageResponse<T>> PageListAsync<TKey>(PageRequest request, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, bool Asc);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TKey">排序字段</typeparam>
        /// <param name="where"></param>
        /// <param name="request"></param>
        /// <param name="Asc"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<PageResponse<TResult>> PageListAsync<TKey,TResult>(PageRequest request, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, bool Asc);


        Task<int> SaveChangeAsync();

        IDbContextTransaction BeginTransaction();
        IIncludableQueryable<T, TKey> Include<TKey>(Expression<Func<T, TKey>> Include);
    }
}
