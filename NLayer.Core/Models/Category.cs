using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Models
{
    public class Category : BaseEntity // miras almayı bu şekilde
    {
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
