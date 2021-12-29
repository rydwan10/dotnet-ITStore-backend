using ITStore.Domain;
using ITStore.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ITStore.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/roles")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUsers> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUsers> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Setup()
        {
            var dataAdmin = new ApplicationUsers
            {
                Email = "admin@test.com",
                UserName = "admin@test.com"                
            };

            var result = await _userManager.CreateAsync(dataAdmin, "admin12345");

            var defaultAdmin = new ApplicationUsers();
            if(result.Succeeded)
            {
                defaultAdmin = await _userManager.FindByEmailAsync("admin@test.com");
            }

            var adminRole = await _roleManager.FindByNameAsync("Admin");
            if(adminRole == null)
            {
                adminRole = new IdentityRole("Admin");
                await _roleManager.CreateAsync(adminRole);

                await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "data.create"));
                await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "data.read"));
                await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "data.update"));
                await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "data.delete"));
            }

            if(!await _userManager.IsInRoleAsync(defaultAdmin, adminRole.Name))
            {
                await _userManager.AddToRoleAsync(defaultAdmin, adminRole.Name);
            }

            var accountManagerRole = await _roleManager.FindByNameAsync("Account Manager");
            if(accountManagerRole == null)
            {
                accountManagerRole = new IdentityRole("Account Manager");
                await _roleManager.CreateAsync(accountManagerRole);

                await _roleManager.AddClaimAsync(accountManagerRole, new Claim(CustomClaimTypes.Permission, "account.manage"));
            }

            if(!await _userManager.IsInRoleAsync(defaultAdmin, accountManagerRole.Name))
            {
                await _userManager.AddToRoleAsync(defaultAdmin, accountManagerRole.Name);
            }

            return Ok();
        }
    }
}
