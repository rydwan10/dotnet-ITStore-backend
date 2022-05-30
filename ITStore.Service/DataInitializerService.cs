using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITStore.Domain;
using ITStore.Persistence;
using ITStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITStore.Services
{
	public class DataInitializerService : IDataInitializerService
	{
        private readonly AppDbContext _context;

        private static Guid PhoneCategoryId = new Guid("ee5c44db-558d-4ae9-aeff-ee311a34f98f");
        private static Guid LaptopCategoryId = new Guid("1eb4efe1-6c66-46a5-b422-12b473e83374");
        private static Guid AccessoriesCategoryId = new Guid("925072de-f3fe-486c-a3c9-d4618a15cb1d");

        private static Guid DiscountId = new Guid("0915553c-435a-45b9-b611-5299c62eafef");

        public DataInitializerService(AppDbContext context)
		{
            _context = context;
		}

        public async Task<string> Initialize()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        private async Task InitializeDiscounts()
        {
            var checkDiscounts = await _context.Discounts.ToListAsync();
            if(!checkDiscounts.Any())
            {
                Discounts discount = new Discounts()
                {
                    Id = DiscountId,
                    Description = "20% off Discount",
                    DiscountPercent = 20M,
                    IsActive = true,
                    Name = "20% off discount",
                };

                discount.CreatedBy(Guid.Empty);

                await _context.Discounts.AddAsync(discount);

                await _context.SaveChangesAsync();
            }
        }

        private async Task InitializeCategories()
        {
            var checkCategory = await _context.Categories.ToListAsync();
            if(!checkCategory.Any())
            {
                List<Categories> categories = new List<Categories>()
                {
                    new Categories
                    {
                        Id = PhoneCategoryId,
                        Name = "Phone",
                        Description = "Phone Category"
                    },
                    new Categories
                    {
                        Id = LaptopCategoryId,
                        Name = "Laptop",
                        Description = "Laptop Category"
                    },
                    new Categories
                    {
                        Id = AccessoriesCategoryId,
                        Name = "Accessories",
                        Description = "Accessories Category"
                    }
                };

                foreach (var category  in categories)
                {
                    category.CreatedBy(Guid.Empty);
                }

                await _context.Categories.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
            }
        }

        private async Task InitializeProducts()
        {
            var checkProducts = await _context.Products.ToListAsync();
            if(!checkProducts.Any())
            {
                List<Products> products = new List<Products>()
                {
                    new Products
                    {
                        Id = Guid.NewGuid(),
                        Name = "iPhone 12 Pro",
                        Description = "iPhone 12 Pro",
                        SKU = "APPL/IP/12",
                        CategoriesId = PhoneCategoryId,
                        DiscountsId = DiscountId,
                    },
                    new Products
                    {
                        Id = Guid.NewGuid(),
                        Name = "MacBook Pro 14",
                        Description = "MacBook Pro 14",
                        SKU = "APPL/MAC/14",
                        CategoriesId = LaptopCategoryId,
                        DiscountsId = null,
                    },
                    new Products
                    {
                        Id = Guid.NewGuid(),
                        Name = "Xiaomi 12",
                        Description = "Xiaomi 12",
                        SKU = "MI/XIA/12",
                        CategoriesId = PhoneCategoryId,
                        DiscountsId = null,
                    },
                    new Products
                    {
                        Id = Guid.NewGuid(),
                        Name = "Asus TUF zlk129",
                        Description = "Asus Tuf zlk129",
                        SKU = "ASUS/TUF/zlik129",
                        CategoriesId = LaptopCategoryId,
                        DiscountsId = null,
                    },
                    new Products
                    {
                        Id = Guid.NewGuid(),
                        Name = "POCO X3 Pro",
                        Description = "POCO X3 Pro",
                        SKU = "POCO/X3/Pro",
                        CategoriesId = PhoneCategoryId,
                        DiscountsId = null
                    }
                };

                List<Inventories> inventories = new List<Inventories>();

                foreach (var product in products)
                {
                    Inventories productInventory = new Inventories()
                    {
                        Id = Guid.NewGuid(),
                        Quantity = 100
                    };
                    productInventory.CreatedBy(Guid.Empty);
                    product.CreatedBy(Guid.Empty);
                    product.InventoriesId = productInventory.Id;
                    inventories.Add(productInventory);
                }

                await _context.Inventories.AddRangeAsync(inventories);
                await _context.Products.AddRangeAsync(products);
            }
        }
        private async Task InitializeUsers()
        {
            var checkUser = await _context.
        }
    }
}

