using ITStore.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.DTOs.Carts
{
    public class CartsViewDTO
    {
        public Guid Id { get; set; }
        public ProductsViewDTO Products { get; set; }
        public int Quantity { get; set; }
    }
}
