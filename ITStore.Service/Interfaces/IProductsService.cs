﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITStore.Domain;
using ITStore.DTOs.Products;

namespace ITStore.Service.Interfaces
{
    public interface IProductsService
    {
        Task<List<ProductsViewDTO>> GetAllProducts();
        Task<ProductsViewDTO> CreateProduct(ProductsCreateDTO data, Guid userId);
        Task<ProductsViewDTO> GetProductById(Guid id);
        Task<ProductsViewDTO> DeleteProductById(Guid id, Guid userId);
        Task<ProductsViewDTO> UpdateProductById(Guid id, ProductsUpdateDTO data, Guid userId);
    }
}
