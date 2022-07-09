using ITStore.DTOs.Categories;
using ITStore.Helpers;
using ITStore.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ITStore.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/categories")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : ControllerBase
    {
        protected Guid UserId { get; set; }

        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        //[HttpGet("claims")]
        //public object GetClaims()
        //{
        //    return User.Claims.Select(c => new { Type = c.Type, Value = c.Value });
        //}

        // GET api/{version}/categories
        /// <summary>
        /// Get all categories data
        /// </summary>
        /// <returns>List of all categories data</returns>  
        /// <response code="200">[Ok] Successfully get all categories</response>
        /// <response code="500">[Internal Server Error] Error when getting all categories</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var result = await _categoriesService.GetAllCategories();
                return Ok(ResponseFormatter.FormatResponse(StatusCodes.Status200OK, "Successfully get all categories", result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  ResponseFormatter.FormatResponse(StatusCodes.Status500InternalServerError, "Error when getting all categories", e));
            }
        }

        // GET api/{version}/categories/{id}
        /// <summary>
        /// Get Category by given id
        /// </summary>
        /// <param name="id">Category id with type UUID</param>
        /// <returns>Selected category by given id</returns>
        /// <response code="200">[Ok] Successfully get category with id {id}</response>
        /// <response code="404">[Not Found] Cannot find category with id {id}</response>
        /// <response code="500">[Internal Server Error] Error when get category by id</response>   
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Get(Guid id)
        {
            try
            {
                var result = await _categoriesService.GetCategoryById(id);
                if(result != null)
                {
                    return Ok(ResponseFormatter.FormatResponse(StatusCodes.Status200OK, $"Successfully get category with id {id}", result));
                } else
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                                      ResponseFormatter.FormatResponse(StatusCodes.Status404NotFound, $"Cannot find category with id {id}"));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  ResponseFormatter.FormatResponse(StatusCodes.Status500InternalServerError,  "Error when get category by id", e));
            }
        }

        // POST api/{version}/categories
        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="data">New category data</param>
        /// <returns>Created category</returns>
        /// <response code="200">[Ok] Successfully created new category</response>
        /// <response code="400">[Bad Request] Payload for creating new category is invalid</response>
        /// <response code="500">[Internal Server Error] Error when creating new category</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Post([FromBody] CategoriesCreateDTO data)
        {
            try
            {
                if(data == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ResponseFormatter.FormatResponse(StatusCodes.Status400BadRequest, $"Payload for creating new category is invalid"));
                }
                var result = await _categoriesService.CreateCategory(data);
                return Ok(ResponseFormatter.FormatResponse(StatusCodes.Status200OK, $"Successfully created new category", result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  ResponseFormatter.FormatResponse(StatusCodes.Status500InternalServerError, "Error when creating new category", e));
            }
        }

        // PUT api/{version}/products/{id}
        /// <summary>
        /// Update category by given id
        /// </summary>
        /// <param name="id">Category id with type UUID</param>
        /// <param name="data">Updated category data</param>
        /// <returns>Updated category</returns>
        /// <response code="200">[Ok] Successfully updated category with id {id}</response>
        /// <response code="404">[Not Found] Cannot find category with id {id}</response>
        /// <response code="500">[Internal Server Error] Error when updating category</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Put(Guid id, [FromBody] CategoriesUpdateDTO data)
        {
            try
            {
                var  result = await _categoriesService.UpdateCategoryById(id, data);
                if(result == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                                      ResponseFormatter.FormatResponse(StatusCodes.Status404NotFound, $"Cannot find category with id {id}"));
                }

                return Ok(ResponseFormatter.FormatResponse(StatusCodes.Status200OK, $"Successfully updated category with id {id}", result));

            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  ResponseFormatter.FormatResponse(StatusCodes.Status500InternalServerError, "Error when updating category", e));
            }
        }

        // DELETE api/{version}/products/{id}
        /// <summary>
        /// Delete category by given id
        /// </summary>
        /// <param name="id">Category id with type UUID</param>
        /// <returns>Deleted category</returns>
        /// <response code="200">[Ok] Successfully deleted category with id {id}</response>
        /// <response code="404">[Not Found] Cannot find category with id {id}</response>
        /// <response code="500">[Internal Server Error] Error when deleting category</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _categoriesService.DeleteCategoryById(id);
                if(result == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                           ResponseFormatter.FormatResponse(StatusCodes.Status404NotFound, $"Cannot find category with id {id}"));
                }
                return Ok(ResponseFormatter.FormatResponse(StatusCodes.Status200OK, $"Successfully deleted category with id {id}", result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                           ResponseFormatter.FormatResponse(StatusCodes.Status500InternalServerError, "Error when deleting category", e));
            }
        }
    }
}
