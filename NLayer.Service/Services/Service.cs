using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class Service<T> : IService<T> where T : class
    {
        //bu servis busines kodlarının olacağı yer
        //saveleme işlerini burada yapacağım
        //ekleme işlemleri olacağı için her ikisini de ekledim.
        private readonly IGenericRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public Service(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.AnyAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }
        

        //id üzerinden silme ve getirme yaparken yapmış olduğum middleware e gönderiyorum hatayı null ise değilse producttı gönderiyorum
        public async Task<T> GetByIdAsync(int id)
        {
            var hasproduct = await _repository.GetByIdAsync(id);
            if (hasproduct == null)
            {//oluşturduğum middleware.
                throw new NotFoundException($"{typeof(T).Name}({id}) not found middlewareden gelen service katmanında kontrol yapıyor.");
            }
            return hasproduct;
        }

        public async Task RemoveAsync(T entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _repository.Where(expression);
        }
    }
}

/*
 Kodunuzda gördüğümüz gibi, `Service<T>` sınıfının constructor'ında `IGenericRepository<T>` tipinden bir nesne bekliyor. Ancak, aslında bu sınıfın kullanımı sırasında bu interface yerine `GenericRepository<T>` sınıfının örneğini geçiyorsunuz.

`GenericRepository<T>` sınıfı, `IGenericRepository<T>` arayüzünü implemente ettiği için, bu sınıfın bir örneği `IGenericRepository<T>` tipine atanabilir. Bu nedenle `Service<T>` sınıfı, `IGenericRepository<T>` tipinden bir nesne alabilir ve
ardından bu nesneyi kullanabilir.

Yani, `Service<T>` sınıfının constructor'ında `IGenericRepository<T>` beklemesi, gerçekte `GenericRepository<T>` gibi bir sınıfın geçilebileceği anlamına gelir. Bu, sınıfın genel bir arayüzü (interface) kabul etmesi ve bu arayüzü implemente 
eden sınıfların geçilebilmesi prensibine dayanır. Bu durumda, `GenericRepository<T>`, `IGenericRepository<T>` arayüzünü uyguladığı için bu sınıfın örnekleri `IGenericRepository<T>` tipine atanabilir ve bu nesne `Service<T>` sınıfına geçilebilir.

Yani, `Service<T>` sınıfı aslında bir "genel" servis sınıfıdır ve çeşitli varlık türleri için çalışabilir, bu nedenle genellikle generic bir repository sınıfı ile ilişkilendirilir. Bu sayede, 
farklı varlık türleri için aynı temel servis mantığını kullanabilirsiniz.*/
