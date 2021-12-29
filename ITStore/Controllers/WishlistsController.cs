﻿using ITStore.DTOs.Wishlists;
using ITStore.Helpers;
using ITStore.Services.Interfaces;
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
    [Route("api/v{version:apiVersion}/wishlists")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WishlistsController : ControllerBase
    {
        protected Guid UserId { get; set; }
        private readonly IWishlistsService _wishlistsService;

        public WishlistsController(IWishlistsService wishlistsService, IHttpContextAccessor httpContextAccessor)
        {
            _wishlistsService = wishlistsService;
            var claimsIdentity = httpContextAccessor.HttpContext.User;
            UserId = new Guid(claimsIdentity.FindFirst("userId").Value);
        }

        // GET api/{version}/wishlists
        /// <summary>
        /// Get all user wishlists
        /// </summary>
        /// <returns>list of all wishlists data</returns>
        /// <response code="200">[Ok] Successfully get wishlist</response>
        /// <response code="500">[Internal Server Error] Error when getting wishlists</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll() 
        {
            try
            {
                var result = await _wishlistsService.GetWishlists(UserId);
                return Ok(ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, "Successfully get wishlists", result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when getting wishlists", e));
            }
        }

        // POST api/{version}/wishlists
        /// <summary>
        /// Crate new wishlist
        /// </summary>
        /// <param name="data">New wishlist data</param>
        /// <returns>Created wishlist</returns>
        /// <response code="200">[Ok] Successfully created new wishlit</response>
        /// <response code="400">[Bad Request] Payload for creating new wishlist is invalid</response>
        /// <response code="500">[Internal Server Error] Error when creating wishlist</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] WishlistsCreateDTO data)
        {
            try
            {
                if(data == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ResponseFormatter.FormatResponse(EnumStatusCodes.BadRequest, "Payload for creating new wishlist is invalid"));
                }
                var result = await _wishlistsService.CreateWishlists(data, UserId);
                return Ok(ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, "Successfully created new wishlist", result));
            }
            catch (Exception e)
            {
                return  StatusCode(StatusCodes.Status500InternalServerError, ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when creating wishlist", e));
            }
        }

        // DELETE api/{version}/wishlists/{id}
        /// <summary>
        /// Delete wishlist by given id
        /// </summary>
        /// <param name="id">Wishlist id with type UUID</param>
        /// <returns>Deleted wishlists data</returns>
        /// <response code="200">[Ok] Successfully delete wishlist with id {id}</response>
        /// <response code="404">[Not Found] Cannot find wishlist with id {id}</response>
        /// <response code="500">[Internal Server Error] Error when deleting wishlist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _wishlistsService.RemoveWishlists(id, UserId);
                if(result == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseFormatter.FormatResponse(EnumStatusCodes.NotFound, $"Cannot find wishlist with id {id}", null));
                }
                return Ok(ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, $"Successfully deleted wishlist with id {id}", result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when deleting wishlist", e));
            }
        }

    }
}