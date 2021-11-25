using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.Domain
{
    public class Products : BaseProperties
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public Guid? CategoriesId { get; set; }
        public Guid InventoriesId { get; set; }
        public decimal Price { get; set; }
        public Guid? DiscountsId { get; set; }

        public virtual Categories Categories { get; set; }
        public virtual Inventories Inventories { get; set; }
        public virtual Discounts Discounts { get; set; }
    }
}
