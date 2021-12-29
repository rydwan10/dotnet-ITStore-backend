using AutoMapper;
using System;
using ITStore.DTOs.Categories;
using ITStore.Domain;
using ITStore.DTOs.Products;
using System.Collections.Generic;
using ITStore.DTOs.Discounts;
using ITStore.DTOs.Inventories;
using ITStore.DTOs.Wishlists;
using ITStore.DTOs.Carts;
using ITStore.DTOs.OrderPayments;
using ITStore.DTOs.Orders;
using ITStore.DTOs.OrderItems;
using ITStore.DTOs.ShippingAddresses;
using ITStore.DTOs.Users;

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

            CreateMap<Products, ProductOrdersViewDTO>();

            // Wishlists Domain -> WishlistsView
            CreateMap<Wishlists, WishlistsViewDTO>()
                .ForMember(x => x.Products, opts => opts.MapFrom(
                    (wishlists, wishilistsViewDTO, i, context) =>
                        {
                            return context.Mapper.Map<ProductsViewDTO>(wishlists.Products);
                        }
                    )
                );
            CreateMap<WishlistsCreateDTO, Wishlists>();

            // Carts Domain -> CartsView
            CreateMap<Carts, CartsViewDTO>()
                .ForMember(x => x.Products, opts => opts.MapFrom((carts, cartsviewDTO, i, context) => 
                    {
                        return context.Mapper.Map<ProductsViewDTO>(carts.Products);
                    }
                ));
            CreateMap<CartsCreateDTO, Carts>();

            // Order 
            CreateMap<OrderPaymentsCreateDTO, OrderPayments>();
            CreateMap<OrdersCreateDTO, Orders>()
                .ForMember(x => x.OrderPayments, opts => opts.Ignore());
            CreateMap<OrderItemsCreateDTO, OrderItems>()
                .ForMember(x => x.Orders, opts => opts.Ignore())
                .ForMember(x => x.Products, opts => opts.Ignore());
            CreateMap<ShippingAddressesCreateDTO, ShippingAddresses>()
                .ForMember(x => x.Orders, opts => opts.Ignore());

            CreateMap<OrderPayments, OrderPaymentsViewDTO>();

            // User
            CreateMap<ApplicationUsers, ApplicationUsersViewDTO>();

            CreateMap<Orders, OrdersViewDTO>()
                .ForMember(x => x.OrderPayments, opts => opts.MapFrom((orders, ordersViewDTO, i, context) => 
                    {
                        return context.Mapper.Map<OrderPaymentsViewDTO>(orders.OrderPayments);
                    }
                )).ForMember(x => x.Users, opts => opts.MapFrom((orders, ordersViewDTO, i, context) => 
                    {
                        return context.Mapper.Map<ApplicationUsersViewDTO>(orders.Users);
                    }
                ));
            CreateMap<OrderItems, OrderItemsViewDTO>()
                .ForMember(x => x.Product, opts => opts.MapFrom((orderItems, orderItemsViewDTO, i, context) =>
                    {
                        return context.Mapper.Map<ProductOrdersViewDTO>(orderItems.Products);
                    }
                ));
            CreateMap<ShippingAddresses, ShippingAddressesViewDTO>();
        }

        #region Products Domain -> ProductsView
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
        #endregion

        #region Wishlists Domain -> WishlistsView

       

        #endregion
    }
}
