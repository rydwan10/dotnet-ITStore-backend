using AutoMapper;
using Microsoft.Extensions.Configuration;
using ITStore.DTOs.Users;
using ITStore.Helpers;
using ITStore.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using static ITStore.Shared.Enums;
using ITStore.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ITStore.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/authentications")]
    [AllowAnonymous]
    public class AuthenticationsController : ControllerBase
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly SignInManager<ApplicationUsers> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AuthenticationsController(
            AppDbContext context, 
            UserManager<ApplicationUsers> userManager, 
            SignInManager<ApplicationUsers> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }

        // POST api/{version}/accounts/login
        /// <summary>
        /// Login with registered email
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns>Token and token expiration time</returns>
        /// <response code="200">[Ok] Successfully login</response>
        /// <response code="400">[Bad Request] Invalid credentials</response>
        /// <response code="500">[Internal Server Error] Error when login with user credentials</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] UserCredentialsDTO userCredentials)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, isPersistent: false, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    var createdToken = await BuildToken(userCredentials);
                    return Ok(ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, "Successfully login", createdToken));
                } else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ResponseFormatter.FormatResponse(EnumStatusCodes.BadRequest, "Invalid credentials", null)); 
                }
            }
            catch (Exception e) { 
            
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when login with user credentials", e));
            }
        }

        // POST api/{version}/accounts/register
        /// <summary>
        /// Register new user account
        /// </summary>
        /// <param name="userRegistersDTO"></param>
        /// <returns>Token and token expiration time</returns>
        /// <response code="200">[Ok] User successfully registered</response>
        /// <response code="400">[Bad Request] Email already taken / User failed to register</response>
        /// <response code="500">[Internal Server Error] Error when registering user</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseFormat), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] UserRegistersDTO userRegistersDTO)
        {
            try
            {
                var findUser = await _userManager.FindByEmailAsync(userRegistersDTO.Email);

                if(findUser != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ResponseFormatter.FormatResponse(EnumStatusCodes.BadRequest, "Email already taken", null));
                }

                var newUser = new ApplicationUsers
                {
                    UserName = userRegistersDTO.Email,
                    Email = userRegistersDTO.Email,
                    PhoneNumber = userRegistersDTO.PhoneNumber,
                    FirstName = userRegistersDTO.FirstName,
                    LastName = userRegistersDTO.LastName,
                };

                var result = await _userManager.CreateAsync(newUser, userRegistersDTO.Password);
                await _userManager.AddToRoleAsync(newUser, "User");


                var credentials = new UserCredentialsDTO
                {
                    Email = userRegistersDTO.Email,
                    Password = userRegistersDTO.Password,
                };

                if(result.Succeeded)
                {
                    var createdToken = await BuildToken(credentials);
                    return Ok(ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, "User successfully registered", createdToken));
                } else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ResponseFormatter.FormatResponse(EnumStatusCodes.BadRequest, "User failed to register", null));
                }
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when registering user", e));
            }
        }

        private async Task<AuthenticationDTO> BuildToken(UserCredentialsDTO userCredentials)
        {
            var user = await _userManager.FindByEmailAsync(userCredentials.Email);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim("email", userCredentials.Email),
                new Claim("userId", user.Id)
            };

            if(roles != null)
            { 
                foreach (var role in roles)
                {
                    var newClaim = new Claim(ClaimTypes.Role, role);
                    claims.Add(newClaim);
                }
            }

            var claimsDB = await _userManager.GetClaimsAsync(user);



            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Change expiration time to minutes later
            var expiration = DateTime.UtcNow.AddMinutes(10);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: credentials);

            return new AuthenticationDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiration
            };

        }
    }
}
