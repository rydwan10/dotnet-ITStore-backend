using ITStore.DTOs.Products;
using ITStore.Helpers;
using ITStore.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static ITStore.Shared.Enums;

namespace ITStore.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : ControllerBase
    {
        protected Guid UserId { get; set; }

        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService, IHttpContextAccessor httpContextAccessor)
        {
            _productsService = productsService;
            var claimsIdentity = httpContextAccessor.HttpContext.User;
            UserId = new Guid(claimsIdentity.FindFirst("userId").Value);
        }

        // GET api/{version}/products
        /// <summary>
        /// Get all products data
        /// </summary>
        /// <returns>list of all products data</returns>
        /// <response code="200">=[Ok] Successfully get all products</response>
        /// <response code="500">=[Internal Server Error] Error when getting all products</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var result = await _productsService.GetAllProducts();
                return Ok(ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, "Successfully get all products", result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when getting all products", e));
            }
        }

        // GET api/{version}/products/{id}
        /// <summary>
        /// Get Product by given id
        /// </summary>
        /// <param name="id">Product id with type UUID</param>
        /// <returns>Selected product by given id</returns>
        /// <response code="200">[Ok] Successfully get product with id {id}</response>
        /// <response code="404">[Not Found] Cannot find product with id {id}</response>
        /// <response code="500">[Internal Server Error] Error when get prouduct by id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult> Get(Guid id)
        {
            try
            {
                var result = await _productsService.GetProductById(id);
                if(result != null)
                {
                    return Ok(ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, $"Successfully get product with id {id}", result));
                } else
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseFormatter.FormatResponse(EnumStatusCodes.NotFound, $"Cannot find product with id {id}", null));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when get product by id", e));
            }
        }

        // POST api/{version}/products
        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="data">New product data</param>
        /// <returns>Created product</returns>
        /// <response code="200">[Ok] Successfully created new product</response>
        /// <response code="400">[Bad Request] Payload for creating new product is invalid</response>
        /// <response code="500">[Internal Server Error] Error when creating new product</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseFormat> Post([FromBody] ProductsCreateDTO data)
        {
            try
            {
                if (data == null)
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.BadRequest, $"Payload for creating new product is invalid", null);
                }
                var result = await _productsService.CreateProduct(data, UserId);
                return ResponseFormatter.FormatResponse(EnumStatusCodes.Created, $"Successfully created new product", result);
            }
            catch (Exception e)
            {

                return ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when creating new product", e);
            }
            
        }

        // PUT api/{version}/products/{id}
        /// <summary>
        /// Update product by given id
        /// </summary>
        /// <param name="id">Product id with type UUID</param>
        /// <param name="data">Updated product data</param>
        /// <returns>Updated product</returns>
        /// <response code="200">[Ok] Successfully update product with id {id}</response>
        /// <response code="404">[Not Found] Cannot find product with id {id}</response>
        /// <response code="500">[Internal Server Error] Error when updating product</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Put(Guid id, [FromBody] ProductsUpdateDTO data)
        {
            try
            {
                var result = await _productsService.UpdateProductById(id, data, UserId);
                if (result == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseFormatter.FormatResponse(EnumStatusCodes.NotFound, $"Cannot find product with id {id}", null));
                }

                return Ok(ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, $"Successfully update product with id {id}", result));

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when updating product", e));
            }
        }

        // DELETE api/{version}/products/{id}
        /// <summary>
        /// Delete product by given id
        /// </summary>
        /// <param name="id">Product id with type UUID</param>
        /// <returns>Deleted product</returns>
        /// <response code="200">[Ok] Successfully deleted product with id {id}</response>
        /// <response code="404">[Not Found] Cannot find product with id {id}</response>
        /// <response code="500">[Internal Serer Error] Error when deleting product</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseFormat> Delete(Guid id)
        {
            try
            {
                var result = await _productsService.DeleteProductById(id, UserId);
                if (result == null)
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.NotFound, $"Cannot find product with id {id}", null);
                }
                return ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, $"Successfully deleted product with id {id}", result);
            }
            catch (Exception e)
            {
                return ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when deleting product", e);
            }
        }
    }
}
