using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITStore.Domain;
using ITStore.Persistence;
using ITStore.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITStore.Services
{
	public class DataInitializerService : IDataInitializerService
	{
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        private readonly Guid _phoneCategoryId = new Guid("ee5c44db-558d-4ae9-aeff-ee311a34f98f");
        private readonly Guid _laptopCategoryId = new Guid("1eb4efe1-6c66-46a5-b422-12b473e83374");
        private readonly Guid _accessoriesCategoryId = new Guid("925072de-f3fe-486c-a3c9-d4618a15cb1d");
        private readonly Guid _discountId = new Guid("0915553c-435a-45b9-b611-5299c62eafef");

        public DataInitializerService(AppDbContext context, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole<Guid>> roleManager)
		{
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> Initialize()
        {
            await InitializeRoles();
            await InitializeAdmin();
            await InitializeUsers();
            await InitializeDiscounts();
            await InitializeCategories();
            await InitializeProducts();

            return "Initialized";
        }

        private async Task InitializeRoles()
        {
            var isRoleAdminExist = await _roleManager.RoleExistsAsync("ADMIN");
            if (!isRoleAdminExist)
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("ADMIN"));
            }

            var isRoleUserExist = await _roleManager.RoleExistsAsync("USER");
            if (!isRoleUserExist)
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("USER"));
            }
        }

        private async Task InitializeAdmin()
        {
            var listUser = new List<ApplicationUsers>()
            {
                new ApplicationUsers
                {
                    UserName = "rydwan.dev@gmail.com",
                    Email = "rydwan.dev@gmail.com",
                    PhoneNumber = "082117055066",
                    FirstName = "Muhammad",
                    LastName = "Rydwan",
                },
                new ApplicationUsers
                {
                    UserName = "admin@itstore.com",
                    Email = "admin@itstore.com",
                    PhoneNumber = "085111222333",
                    FirstName = "Admin",
                    LastName = "",
                },
            };

           
            foreach (var newUser in listUser)
            {
                var isEmailTaken = await _userManager.FindByEmailAsync(newUser.Email);
                if (isEmailTaken == null)
                {
                    await _userManager.CreateAsync(newUser, "admin12345");
                    await _userManager.AddToRoleAsync(newUser, "ADMIN");
                }
            }
        }

        private async Task InitializeDiscounts()
        {
            var checkDiscounts = await _context.Discounts.ToListAsync();
            if(!checkDiscounts.Any())
            {
                var discount = new Discounts()
                {
                    Id = _discountId,
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
                        Id = _phoneCategoryId,
                        Name = "Phone",
                        Description = "Phone Category"
                    },
                    new Categories
                    {
                        Id = _laptopCategoryId,
                        Name = "Laptop",
                        Description = "Laptop Category"
                    },
                    new Categories
                    {
                        Id = _accessoriesCategoryId,
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
                        CategoriesId = _phoneCategoryId,
                        DiscountsId = _discountId,
                    },
                    new Products
                    {
                        Id = Guid.NewGuid(),
                        Name = "MacBook Pro 14",
                        Description = "MacBook Pro 14",
                        SKU = "APPL/MAC/14",
                        CategoriesId = _laptopCategoryId,
                        DiscountsId = null,
                    },
                    new Products
                    {
                        Id = Guid.NewGuid(),
                        Name = "Xiaomi 12",
                        Description = "Xiaomi 12",
                        SKU = "MI/XIA/12",
                        CategoriesId = _phoneCategoryId,
                        DiscountsId = null,
                    },
                    new Products
                    {
                        Id = Guid.NewGuid(),
                        Name = "Asus TUF zlk129",
                        Description = "Asus Tuf zlk129",
                        SKU = "ASUS/TUF/zlik129",
                        CategoriesId = _laptopCategoryId,
                        DiscountsId = null,
                    },
                    new Products
                    {
                        Id = Guid.NewGuid(),
                        Name = "POCO X3 Pro",
                        Description = "POCO X3 Pro",
                        SKU = "POCO/X3/Pro",
                        CategoriesId = _phoneCategoryId,
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

                await _context.SaveChangesAsync();
            }
        }
        private async Task InitializeUsers()
        {
           var listUser = new List<ApplicationUsers>()
            {
                new ApplicationUsers
                {
                    UserName = "user1@itstore.com",
                    Email = "user1@itstore.com",
                    PhoneNumber = "085111222333",
                    FirstName = "User 1",
                    LastName = "",
                },
                new ApplicationUsers
                {
                    UserName = "user2@itstore.com",
                    Email = "user2@itstore.com",
                    PhoneNumber = "085111222333",
                    FirstName = "User 2",
                    LastName = "",
                },
            };

           
           foreach (var newUser in listUser)
           {
               var isEmailTaken = await _userManager.FindByEmailAsync(newUser.Email);
               if (isEmailTaken == null)
               {
                   await _userManager.CreateAsync(newUser, "admin12345");
                   await _userManager.AddToRoleAsync(newUser, "USER");
               }
           }
        }
    }
}

