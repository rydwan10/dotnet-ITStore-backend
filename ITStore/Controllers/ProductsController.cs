using ITStore.DTOs.Products;
using ITStore.Helpers;
using ITStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static ITStore.Shared.Enums;

namespace ITStore.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        // GET: api/{version}/products
        /// <summary>
        /// Get all products data
        /// </summary>
        /// <returns>list of all products data</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseFormat), 200)]
        public async Task<ResponseFormat> GetAll()
        {
            try
            {
                var result = await _productsService.GetAllProducts();
                return ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, "Successfully get all products", result);
            }
            catch (Exception e)
            {
                return ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when getting all products", e);
            }
        }

        // GET api/{version}/products/{id}
        /// <summary>
        /// Get Product by given id
        /// </summary>
        /// <param name="id">Product id with type UUID</param>
        /// <returns>Selected product by given id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), 200)]
        public async Task<ResponseFormat> Get(Guid id)
        {
            try
            {
                var result = await _productsService.GetProductById(id);
                if(result != null)
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, $"Successfully get product with id {id}", result);
                } else
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.NotFound, $"Cannot find product with id {id}", null);
                }
            }
            catch (Exception e)
            {
                return ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when get product by id", e);
            }
        }

        // POST api/{version}/products
        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="data">New product data</param>
        /// <returns>Created product</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseFormat), 200)]
        public async Task<ResponseFormat> Post([FromBody] ProductsCreateDTO data)
        {
            try
            {
                if (data == null)
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.BadRequest, $"Payload for creating new product is invalid", null);
                }
                var result = await _productsService.CreateProduct(data);
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
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), 200)]
        public async Task<ResponseFormat> Put(Guid id, [FromBody] ProductsUpdateDTO data)
        {
            try
            {
                var result = await _productsService.UpdateProductById(id, data);
                if (result == null)
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.NotFound, $"Cannot find product with id {id}", null);
                }

                return ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, $"Successfully updated product with id {id}", result);

            }
            catch (Exception e)
            {
                return ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when updating product", e);
            }
        }

        // DELETE api/{version}/products/{id}
        /// <summary>
        /// Delete product by given id
        /// </summary>
        /// <param name="id">Product id with type UUID</param>
        /// <returns>Deleted product</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), 200)]
        public async Task<ResponseFormat> Delete(Guid id)
        {
            try
            {
                var result = await _productsService.DeleteProductById(id);
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
