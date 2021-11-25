using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.DTOs.Products
{
    public class ProductsCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string SKU { get; set; }
        public Guid? CategoriesId { get; set; }
        [Required]
        public Guid InventoriesId { get; set; }
        [Required]
        public decimal Price { get; set; }
        public Guid? DiscountsId { get; set; }
    }
}
