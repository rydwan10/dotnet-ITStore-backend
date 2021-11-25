using ITStore.DTOs.Categories;
using ITStore.Helpers;
using ITStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static ITStore.Shared.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ITStore.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/categories")]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        // GET: api/{version}/categories
        /// <summary>
        /// Get all categories data
        /// </summary>
        /// <returns>list of all categories data</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseFormat), 200)]
        public async Task<ResponseFormat> GetAll()
        {
            try
            {
                var result = await _categoriesService.GetAllCategories();
                return ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, "Successfully get all categories", result);
            }
            catch (Exception e)
            {
                return ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when getting all categories", e);
            }
        }

        // GET: api/{version}/categories/{id}
        /// <summary>
        /// Get Category by given id
        /// </summary>
        /// <param name="id">Category id with type UUID</param>
        /// <returns>Selected category by given id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), 200)]
        public async Task<ResponseFormat> Get(Guid id)
        {
            try
            {
                var result = await _categoriesService.GetCategoryById(id);
                if(result != null)
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, $"Successfully get category with id {id}", result);
                } else
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.NotFound, $"Cannot find category with id {id}", null);
                }
            }
            catch (Exception e)
            {
                return ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError,  "Error when get category by id", e);
            }
        }

        // POST api/{version}/categories
        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="data">New category data</param>
        /// <returns>Created category</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseFormat), 201)]
        public async Task<ResponseFormat> Post([FromBody] CategoriesCreateDTO data)
        {
            try
            {
                if(data == null)
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.BadRequest, $"Payload for creating new category is invalid", null);
                }
                var result = await _categoriesService.CreateCategory(data);
                return ResponseFormatter.FormatResponse(EnumStatusCodes.Created, $"Successfully created new category", result);
            }
            catch (Exception e)
            {
                return ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when creating new category", e);
            }
        }

        // PUT api/{version}/products/{id}
        /// <summary>
        /// Update category by given id
        /// </summary>
        /// <param name="id">Category id with type UUID</param>
        /// <param name="data">Updated category data</param>
        /// <returns>Updated category</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), 200)]
        public async Task<ResponseFormat> Put(Guid id, [FromBody] CategoriesUpdateDTO data)
        {
            try
            {
                var  result = await _categoriesService.UpdateCategoryById(id, data);
                if(result == null)
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.NotFound, $"Cannot find category with id {id}", null);
                }

                return ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, $"Successfully updated category with id {id}", result);

            } catch (Exception e) {
                return ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when updating category", e);
            }
        }

        // DELETE api/{version}/products/{id}
        /// <summary>
        /// Delete category by given id
        /// </summary>
        /// <param name="id">Category id with type UUID</param>
        /// <returns>Deleted category</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), 200)]
        public async Task<ResponseFormat> Delete(Guid id)
        {
            try
            {
                var result = await _categoriesService.DeleteCategoryById(id);
                if(result == null)
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.NotFound, $"Cannot find category with id {id}", null);
                }
                return ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, $"Successfully deleted category with id {id}", result);
            }
            catch (Exception e)
            {
                return ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when deleting category", e);
            }
        }
    }
}
