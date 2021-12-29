using System;
using ITStore.DTOs.Categories;
using ITStore.DTOs.Inventories;
using ITStore.DTOs.Discounts;

namespace ITStore.DTOs.Products
{
    public class ProductsViewDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public bool? IsInStock
        {
            get
            {
                try
                {
                    return Inventories?.Quantity > 0 ? true : false;
                }
                catch (Exception)
                {

                    return null;
                }
            }
        }
        public CategoriesViewDTO Categories { get; set; }
        public InventoriesViewDTO Inventories { get; set; }
        public DiscountsViewDTO Discounts { get; set; }
    }
}
