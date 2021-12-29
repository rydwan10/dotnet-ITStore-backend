using ITStore.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.DTOs.Wishlists
{
    public class WishlistsViewDTO
    {
        public Guid Id { get; set; }
        public ProductsViewDTO Products { get; set; }
    }
}
