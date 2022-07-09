using ITStore.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITStore.Service.Interfaces
{
    public interface ICategoriesService
    {
        Task<List<CategoriesViewDTO>> GetAllCategories();
        Task<CategoriesViewDTO> CreateCategory(CategoriesCreateDTO data);
        Task<CategoriesViewDTO> GetCategoryById(Guid id);
        Task<CategoriesViewDTO> DeleteCategoryById(Guid id);
        Task<CategoriesViewDTO> UpdateCategoryById(Guid id, CategoriesUpdateDTO data);
    }
}
