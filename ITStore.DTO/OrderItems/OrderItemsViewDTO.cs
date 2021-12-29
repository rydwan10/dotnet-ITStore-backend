using ITStore.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.DTOs.OrderItems
{
    public class OrderItemsViewDTO
    {
        public Guid Id { get; set; }
        public ProductOrdersViewDTO Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
