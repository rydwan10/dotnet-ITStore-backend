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
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using static ITStore.Shared.Enums;
using ITStore.Domain;

namespace ITStore.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly SignInManager<ApplicationUsers> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AccountsController(
            AppDbContext context, 
            UserManager<ApplicationUsers> userManager, 
            SignInManager<ApplicationUsers> signInManager, 
            IConfiguration configuration,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        // POST: api/{version}/accounts/login
        /// <summary>
        /// Login with registered email
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns>Token and token expiration time</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseFormat), 200)]
        
        public async Task<ResponseFormat> Login([FromBody] UserCredentialsDTO userCredentials)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, isPersistent: false, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    var createdToken = await BuildToken(userCredentials);
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.Ok, "Successfully Login", createdToken);
                } else
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.BadRequest, "Invalid Credentials", null);
                }
            }
            catch (Exception e)
            {

                return ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when login with user credentials", e);
            }
        }

        // POST: api/{version}/accounts/register
        /// <summary>
        /// Register new user account
        /// </summary>
        /// <param name="userRegistersDTO"></param>
        /// <returns>Token and token expiration time</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ResponseFormat), 201)]
        public async Task<ResponseFormat> Register([FromBody] UserRegistersDTO userRegistersDTO)
        {
            try
            {
                var newUser = new ApplicationUsers
                {
                    UserName = userRegistersDTO.Email,
                    Email = userRegistersDTO.Email,
                    PhoneNumber = userRegistersDTO.PhoneNumber,
                    FirstName = userRegistersDTO.FirstName,
                    LastName = userRegistersDTO.LastName,
                };

                var result = await _userManager.CreateAsync(newUser, userRegistersDTO.Password);

                var credentials = new UserCredentialsDTO
                {
                    Email = userRegistersDTO.Email,
                    Password = userRegistersDTO.Password,
                };

                if(result.Succeeded)
                {
                    var createdToken = await BuildToken(credentials);
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.Created, "User successfully registered", createdToken);
                } else
                {
                    return ResponseFormatter.FormatResponse(EnumStatusCodes.BadRequest, "User failed to register", null);
                }
            }
            catch (Exception e)
            {

                return ResponseFormatter.FormatResponse(EnumStatusCodes.InternalServerError, "Error when registering user", e);
            }
        }

        private async Task<AuthenticationDTO> BuildToken(UserCredentialsDTO userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", userCredentials.Email)
            };

            var user = await _userManager.FindByEmailAsync(userCredentials.Email);
            var claimsDB = await _userManager.GetClaimsAsync(user);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyjwt"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Change expiration time to minutes later
            var expiration = DateTime.UtcNow.AddSeconds(10);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: credentials);

            return new AuthenticationDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiration
            };

        }
    }
}
