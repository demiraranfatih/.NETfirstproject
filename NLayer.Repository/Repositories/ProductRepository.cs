using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        //context'le ilgili şeyleri aktif etmem gerekiyor ya
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        //burada generic repositoryi miras alacak ki hazır fonksiyonlar gelsin
        //implementasyonu yaptığımda sadece bir method geldi. çünkü generic repositoryde var hazır crud fonksiyonları
        public async Task<List<Product>> GetProductsWithCategory()
        {

            //Eagle Loading
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }
    }
}
