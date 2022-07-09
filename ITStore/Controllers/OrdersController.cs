﻿using ITStore.DTOs.Transactions;
using ITStore.Helpers;
using ITStore.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ITStore.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/orders")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        // POST api/{version}/orders
        /// <summary>
        /// Create new order
        /// </summary>
        /// <param name="data">New order data</param>
        /// <returns>Created order</returns>
        /// <response code="200">[Ok] Successfully created new order</response>
        /// <response code="400">[Bad Request] Payload for creating new order is invalid</response>
        /// <response code="500">[Internal Server Error] Error when creating new order</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] TransactionsCreateDTO data)
        {
            try
            {
                if (data == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                           ResponseFormatter.FormatResponse(StatusCodes.Status400BadRequest, $"Payload for creating new order is invalid"));
                }
                var result = await _ordersService.CreateOrder(data);
                return Ok(ResponseFormatter.FormatResponse(StatusCodes.Status200OK, $"Successfully created new order", result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  ResponseFormatter.FormatResponse(StatusCodes.Status500InternalServerError, "Error when creating new order", e));
            }
        }
        // GET api/{version}/orders
        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>List of all orders</returns>
        /// <response code="200">[Ok] Successfully get all orders</response>
        /// <response code="500">[Internal Server Error] Error when getting all orders</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        public async Task<ResponseFormat> GetAll()
        {
            try
            {
                var result = await _ordersService.GetAllOrders();
                return ResponseFormatter.FormatResponse(StatusCodes.Status200OK, $"Successfully get all orders", result);
            }
            catch (Exception e)
            {
                return ResponseFormatter.FormatResponse(StatusCodes.Status200OK, "Error when getting all orders", e);
            }
        }
    }
}
