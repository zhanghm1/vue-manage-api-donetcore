using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using VueManage.Domain.Base;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Query;
using VueManage.Domain;
using AutoMapper;
using VueManage.Domain.Entities;

namespace VueManage.Infrastructure.EFCore
{
    public class EFRepository<T> : IRepository<T> where T :class,IEntityBase
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IMapper _mapper;
        public EFRepository(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._dbSet = this._context.Set<T>();
            this._mapper = mapper;
            
        }

        public async Task AddAsync(T t)
        {
            await _dbSet.AddAsync(t);
            //await _context.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<T> listT)
        {
            await _dbSet.AddRangeAsync(listT);
            //await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync<TD>(TD t) where TD : class, IEntityBase, IEntityDeletedBase
        {
            t.IsDeleted = true;
            _context.Set<TD>().Update(t);
            
            //return (await _context.SaveChangesAsync())>0;
        }
        public async Task DeleteAsync<TD>(IEnumerable<TD> t) where TD : class, IEntityBase, IEntityDeletedBase
        {
            foreach (var item in t)
            {
                item.IsDeleted = true;
            }
            _context.Set<TD>().UpdateRange(t);
        }

        public async Task<T> FindAsync(int Id)
        {
           return await  _dbSet.Where(a => a.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<T> FindNoTrackingAsync(int Id)
        {
            return await _dbSet.Where(a => a.Id == Id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> where)
        {
            return await _dbSet.Where(where).ToListAsync();
        }

        public async Task<PageResponse<T>> PageListAsync<TKey>( PageRequest request, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order , bool Asc)
        {
            var Iquery = _dbSet.Where(where);
            var orderQuery = Iquery.OrderByDescending(a=>a.Id);
            if (Asc)
            {
                orderQuery= Iquery.OrderBy(order);
            }
            else
            {
                orderQuery = Iquery.OrderByDescending(order);
            }

            PageResponse<T> resp = new PageResponse<T>() {
                PageIndex= request.PageIndex,
                PageSize= request.PageSize
            };

            resp.Total = Iquery.Count();
            var ind = resp.Total / resp.PageSize;
            resp.PageCount = (int)Math.Ceiling((double)resp.Total / (double)resp.PageSize);

            resp.List = await orderQuery.Skip((request.PageIndex - 1) * request.PageSize ).Take(request.PageSize).ToListAsync();

            return resp;
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T t)
        {
            _dbSet.Update(t);
            //await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<T> listT)
        {
            _dbSet.UpdateRange(listT);
            //await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            
            return transaction;
        }
        public void TransactionCommit(IDbContextTransaction transaction)
        {
            try
            {
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }

        public async Task RealDeleteAsync(T t)
        {
            _dbSet.Remove(t);
            //return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> where)
        {
            return await _dbSet.Where(where).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ListNoTrackingAsync(Expression<Func<T, bool>> where)
        {
            return await _dbSet.Where(where).AsNoTracking().ToListAsync();
        }
        public IIncludableQueryable<T, TKey> Include<TKey>(Expression<Func<T, TKey>> Include)
        {
            return _dbSet.Include(Include);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        {
            return await _dbSet.AnyAsync(where);
        }

        public async Task RealDeleteAsync(IEnumerable<T> list)
        {
            _dbSet.RemoveRange(list);
        }

        public async Task<PageResponse<TResult>> PageListAsync<TKey, TResult>(PageRequest request, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, bool Asc)
        {
            var Iquery = _dbSet.Where(where);
            var orderQuery = Iquery.OrderByDescending(a => a.Id);
            if (Asc)
            {
                orderQuery = Iquery.OrderBy(order);
            }
            else
            {
                orderQuery = Iquery.OrderByDescending(order);
            }

            PageResponse<TResult> resp = new PageResponse<TResult>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            resp.Total = Iquery.Count();
            var ind = resp.Total / resp.PageSize;
            resp.PageCount = (int)Math.Ceiling((double)resp.Total / (double)resp.PageSize);

            var list = await orderQuery.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            
            resp.List = _mapper.Map<List<T>,List<TResult>>(list);

            return resp;
        }



        #endregion

    }
}
