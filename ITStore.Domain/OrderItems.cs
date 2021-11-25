using System;

namespace ITStore.Domain
{
    public class OrderItems : BaseProperties
    {
        public Guid OrdersId { get; set; }
        public Guid ProductsId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual Orders Orders { get; set; }
        public virtual Products Products { get; set; }
    }
}
