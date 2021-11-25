using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ITStore.Domain;
using ITStore.DTOs.Products;
using ITStore.Persistence;
using ITStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITStore.Services
{
    public class ProductsService : IProductsService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductsService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductsViewDTO> CreateProduct(ProductsCreateDTO data)
        {
            var newProduct = _mapper.Map<Products>(data);

            newProduct.Id = Guid.NewGuid();
            newProduct.CreatedBy(Guid.Empty);

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            var mappedResult = await GetProductById(newProduct.Id);
            return mappedResult;
        }

        public async Task<ProductsViewDTO> DeleteProductById(Guid id)
        {
            var product = await _context.Products.Include(x => x.Inventories).Include(x => x.Discounts).Include(x => x.Categories).SingleOrDefaultAsync(x => x.Id == id);
            if (product == null) return null;

            product.DeletedBy(Guid.Empty);

            // Delete unused inventory data
            var productInventory = await _context.Inventories.SingleOrDefaultAsync(x => x.Id == product.InventoriesId);
            productInventory.DeletedBy(Guid.Empty);

            await _context.SaveChangesAsync();

            var mappedResult = _mapper.Map<ProductsViewDTO>(product);
            return mappedResult;
        }

        public async Task<List<ProductsViewDTO>> GetAllProducts()
        {
            var products = await _context.Products.Include(x => x.Categories).Include(x => x.Inventories).Include(x => x.Discounts).ToListAsync();
            var mappedResult = _mapper.Map<List<ProductsViewDTO>>(products);
            
            return mappedResult;
        }

        public async Task<ProductsViewDTO> GetProductById(Guid id)
        {
            var result = await _context.Products.Include(x => x.Categories).Include(x => x.Inventories).Include(x => x.Discounts).FirstOrDefaultAsync(x => x.Id == id);
            var mappedResult = _mapper.Map<ProductsViewDTO>(result);

            return mappedResult;
        }

        public async Task<ProductsViewDTO> UpdateProductById(Guid id, ProductsUpdateDTO data)
        {
            var selectedProduct = await _context.Products.FindAsync(id);
            if (string.IsNullOrWhiteSpace(id.ToString()) || selectedProduct == null) return null;

            var updatedProduct = _mapper.Map(data, selectedProduct);

            updatedProduct.Id = id;
            updatedProduct.ModifiedBy(Guid.Empty);

            await _context.SaveChangesAsync();

            var updatedProductResult = await _context.Products.Include(x => x.Inventories).Include(x => x.Discounts).Include(x => x.Categories).SingleOrDefaultAsync(x => x.Id == updatedProduct.Id);

            var mappedResult = _mapper.Map<ProductsViewDTO>(updatedProductResult);
            return mappedResult;
        }
    }
}
