using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Models
{
    public class Product : BaseEntity
    {
        ////public Product(string name)//constructor null değer olmasını istemezsem
        ////{
        ////    Name = name ?? throw new ArgumentNullException(nameof(Name));
        ////    //null olursa null exception hatası fırlatır.
        ////}
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; } // foreign key
        public Category Category { get; set; } // sadece 1 tane olacağını belirliyorum

        public ProductFeature ProductFeature { get; set; } // sadece 1 tane olacağını belirliyorum.
    }
}
/*String ve sınıflar referans tiplidir 
 * o yüzden altında yeşil oluyor bunu halletmek için ? koymam
 lazım
name alanı null olmasın*/