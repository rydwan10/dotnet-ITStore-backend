using ITStore.DTOs.Carts;
using ITStore.Helpers;
using ITStore.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ITStore.Shared.Enums;

namespace ITStore.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/carts")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartsController : ControllerBase
    {
        protected Guid UserId { get; set; }
        public readonly ICartsService _cartsService;

        public CartsController(ICartsService cartsService, IHttpContextAccessor httpContextAccessor)
        {
            _cartsService = cartsService;
            var claimsIdentity = httpContextAccessor.HttpContext.User;
            UserId = new Guid(claimsIdentity.FindFirst("userId").Value);
        }

        // GET api/{version}/carts
        /// <summary>
        /// Get all carts data
        /// </summary>
        /// <returns>List of all carts data</returns>
        /// <response code="200">[Ok] Successfully get products in cart</response>
        /// <response code="500">[Internal Server Error] Error when getting products in cart</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var result = await _cartsService.GetCarts(UserId);
                return Ok(ResponseFormatter.FormatResponse(StatusCodes.Status200OK, "Successfully get products in cart", result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  ResponseFormatter.FormatResponse(StatusCodes.Status500InternalServerError, "Error when getting products in cart", e));
            }
        }

        // POST api/{version}/carts
        /// <summary>
        /// Create new carts data
        /// </summary>
        /// <param name="data">New carts item data</param>
        /// <returns>Created carts item</returns>
        /// <response code="200">[Ok] Successfully created new carts item</response>
        /// <response code="400">[Bad Request] Payload for creating new carts item is invalid</response>
        /// <response code="500">[Internal Server Error] Error when creating carts item</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] CartsCreateDTO data)
        {
            try
            {
                if(data == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                                      ResponseFormatter.FormatResponse(StatusCodes.Status400BadRequest, "Payload for creating new carts item is invalid"));
                }
                var result = await _cartsService.AddToCarts(data, UserId);
                return Ok(ResponseFormatter.FormatResponse(StatusCodes.Status200OK, "Successfully created new carts item", result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  ResponseFormatter.FormatResponse(StatusCodes.Status500InternalServerError, "Error when creating carts item", e));
            }
        }

        // DELETE api/{version}/carts
        /// <summary>
        /// Delete carts item by given id
        /// </summary>
        /// <param name="id">Carts item id with type UUID</param>
        /// <response code="200">[Ok] Successfully deleted carts item with id {id}</response>
        /// <response code="404">[Not Found] Cannot find carts item with id {id}</response>
        /// <response code="500">[Internal Server Error] Error when deleting carts item {id}</response>
        /// <returns>Deleted carts item</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _cartsService.RemoveFromCarts(id, UserId);
                if(result == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                                      ResponseFormatter.FormatResponse(StatusCodes.Status404NotFound, $"Cannot find carts item with id {id}", null));
                }
                return Ok(ResponseFormatter.FormatResponse(StatusCodes.Status200OK, $"Successfully deleted carts item with id {id}", result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       ResponseFormatter.FormatResponse(StatusCodes.Status500InternalServerError, "Error when deleting carts item", e));
            }
        }
    }
}
