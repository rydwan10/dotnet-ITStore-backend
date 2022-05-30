using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITStore.Helpers;
using ITStore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ITStore.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/data-initializer")]
    public class DataInitializerController : Controller
    {
        private readonly IDataInitializerService _dataInitializerService;
        public DataInitializerController(IDataInitializerService dataInitializerService)
        {
            _dataInitializerService = dataInitializerService;
        }

        [HttpPost]

        public async Task <ActionResult<string>> Initialize()
        {
            try
            {
                var result = await _dataInitializerService.Initialize();
                return Ok(ResponseFormatter.FormatResponse(StatusCodes.Status200OK, $"Data sucessfully initialized", result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  ResponseFormatter.FormatResponse(StatusCodes.Status500InternalServerError, "Error when initializing data", e));
            }
        }
    }
}

