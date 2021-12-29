using System;

namespace ITStore.DTOs.Products
{
    public class ProductOrdersViewDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
    }
}
