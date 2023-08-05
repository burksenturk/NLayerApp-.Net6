using System.Linq.Expressions;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);  
        IQueryable<T> GetAll();
        // yazdığımız sorgular direkt database ye gitmez where den sonra gider //func falan delegedir delegeler metotları işaret eden yapılardır.
        IQueryable<T> Where(Expression<Func<T,bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity); //uzun süren bir işlem olmadııgı için async u yok. Efcore zaten bellekte tutuyor sadece modifyed olarak değiştiriyor
        void Remove(T entity);  // remove da aynı update gibi
        void RemoveRange(IEnumerable<T> entities);
    }
}
