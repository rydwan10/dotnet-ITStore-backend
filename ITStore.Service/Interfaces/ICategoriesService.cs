using ITStore.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITStore.Service.Interfaces
{
    public interface ICategoriesService
    {
        Task<List<CategoriesViewDTO>> GetAllCategories();
        Task<CategoriesViewDTO> CreateCategory(CategoriesCreateDTO data, Guid userId);
        Task<CategoriesViewDTO> GetCategoryById(Guid id);
        Task<CategoriesViewDTO> DeleteCategoryById(Guid id, Guid userId);
        Task<CategoriesViewDTO> UpdateCategoryById(Guid id, CategoriesUpdateDTO data, Guid userId);
    }
}
