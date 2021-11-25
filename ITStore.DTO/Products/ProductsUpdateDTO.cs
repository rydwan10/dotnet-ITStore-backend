using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ITStore.DTOs.Products
{
    public class ProductsUpdateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public Guid? CategoriesId { get; set; }
        public Guid? DiscountsId { get; set; }
        public Guid InventoriesId { get; set; }
        public decimal Price { get; set; }
    }
}
