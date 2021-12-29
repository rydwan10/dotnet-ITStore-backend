using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.DTOs.Carts
{
    public class CartsCreateDTO
    {
        public Guid ProductsId { get; set; }
        public int Quantity { get; set; }
    }
}
