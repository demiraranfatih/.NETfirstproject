using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLayer.Core.Models;
using NLayer.Repository.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    public class AppDbContext:DbContext//dbcontext sınıfından alacak
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)//veritabanı yolunu buradan veriyorum constructor. // class<değişken> adı 
        {
            
        }

        //her bir entitiye karşılık dbset oluşturulacak.
        /*Bu sınıf, bir veritabanı tablosunu temsil eden bir koleksiyonu ifade eder 
         * ve bu koleksiyon üzerinde çeşitli sorguların yapılmasına ve veritabanı
         * işlemlerinin gerçekleştirilmesine olanak tanır.*/
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }/*biri product feature eklemek istiyorsa mutlaka product nesnesi üzerinden
       eklesin diyebilirim 
        productRepository.add(new Product) gibi ekleyeceğim
        var p = new Product() { ProductFeature = new ProductFeature() { } };
        prop içerisine yazılabilir.*/

        //override on tab model oluşurken çalışacak olan metod
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //yapılan configurationları buradan kolayca execute edebiliyorsun
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            //Prdouct feature için seed burada da yapılabilir
            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature()
            {
                Id = 1,
                Color = "Kırmızı",
                Height = 100,
                Width = 200,
                ProductId = 1,
            },
            new ProductFeature(){
                Id = 2 ,
                Color = "Mavi" , 
                Height = 300 , 
                Width = 500 ,
                ProductId = 2 ,
            });
        }

    }
}
