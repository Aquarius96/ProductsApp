using System.Collections.Generic;

namespace ProductsApp.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; } = new List<Product>();
    }
}
