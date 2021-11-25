using AutoMapper;
using System;
using ITStore.DTOs.Categories;
using ITStore.Domain;
using ITStore.DTOs.Products;
using System.Collections.Generic;
using ITStore.DTOs.Discounts;
using ITStore.DTOs.Inventories;

namespace ITStore.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            // Categories
            CreateMap<CategoriesViewDTO, Categories>().ReverseMap();
            CreateMap<CategoriesCreateDTO, Categories>();
            CreateMap<Categories, CategoriesViewDTO>();
            CreateMap<CategoriesUpdateDTO, Categories>();

            // Products Domain -> ProductsView
            CreateMap<Products, ProductsViewDTO>()
                .ForMember(x => x.Categories, opts => opts.MapFrom(MapProductsCategories))
                .ForMember(x => x.Discounts, opts => opts.MapFrom(MapProductsDiscounts))
                .ForMember(x => x.Inventories, opts => opts.MapFrom(MapProductsInventories));
            // ProductsUpdate -> Products Domain
            CreateMap<ProductsUpdateDTO, Products>()
                .ForMember(x => x.Categories, opts => opts.Ignore())
                .ForMember(x => x.Discounts, opts => opts.Ignore())
                .ForMember(x => x.Inventories, opts => opts.Ignore());

        }
        private CategoriesViewDTO MapProductsCategories(Products products, ProductsViewDTO productsViewDTO)
        {
            if(products.Categories != null)
            {
                return new CategoriesViewDTO() 
                { 
                    Id = products.Categories.Id, 
                    Name = products.Categories.Name, 
                    Description = products.Categories.Description
                };                
            }

            return null;
        }

        private Guid? MapProductsCategories(ProductsUpdateDTO productsUpdateDTO, Products products)
        {
            return productsUpdateDTO.CategoriesId;
        }

        private DiscountsViewDTO MapProductsDiscounts(Products products, ProductsViewDTO productsViewDTO)
        {
            if (products.Categories != null)
            {
                return new DiscountsViewDTO()
                {
                    Id = products.Discounts.Id,
                    Name = products.Discounts.Name,
                    Description = products.Discounts.Description,
                    DiscountPercent = products.Discounts.DiscountPercent,
                    IsActive = products.Discounts.IsActive
                };
            }

            return null;
        }

        private Guid? MapProductsDiscounts(ProductsUpdateDTO productsUpdateDTO, Products products)
        {
            return productsUpdateDTO.DiscountsId;
        }

        private InventoriesViewDTO MapProductsInventories(Products products, ProductsViewDTO productsViewDTO)
        {
            if (products.Categories != null)
            {
                return new InventoriesViewDTO()
                {
                    Id = products.Inventories.Id,
                    Quantity = products.Inventories.Quantity
                };
            }

            return null;
        }

        private Guid MapProductsInventories(ProductsUpdateDTO productsUpdateDTO, Products products)
        {
            return productsUpdateDTO.InventoriesId;
        }
    }
}
