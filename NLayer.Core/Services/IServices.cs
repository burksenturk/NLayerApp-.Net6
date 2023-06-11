using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface IServices<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(); // dönüş tipini aynı bırakabilirdik değişiklik  olsun diye  değiştirdik.. Repodaki Getall metoduna karışılık bu
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity); //veritabanına bu değişiklşkleri  SaveChangesAsync ile yansıtacağım için dönüş tipi  Task yaptım
        Task RemoveAsync(T entity); //aynı
        Task RemoveRangeAsync(IEnumerable<T> entities); //aynı
    }
}
