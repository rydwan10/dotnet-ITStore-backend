using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.DTOs.Carts
{
    public class CartsCreateDTO
    {
        [Required]
        public Guid ProductsId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
