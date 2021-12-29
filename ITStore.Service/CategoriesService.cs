using ITStore.Persistence;
using ITStore.DTOs.Categories;
using ITStore.Service.Interfaces;
using ITStore.Domain;

using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System;
using System.Linq;

namespace ITStore.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoriesViewDTO> CreateCategory(CategoriesCreateDTO data, Guid userId)
        {
            var newCategory = _mapper.Map<Categories>(data);

            newCategory.CreatedBy(userId);

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            var result = await GetCategoryById(newCategory.Id);
            var mappedResult = _mapper.Map<CategoriesViewDTO>(result);

            return mappedResult;
        }

        public async Task<CategoriesViewDTO> UpdateCategoryById(Guid id, CategoriesUpdateDTO data, Guid userId)
        {

            var selectedCategory = await _context.Categories.FindAsync(id);
            if (string.IsNullOrWhiteSpace(id.ToString()) || selectedCategory == null) return null;


            var updatedCategory = _mapper.Map(data, selectedCategory);
            updatedCategory.ModifiedBy(userId);


            await _context.SaveChangesAsync();

            var mappedResult = _mapper.Map<CategoriesViewDTO>(updatedCategory);
            return mappedResult;
        }

        public async Task<CategoriesViewDTO> DeleteCategoryById(Guid id, Guid userId)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null) return null;

            category.DeletedBy(userId);
            await _context.SaveChangesAsync();

            var mappedResult = _mapper.Map<CategoriesViewDTO>(category);
            return mappedResult;
        }

        public async Task<List<CategoriesViewDTO>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            var mappedResult = _mapper.Map<List<CategoriesViewDTO>>(categories);

            return mappedResult;
        }

        public async Task<CategoriesViewDTO> GetCategoryById(Guid id)
        {
            var result = await _context.Categories.FindAsync(id);
            var mappedResult = _mapper.Map<CategoriesViewDTO>(result);

            return mappedResult;
        }
    }
}
