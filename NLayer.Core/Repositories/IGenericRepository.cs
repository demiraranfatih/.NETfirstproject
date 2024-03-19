using System.Linq.Expressions;
//veritabanına yapılacak tüm temel sorgular eklenecek.
namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        //IQueryable falan dönenlerde veritabanında sorgu atılmaz.
        Task<T> GetByIdAsync(int id);

        //productRepository.GetAll(x=>x.id>5).Tolist();
        IQueryable<T> GetAll();

        //productRepository.where(x=>x.id>5).OrderBy.TolistAsync(); buradaki to list async veritabanına gidiyor.
        //t alacak bool dönecek expression burada değişken küçük harfle başlayan için
        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression); // 
        //bool hasAny = await AnyAsync(x => x.Property == someValue); gibi düşün
        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);
        //update ve remove asenkron işlemi yok.
        void Update(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);//varlıklar
    }
}
